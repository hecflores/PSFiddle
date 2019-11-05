using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Helpers
{
    public static class MethodSupport<T>
    {
        public static LambdaExpression Action(Expression<Func<T, Action>> expression)
        {
            return expression;
        }

        public static LambdaExpression Action<TParam>(Expression<Func<T, Action<TParam>>> expression)
        {
            return expression;
        }

        public static LambdaExpression Func<TResult>(Expression<Func<T, Func<TResult>>> expression)
        {
            return expression;
        }

        public static LambdaExpression Func<TParam, TResult>(Expression<Func<T, Func<TParam, TResult>>> expression)
        {
            return expression;
        }

    }
}
