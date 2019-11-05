using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IReflectionHelperService
    {
        MemberInfo Action<T>(Expression<Func<T, Action>> expression);

        MemberInfo Action<T, TParam>(Expression<Func<T, Action<TParam>>> expression);

        MemberInfo Func<T, TResult>(Expression<Func<T, Func<TResult>>> expression);

        MemberInfo Func<T, TParam, TResult>(Expression<Func<T, Func<TParam, TResult>>> expression);
        MemberInfo Func<T, TParam1, TParam2, TResult>(Expression<Func<T, Func<TParam1, TParam2, TResult>>> expression);
        MemberInfo Func<T, TParam1, TParam2, TParam3, TResult>(Expression<Func<T, Func<TParam1, TParam2, TParam3, TResult>>> expression);
        MemberInfo Func<T, TParam1, TParam2, TParam3, TParam4, TResult>(Expression<Func<T, Func<TParam1, TParam2, TParam3, TParam4, TResult>>> expression);
    }
}
