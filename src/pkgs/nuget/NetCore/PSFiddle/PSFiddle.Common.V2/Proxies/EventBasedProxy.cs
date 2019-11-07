using MC.Track.TestSuite.Interfaces.Proxies;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Model.EventArgs;
using System;
using System.Collections.Generic;
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

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var preInvokeArgs = new PreInvokedEventBasedProxyEventArgs();
            preInvokeArgs.FunctionName = input.MethodBase.Name;
            preInvokeArgs.TargetObj = input.Target;
            preInvokeArgs.MethodBase = input.MethodBase;
            preInvokeArgs.GetArgmentByName = (argName) => input.Arguments[argName];
            preInvokeArgs.GetArgmentByIndex = (argIndex) => input.Arguments[argIndex];
            preInvokeArgs.Execute = true;

            // Discovery Pre Invoked Inturuptions
            this.PreInvokedSubscription?.Invoke(this, preInvokeArgs);

            if (preInvokeArgs.OutputVariables != null) throw new NotImplementedException("Currently have not implmented the use of output variables in AOP");

            IMethodReturn functionCallResults = null;
            if (!preInvokeArgs.Execute)
            {
                functionCallResults = input.CreateMethodReturn(preInvokeArgs.ReturnObj); // Fake Function Call
                functionCallResults.Exception = preInvokeArgs.Exception;
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

            return functionCallResults;
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
