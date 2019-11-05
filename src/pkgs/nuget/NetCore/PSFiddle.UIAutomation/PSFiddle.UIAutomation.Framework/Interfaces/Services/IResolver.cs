using MC.Track.TestSuite.Interfaces.Proxies;
using MC.Track.TestSuite.Model.Enums;
using MC.Track.TestSuite.Model.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Interception.InterceptionBehaviors;
using Unity.Registration;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IResolver : IDisposable
    {
        
        T ApplyIntercepts<T>(T obj) where T : class;
        void UseLogging(params FrameworkType[] types);
        void SubscribeToResolver(Action<PreInvokedEventBasedProxyEventArgs> callback);
        IFunctionProxyEnabler<T> InterceptClass<T>();
        T Resolve<T>() where T : class;
        T Resolve<T>(String Name) where T : class;
        void RegisterAll(Assembly assembly);
        void RegisterType<TInstance>();
        void RegisterType<TInterface, TInstance>(params InjectionMember[] injectionMembers) where TInstance : TInterface;
        void RegisterType<TInterface, TInstance>(Func<IResolver, TInterface> factory, params InjectionMember[] injectionMembers) where TInstance : TInterface;
        void RegisterType<TInterface, TInstance>(Object[] contructorParameters, params InjectionMember[] injectionMembers) where TInstance : TInterface;
        void RegisterType<TInterface, TInstance>(String Name) where TInstance : TInterface;
        void RegisterType<TInterface, TInstance>(String Name, Object[] contructorParameters, params InjectionMember[] injectionMembers) where TInstance : TInterface;
        void RegisterType<TInterface, TInstance>(String Name, Func<IResolver, TInterface> factory, params InjectionMember[] injectionMembers) where TInstance : TInterface;
        void RegisterType<TInterface, TInstance>(String Name, TInstance instance, params InjectionMember[] injectionMembers) where TInstance : TInterface;
        void RegisterType<TInstance>(TInstance instance);
        void RegisterType<TInstance>(String Name, TInstance instance);

        void RegisterInstance<TInterface>(TInterface instance);
        void RegisterSingleton<TInstance>();
        void RegisterSingleton<TInstance>(TInstance instance);
        void RegisterSingleton<TInstance>(String Name, TInstance instance);
        void RegisterSingleton<TInterface, TInstance>(params InjectionMember[] injectionMembers) where TInstance : TInterface;
        void RegisterSingleton<TInterface, TInstance>(TInterface instance, params InjectionMember[] injectionMembers) where TInstance : TInterface;
        void RegisterSingleton<TInterface>(Func<IResolver, TInterface> factory, params InjectionMember[] injectionMembers);
        void RegisterSingleton<TInterface, TInstance>(Object[] contructorParameters, params InjectionMember[] injectionMembers) where TInstance : TInterface;
        void RegisterSingleton<TInterface, TInstance>(String Name) where TInstance : TInterface;
        void RegisterSingleton<TInterface, TInstance>(String Name, TInterface instance, params InjectionMember[] injectionMembers) where TInstance : TInterface;
        void RegisterSingleton<TInterface, TInstance>(String Name, Func<IResolver, TInterface> factory, params InjectionMember[] injectionMembers) where TInstance : TInterface;
        void RegisterSingleton<TInterface, TInstance>(String Name, Object[] contructorParameters, params InjectionMember[] injectionMembers) where TInstance : TInterface;

        void RegisterSingleton(Type from, Type to, String name, Object[] contructorArguments);
        void RegisterType(Type from, Type to, String name, Object[] contructorArguments);
        void UseInterceptor(IInterceptionBehavior interceptor);
        void UseInterceptor<T>() where T : class, IInterceptionBehavior;
    }
}
