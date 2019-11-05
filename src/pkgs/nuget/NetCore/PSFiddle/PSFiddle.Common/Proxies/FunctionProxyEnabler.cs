using MC.Track.TestSuite.Interfaces.Factories;
using MC.Track.TestSuite.Interfaces.Proxies;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Proxies
{
    public class FunctionProxyEnabler<T1> : IFunctionProxyEnabler<T1>
    {
        public FunctionProxyEnabler(IFunctionProxyFactory factory)
        {
            this.factory = factory;
        }
        private static bool IsNET45 = Type.GetType("System.Reflection.ReflectionContext", false) != null;
        private readonly IFunctionProxyFactory factory;

        public IFunctionProxy GeneralFunction(LambdaExpression expression)
        {
            var unaryExpression = (UnaryExpression)expression.Body;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;

            if (IsNET45)
            {
                var methodCallObject = (ConstantExpression)methodCallExpression.Object;
                var methodInfo = (MethodInfo)methodCallObject.Value;
                return factory.Create<T1>(methodInfo);
            }
            else
            {
                var methodInfoExpression = (ConstantExpression)methodCallExpression.Arguments.Last();
                var methodInfo = (MemberInfo)methodInfoExpression.Value;
                return factory.Create<T1>(methodInfo);
            }
        }
        public IFunctionProxy Action(Expression<Func<T1, Action>> expression)
        {
            return GeneralFunction(expression);
        }

        public IFunctionProxy Action<TParam>(Expression<Func<T1, Action<TParam>>> expression)
        {
            return GeneralFunction(expression);
        }
        public IFunctionProxy Action<TParam1, TParam2>(Expression<Func<T1, Action<TParam1, TParam2>>> expression)
        {
            return GeneralFunction(expression);
        }
        public IFunctionProxy Func<TResult>(Expression<Func<T1, Func<TResult>>> expression)
        {
            return GeneralFunction(expression);
        }

        public IFunctionProxy Func<TParam, TResult>(Expression<Func<T1, Func<TParam, TResult>>> expression)
        {
            return GeneralFunction(expression);
        }
        public IFunctionProxy Func<TParam1, TParam2, TResult>(Expression<Func<T1, Func<TParam1, TParam2, TResult>>> expression)
        {
            return GeneralFunction(expression);
        }
        public IFunctionProxy Func<TParam1, TParam2, TParam3, TResult>(Expression<Func<T1, Func<TParam1, TParam2, TParam3, TResult>>> expression)
        {
            return GeneralFunction(expression);
        }
        public IFunctionProxy Func<TParam1, TParam2, TParam3, TParam4, TResult>(Expression<Func<T1, Func<TParam1, TParam2, TParam3, TParam4, TResult>>> expression)
        {
            return GeneralFunction(expression);
        }



        public ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled(Expression<Func<T1, Action>> expression, Action<T1> callback)

        {
            return Action(expression).AfterCalled(arg =>
            {
                callback((T1)arg.TargetObj);
            });
        }
        public ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled<T2>(Expression<Func<T1, Action<T2>>> expression, Action<T1, T2> callback)

        {
            return Action(expression).AfterCalled(arg =>
            {
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                callback((T1)arg.TargetObj, argument1);
            });
        }
        public ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled<T2, T3>(Expression<Func<T1, Action<T2, T3>>> expression, Action<T1, T2, T3> callback)

        {
            return Action(expression).AfterCalled(arg =>
            {
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                var argument2 = (T3)arg.GetArgmentByIndex(1);
                callback((T1)arg.TargetObj, argument1, argument2);
            });
        }

        public ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled<T2>(Expression<Func<T1, Func<T2>>> expression, Action<T1, T2> callback)

        {
            return Func(expression).AfterCalled(arg =>

            {
                var returnObj = (T2)arg.ReturnObj;
                callback((T1)arg.TargetObj, returnObj);
            });
        }

        public ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled<T2, T3>(Expression<Func<T1, Func<T2, T3>>> expression, Action<T1, T2, T3> callback)

        {
            return Func(expression).AfterCalled(arg =>

            {
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                var returnObj = (T3)arg.ReturnObj;
                callback((T1)arg.TargetObj, argument1, returnObj);
            });
        }
        public ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled<T2, T3, T4>(Expression<Func<T1, Func<T2, T3, T4>>> expression, Action<T1, T2, T3, T4> callback)

        {
            return Func(expression).AfterCalled(arg =>

            {
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                var argument2 = (T3)arg.GetArgmentByIndex(0);
                var returnObj = (T4)arg.ReturnObj;
                callback((T1)arg.TargetObj, argument1, argument2, returnObj);
            });
        }

        public ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled<T2, T3, T4, T5>(Expression<Func<T1, Func<T2, T3, T4, T5>>> expression, Action<T1, T2, T3, T4, T5> callback)

        {
            return Func(expression).AfterCalled(arg =>

            {
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                var argument2 = (T3)arg.GetArgmentByIndex(0);
                var argument3 = (T4)arg.GetArgmentByIndex(0);
                var returnObj = (T5)arg.ReturnObj;
                callback((T1)arg.TargetObj, argument1, argument2, argument3, returnObj);
            });
        }


        #region Intercept Methods Before Called
        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled(Expression<Func<T1, Action>> expression, Action<T1> callback)

        {
            return Action(expression).BeforeCalled(arg =>

            {
                callback((T1)arg.TargetObj);
            });
        }
        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled<T2>(Expression<Func<T1, Action<T2>>> expression, Action<T1, T2> callback)

        {
            return Action(expression).BeforeCalled(arg =>

            {
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                callback((T1)arg.TargetObj, argument1);
            });
        }
        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled<T2, T3>(Expression<Func<T1, Action<T2, T3>>> expression, Action<T1, T2, T3> callback)

        {
            return Action(expression).BeforeCalled(arg =>

            {
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                var argument2 = (T3)arg.GetArgmentByIndex(1);
                callback((T1)arg.TargetObj, argument1, argument2);
            });
        }

        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled<T2>(Expression<Func<T1, Func<T2>>> expression, Action<T1> callback)

        {
            return Func(expression).BeforeCalled(arg =>

            {
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                callback((T1)arg.TargetObj);
            });
        }

        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled<T2, T3>(Expression<Func<T1, Func<T2, T3>>> expression, Action<T1, T2> callback)

        {
            return Func(expression).BeforeCalled(arg =>

            {
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                callback((T1)arg.TargetObj, argument1);
            });
        }
        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled<T2, T3, T4>(Expression<Func<T1, Func<T2, T3, T4>>> expression, Action<T1, T2, T3> callback)

        {
            return Func(expression).BeforeCalled(arg =>

            {
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                var argument2 = (T3)arg.GetArgmentByIndex(0);
                callback((T1)arg.TargetObj, argument1, argument2);
            });
        }

        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled<T2, T3, T4, T5>(Expression<Func<T1, Func<T2, T3, T4, T5>>> expression, Action<T1, T2, T3, T4> callback)

        {
            return Func(expression).BeforeCalled(arg =>

            {
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                var argument2 = (T3)arg.GetArgmentByIndex(0);
                var argument3 = (T4)arg.GetArgmentByIndex(0);
                callback((T1)arg.TargetObj, argument1, argument2, argument3);
            });
        }
        #endregion



        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod(Expression<Func<T1, Action>> expression, Action<T1> callback)

        {
            return Action(expression).BeforeCalled(arg =>
            {
                arg.Execute = false;
                callback((T1)arg.TargetObj);
            });
        }
        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2>(Expression<Func<T1, Action<T2>>> expression, Action<T1, T2> callback)

        {
            return Action(expression).BeforeCalled(arg =>

            {
                arg.Execute = false;
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                callback((T1)arg.TargetObj, argument1);
            });
        }
        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2, T3>(Expression<Func<T1, Action<T2, T3>>> expression, Action<T1, T2, T3> callback)

        {
            return Action(expression).BeforeCalled(arg =>

            {
                arg.Execute = false;
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                var argument2 = (T3)arg.GetArgmentByIndex(1);
                callback((T1)arg.TargetObj, argument1, argument2);
            });
        }

        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2>(Expression<Func<T1, Func<T2>>> expression, Func<T1, T2> callback)

        {
            return Func(expression).BeforeCalled(arg =>

            {
                arg.Execute = false;
                arg.ReturnObj = callback((T1)arg.TargetObj);
            });
        }

        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2, T3>(Expression<Func<T1, Func<T2, T3>>> expression, Func<T1, T2, T3> callback)

        {
            return Func(expression).BeforeCalled(arg =>

            {
                arg.Execute = false;
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                arg.ReturnObj = callback((T1)arg.TargetObj, argument1);
            });
        }

        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2, T3, T4>(Expression<Func<T1, Func<T2, T3, T4>>> expression, Func<T1, T2, T3, T4> callback)

        {
            return Func(expression).BeforeCalled(arg =>

            {
                arg.Execute = false;
                var argument1 = (T2)arg.GetArgmentByIndex(0);
                var argument2 = (T3)arg.GetArgmentByIndex(0);
                arg.ReturnObj = callback((T1)arg.TargetObj, argument1, argument2);
            });
        }

        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2, T3, T4, T5>(Expression<Func<T1, Func<T2, T3, T4, T5>>> expression, Func<T1, T2, T3, T4, T5> callback) => OverrideClassMethod(expression, callback.Method, typeof(T5), new List<Type>() { typeof(T2) });

        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod(LambdaExpression expression, MethodInfo callback, Type ReturnType, List<Type> ParameterArguments)
        {
            return GeneralFunction(expression).BeforeCalled(arg =>
            {
                if (arg.MethodBase.GetParameters().Count() != ParameterArguments.Count())
                {
                    // this._logger.LogTrace($"Override Class Method '{callback.Name}' with return type '{ReturnType.Name}' and parameters of '{String.Join(",", ParameterArguments.Select(b => b.Name))}' failed because the paramerters did not match");
                    return;
                }


                for (var i = 0; i < arg.MethodBase.GetParameters().Count(); i++)
                {
                    if (arg.MethodBase.GetParameters()[i].ParameterType != ParameterArguments[i])
                        return;
                }

                var foundReturnType = (arg.MethodBase as MethodInfo)?.ReturnType ?? typeof(void);
                var expectedReturnType = ReturnType;

                if (foundReturnType != expectedReturnType)
                {
                    // this._logger.LogTrace($"Override Class Method '{callback.Name}' with return type '{ReturnType.Name}' and parameters of '{String.Join(",", ParameterArguments.Select(b => b.Name))}' failed because the return type did not match");
                    return;
                }

                arg.Execute = false;
                if (expectedReturnType == typeof(void))
                    callback.Invoke(null, (new List<Type>() { typeof(T1) }.Union(ParameterArguments.Select((b, i) => arg.GetArgmentByIndex(i))).ToArray()));
                else
                    arg.ReturnObj = callback.Invoke(null, (new List<Type>() { typeof(T1) }.Union(ParameterArguments.Select((b, i) => arg.GetArgmentByIndex(i))).ToArray()));
            });
        }
    }
}
