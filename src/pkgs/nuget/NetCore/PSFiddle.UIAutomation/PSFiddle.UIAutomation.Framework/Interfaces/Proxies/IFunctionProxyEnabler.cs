using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Proxies
{
    public interface IFunctionProxyEnabler<T1>
    {
        IFunctionProxy GeneralFunction(LambdaExpression expression);
        IFunctionProxy Action(Expression<Func<T1, Action>> expression);

        IFunctionProxy Action<TParam>(Expression<Func<T1, Action<TParam>>> expression);
        IFunctionProxy Action<TParam1, TParam2, TParam3, TParam4>(Expression<Func<T1, Action<TParam1, TParam2, TParam3, TParam4>>> expression);

        IFunctionProxy Action<TParam1, TParam2>(Expression<Func<T1, Action<TParam1, TParam2>>> expression);

        IFunctionProxy Func<TResult>(Expression<Func<T1, Func<TResult>>> expression);

        IFunctionProxy Func<TParam, TResult>(Expression<Func<T1, Func<TParam, TResult>>> expression);

        IFunctionProxy Func<TParam1, TParam2, TResult>(Expression<Func<T1, Func<TParam1, TParam2, TResult>>> expression);
        IFunctionProxy Func<TParam1, TParam2, TParam3, TResult>(Expression<Func<T1, Func<TParam1, TParam2, TParam3, TResult>>> expression);
        IFunctionProxy Func<TParam1, TParam2, TParam3, TParam4, TResult>(Expression<Func<T1, Func<TParam1, TParam2, TParam3, TParam4, TResult>>> expression);
        IFunctionProxy Func<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>(Expression<Func<T1, Func<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>>> expression);
        IFunctionProxy Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult>(Expression<Func<T1, Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult>>> expression);
        IFunctionProxy Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult>(Expression<Func<T1, Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult>>> expression);



        ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled(Expression<Func<T1, Action>> expression, Action<T1> callback);
        ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled<T2>(Expression<Func<T1, Action<T2>>> expression, Action<T1,T2> callback);
        ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled<T2, T3>(Expression<Func<T1, Action<T2, T3>>> expression, Action<T1,T2, T3> callback);

        ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled<T2>(Expression<Func<T1, Func<T2>>> expression, Action<T1,T2> callback);


        ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled<T2, T3>(Expression<Func<T1, Func<T2, T3>>> expression, Action<T1,T2, T3> callback);
        ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled<T2, T3, T4>(Expression<Func<T1, Func<T2, T3, T4>>> expression, Action<T1,T2, T3, T4> callback);

        ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> InterceptClassMethodAfterItsCalled<T2, T3, T4, T5>(Expression<Func<T1, Func<T2, T3, T4, T5>>> expression, Action<T1,T2, T3, T4, T5> callback);


        #region Intercept Methods Before Called
        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled(Expression<Func<T1, Action>> expression, Action<T1> callback);
        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled<T2>(Expression<Func<T1, Action<T2>>> expression, Action<T1,T2> callback);
        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled<T2, T3>(Expression<Func<T1, Action<T2, T3>>> expression, Action<T1,T2, T3> callback);

        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled<T2>(Expression<Func<T1, Func<T2>>> expression, Action<T1> callback);

        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled<T2, T3>(Expression<Func<T1, Func<T2, T3>>> expression, Action<T1,T2> callback);
        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled<T2, T3, T4>(Expression<Func<T1, Func<T2, T3, T4>>> expression, Action<T1,T2, T3> callback);

        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> InterceptClassMethodBeforeItsCalled<T2, T3, T4, T5>(Expression<Func<T1, Func<T2, T3, T4, T5>>> expression, Action<T1,T2, T3, T4> callback);
        #endregion



        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod(Expression<Func<T1, Action>> expression, Action<T1> callback);
        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2>(Expression<Func<T1, Action<T2>>> expression, Action<T1,T2> callback);
        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2, T3>(Expression<Func<T1, Action<T2, T3>>> expression, Action<T1,T2, T3> callback);

        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2>(Expression<Func<T1, Func<T2>>> expression, Func<T1, T2> callback);

        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2, T3>(Expression<Func<T1, Func<T2, T3>>> expression, Func<T1, T2, T3> callback);

        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2, T3, T4>(Expression<Func<T1, Func<T2, T3, T4>>> expression, Func<T1, T2, T3, T4> callback);

        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2, T3, T4, T5>(Expression<Func<T1, Func<T2, T3, T4, T5>>> expression, Func<T1, T2, T3, T4, T5> callback);
        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2, T3, T4, T5, T6>(Expression<Func<T1, Func<T2, T3, T4, T5, T6>>> expression, Func<T1, T2, T3, T4, T5, T6> callback);
        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2, T3, T4, T5, T6, T7>(Expression<Func<T1, Func<T2, T3, T4, T5, T6, T7>>> expression, Func<T1, T2, T3, T4, T5, T6, T7> callback);
        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2, T3, T4, T5, T6, T7, T8>(Expression<Func<T1, Func<T2, T3, T4, T5, T6, T7, T8>>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8> callback);
        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> OverrideClassMethod<T2, T3, T4, T5>(Expression<Func<T1, Action<T2, T3, T4, T5>>> expression, Action<T1, T2, T3, T4, T5> callback);
    }
}
