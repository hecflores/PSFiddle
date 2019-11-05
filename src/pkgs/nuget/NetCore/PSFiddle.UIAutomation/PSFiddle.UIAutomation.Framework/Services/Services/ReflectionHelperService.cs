using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    [AthenaRegister(typeof(IReflectionHelperService), Model.Enums.AthenaRegistrationType.Singleton)]
    public class ReflectionHelperService : IReflectionHelperService
    {
        private static bool IsNET45 = Type.GetType("System.Reflection.ReflectionContext", false) != null;
        private MemberInfo CreateFunctionProxt(LambdaExpression expression)
        {
            var unaryExpression = (UnaryExpression)expression.Body;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;

            if (IsNET45)
            {
                var methodCallObject = (ConstantExpression)methodCallExpression.Object;
                var methodInfo = (MethodInfo)methodCallObject.Value;
                return methodInfo;
            }
            else
            {
                var methodInfoExpression = (ConstantExpression)methodCallExpression.Arguments.Last();
                var methodInfo = (MemberInfo)methodInfoExpression.Value;
                return methodInfo;
            }
        }
        public MemberInfo Action<T>(Expression<Func<T, Action>> expression)
        {
            return CreateFunctionProxt(expression);
        }

        public MemberInfo Action<T, TParam>(Expression<Func<T, Action<TParam>>> expression)
        {
            return CreateFunctionProxt(expression);
        }

        public MemberInfo Func<T, TResult>(Expression<Func<T, Func<TResult>>> expression)
        {
            return CreateFunctionProxt(expression);
        }

        public MemberInfo Func<T, TParam, TResult>(Expression<Func<T, Func<TParam, TResult>>> expression)
        {
            return CreateFunctionProxt(expression);
        }
        public MemberInfo Func<T, TParam1, TParam2, TResult>(Expression<Func<T, Func<TParam1, TParam2, TResult>>> expression)
        {
            return CreateFunctionProxt(expression);
        }
        public MemberInfo Func<T, TParam1, TParam2, TParam3, TResult>(Expression<Func<T, Func<TParam1, TParam2, TParam3, TResult>>> expression)
        {
            return CreateFunctionProxt(expression);
        }
        public MemberInfo Func<T, TParam1, TParam2, TParam3, TParam4, TResult>(Expression<Func<T, Func<TParam1, TParam2, TParam3, TParam4, TResult>>> expression)
        {
            return CreateFunctionProxt(expression);
        }
    }
}
