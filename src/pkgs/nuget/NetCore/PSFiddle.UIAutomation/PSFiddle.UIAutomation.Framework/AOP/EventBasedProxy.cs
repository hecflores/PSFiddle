using MC.Track.TestSuite.Interfaces.Proxies;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Model.EventArgs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace MC.Track.TestSuite.Proxies
{
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    public class EventBasedProxy : IEventBasedProxy, IInterceptionBehavior
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private readonly ISmartResourceDestroyerServiceFactory smartResourceDestroyerServiceFactory;

        public EventBasedProxy(ISmartResourceDestroyerServiceFactory smartResourceDestroyerServiceFactory)
        {
            this.smartResourceDestroyerServiceFactory = smartResourceDestroyerServiceFactory;
        }

        public bool WillExecute => true;

        public EventHandler<PreInvokedEventBasedProxyEventArgs> PreInvokedSubscription { get; set; }

        public EventHandler<PostInvokedEventBasedProxyEventArgs> PostInvokedSubscription { get; set; }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }
        private Task CreateGenericWrapperTask<T>(Task task, IMethodInvocation input)
        {
          return this.DoCreateGenericWrapperTask<T>((Task<T>) task, input);
        }
        private readonly ConcurrentDictionary<Type, Func<Task, IMethodInvocation, Task>> wrapperCreators = new ConcurrentDictionary<Type, Func<Task,IMethodInvocation, Task>>();
        private Func<Task, IMethodInvocation, Task> GetWrapperCreator(Type taskType)
        {
            return this.wrapperCreators.GetOrAdd( taskType, (Type t) => {
                  if (t == typeof(Task))
                  {
                      return this.CreateWrapperTask;
                  }
                  else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>))
                  {
                      return (Func<Task, IMethodInvocation, Task>)this.GetType().GetMethod("CreateGenericWrapperTask",BindingFlags.Instance | BindingFlags.NonPublic)
                                .MakeGenericMethod(new Type[] { t.GenericTypeArguments[0] })
                                .CreateDelegate(typeof(Func<Task, IMethodInvocation, Task>), this);
                  }
                  else
                  {
                    // Other cases are not supported
                    return (task, _) => task;
                  }
              });
        }
        private async Task<T> DoCreateGenericWrapperTask<T>(Task<T> task, IMethodInvocation input)
        {
            try
            {
                T value = await task.ConfigureAwait(false);
                Trace.TraceInformation("Successfully finished async operation {0} with value: {1}",
                  input.MethodBase.Name, value);
                return value;
            }
            catch (Exception e)
            {
                Trace.TraceWarning("Async operation {0} threw: {1}", input.MethodBase.Name, e);
                throw;
            }
        }
        private async Task CreateWrapperTask(Task task, IMethodInvocation input)
        {
            try
            {
                await task.ConfigureAwait(false);
                Trace.TraceInformation("Successfully finished async operation {0}",
                  input.MethodBase.Name);
            }
            catch (Exception e)
            {
                Trace.TraceWarning("Async operation {0} threw: {1}",
                  input.MethodBase.Name, e);
                throw;
            }
        }
        public IMethodReturn CoverSpecialMethods(IMethodInvocation input, IMethodReturn value)
        {
            var method = input.MethodBase as MethodInfo;
            if (value.ReturnValue != null && method != null && typeof(Task).IsAssignableFrom(method.ReturnType))
            {
                // If this method returns a Task, override the original return value
                var task = (Task)value.ReturnValue;
                return input.CreateMethodReturn( this.GetWrapperCreator(method.ReturnType)(task, input), value.Outputs);
            }
            return value;
        }
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var preInvokeArgs = new PreInvokedEventBasedProxyEventArgs();
            preInvokeArgs.FunctionName = input.MethodBase.Name;
            preInvokeArgs.TargetObj = input.Target;
            preInvokeArgs.MethodBase = input.MethodBase;
            preInvokeArgs.GetArgmentByName = (argName) => input.Arguments[argName];
            preInvokeArgs.GetArgmentByIndex = (argIndex) => input.Arguments[argIndex];
            preInvokeArgs.SetArgumentByIndex = (argIndex, obj) => input.Arguments[argIndex] = obj;
            preInvokeArgs.SetArgumentByName = (argName, obj) => input.Arguments[argName] = obj;
            preInvokeArgs.Execute = true;
            
            // Discovery Pre Invoked Inturuptions
            this.PreInvokedSubscription?.Invoke(this, preInvokeArgs);

            if (preInvokeArgs.OutputVariables != null) throw new NotImplementedException("Currently have not implmented the use of output variables in AOP");

            IMethodReturn functionCallResults = null;
            if (!preInvokeArgs.Execute) {
                functionCallResults = CoverSpecialMethods(input, input.CreateMethodReturn(preInvokeArgs.ReturnObj)); // Fake Function Call
            }
            else
                functionCallResults = getNext().Invoke(input, getNext);         // Real Functional Call

            // Setup Post Invoke Args
            var postInvokeArgs = new PostInvokedEventBasedProxyEventArgs(preInvokeArgs.Execute);
            postInvokeArgs.MethodBase = input.MethodBase;
            postInvokeArgs.FunctionName = input.MethodBase.Name;
            postInvokeArgs.TargetObj = input.Target;
            postInvokeArgs.GetArgmentByName = (argName) => input.Arguments[argName];
            postInvokeArgs.GetArgmentByIndex = (argIndex) => input.Arguments[argIndex];
            postInvokeArgs.ReturnObj = functionCallResults.ReturnValue;
            postInvokeArgs.Exception = functionCallResults.Exception;
            
            // Post Overrides
            this.PostInvokedSubscription?.Invoke(this, postInvokeArgs);
            functionCallResults.Exception = postInvokeArgs.Exception;
            functionCallResults.ReturnValue = postInvokeArgs.ReturnObj;

            // Call it again if asked for
            if (postInvokeArgs.CallAgain)
                functionCallResults = Invoke(input, getNext);

            return CoverSpecialMethods(input, functionCallResults);
        }

        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> BeforeCalled(Action<PreInvokedEventBasedProxyEventArgs> args)
        {
            EventHandler<PreInvokedEventBasedProxyEventArgs> thisEvent = (sender, arg) =>
            {
                args(arg);
            };
            this.PreInvokedSubscription += thisEvent;

            return this.smartResourceDestroyerServiceFactory.Create(thisEvent, (remove) =>
            {
                this.PreInvokedSubscription -= remove;
            });
        }
        public ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> AfterCalled(Action<PostInvokedEventBasedProxyEventArgs> args)
        {
            EventHandler<PostInvokedEventBasedProxyEventArgs> thisEvent = (sender, arg) =>
            {
                args(arg);
            };
            this.PostInvokedSubscription += thisEvent;

            return this.smartResourceDestroyerServiceFactory.Create(thisEvent, (remove) =>
            {
                this.PostInvokedSubscription -= remove;
            });
        }
    }
}
