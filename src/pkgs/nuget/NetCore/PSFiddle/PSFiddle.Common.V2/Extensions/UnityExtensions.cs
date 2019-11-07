using MC.Track.TestSuite.Interfaces.Factories;
using MC.Track.TestSuite.Interfaces.Proxies;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Proxies;
using MC.Track.TestSuite.Services.Factories;
using MC.Track.TestSuite.Services.Services;
using MC.Track.TestSuite.Services.Util;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;
using Unity.Interception.ContainerIntegration;
using System.Linq;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using MC.Track.TestSuite.Model.EventArgs;
using PSFiddle.Common.Unity.Extensions;
using Unity.Lifetime;
using System.Runtime.CompilerServices;
using System.Reflection;
using Unity.Interception;
using Unity.Interception.InterceptionBehaviors;
using Unity.Policy;
using Unity.Builder;
using PSFiddle.Common.Configurations;

#if NETCORE
using Microsoft.Extensions.Options;
using Unity.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Unity.Microsoft.DependencyInjection.Lifetime;
#endif

namespace PSFiddle.Common.Extensions
{
   
    public static class UnityExtensions
    {
        

        /// <summary>
        /// Makes the interceptable.
        /// </summary>
        /// <param name="unityContainer">The unity container.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Currently only able to make interfaces interceptable</exception>
        public static object MakeInterceptable(this IUnityContainer unityContainer, Type interfaceType, object obj)
        {
            if (!interfaceType.IsInterface)
                throw new Exception("Currently only able to make interfaces interceptable");

            object ReturnClass = Intercept.ThroughProxy(interfaceType, obj, new InterfaceInterceptor(), new List<IInterceptionBehavior>() { unityContainer.Resolve<IEventBasedProxy>() });
            return ReturnClass;
        }
        /// <summary>
        /// Makes the interceptable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unityContainer">The unity container.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static T MakeInterceptable<T>(this IUnityContainer unityContainer, T obj) where T:class{
            return (T)unityContainer.MakeInterceptable(typeof(T), obj);
        }
        /// <summary>
        /// Makes the interceptable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unityContainer">The unity container.</param>
        /// <param name="typeObject">The type object.</param>
        /// <returns></returns>
        public static IUnityContainer MakeInterceptable<T>(this IUnityContainer unityContainer, ref T typeObject) where T : class
        {
            var obj = (object)typeObject;
            unityContainer.MakeInterceptable(typeof(T), ref obj);
            typeObject = (T)obj;
            return unityContainer;
        }
        /// <summary>
        /// Makes the interceptable.
        /// </summary>
        /// <param name="unityContainer">The unity container.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Currently only able to make interfaces interceptable</exception>
        public static IUnityContainer MakeInterceptable(this IUnityContainer unityContainer, Type interfaceType, ref object obj)
        {
            if (!interfaceType.IsInterface)
                throw new Exception("Currently only able to make interfaces interceptable");

            obj = Intercept.ThroughProxy(interfaceType, obj, new InterfaceInterceptor(), new List<IInterceptionBehavior>() { unityContainer.Resolve<IEventBasedProxy>() });
            return unityContainer;
        }

        /// <summary>
        /// Makes the interface interceptable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unityContainer">The unity container.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static IUnityContainer MakeInterfaceInterceptable<T>(this IUnityContainer unityContainer) where T:class
        {
            if (!typeof(T).IsInterface)
                throw new Exception($"Currently not supporting the interception of a non-interface - {typeof(T).Name}");

            unityContainer.AddExtension(new InterceptSingleServiceExtension(unityContainer.Resolve<IEventBasedProxy>(), typeof(T)));

            return unityContainer;
        }

        /// <summary>
        /// Makes everything interceptable. Anything that was registered using unity will be applied with an attempt of interception.
        ///   Case Not Supporting:
        ///      Non Interfaces
        ///      RegisterInstance registrations
        ///      Named Registrations
        /// </summary>
        /// <param name="unityContainer">The unity container.</param>
        /// <param name="ExcludeTypes">The exclude types.</param>
        /// <param name="IncludeTypes">The include types.</param>
        /// <returns></returns>
        public static IUnityContainer MakeEverythingInterceptable(this IUnityContainer unityContainer, List<Type> ExcludeTypes = null, List<Type> IncludeTypes = null)
        {
            unityContainer.AddExtension(new InterceptorExtension(unityContainer.Resolve<IEventBasedProxy>(), IncludeTypes, ExcludeTypes));
            
            return unityContainer;
        }
        /// <summary>
        /// Setups the interception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unityContainer">The unity container.</param>
        /// <returns></returns>
        public static IFunctionProxyEnabler<T> SetupInterception<T>(this IUnityContainer unityContainer)
        {
            return unityContainer.Resolve<IFunctionProxyEnablerFactory>().Create<T>();
        }

        /// <summary>
        /// Intercepts the interface method calls.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unityContainer">The unity container.</param>
        /// <param name="BeforeSubscription">The before subscription.</param>
        /// <param name="AfterSubscription">The after subscription.</param>
        /// <returns></returns>
        public static IUnityContainer InterceptInterfaceMethodCalls<T>(this IUnityContainer unityContainer, Action<PreInvokedEventBasedProxyEventArgs> BeforeSubscription = null, Action<PostInvokedEventBasedProxyEventArgs> AfterSubscription = null)
        {
            if (BeforeSubscription != null)
                unityContainer.Resolve<IEventBasedProxy>().BeforeCalled(arg =>
                {
                    if (typeof(T).IsInstanceOfType(arg.TargetObj))
                        BeforeSubscription?.Invoke(arg);
                });

            if (AfterSubscription != null)
                unityContainer.Resolve<IEventBasedProxy>().AfterCalled(arg => 
                {
                    if (typeof(T).IsInstanceOfType(arg.TargetObj))
                        AfterSubscription?.Invoke(arg);
                });
            return unityContainer;
        }
        /// <summary>
        /// Intercepts the interface method calls.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unityContainer">The unity container.</param>
        /// <param name="BeforeSubscription">The before subscription.</param>
        /// <param name="AfterSubscription">The after subscription.</param>
        /// <returns></returns>
        public static IUnityContainer InterceptAllInterfaceMethodCalls(this IUnityContainer unityContainer, Action<PreInvokedEventBasedProxyEventArgs> BeforeSubscription = null, Action<PostInvokedEventBasedProxyEventArgs> AfterSubscription = null)
        {
            if (BeforeSubscription != null)
                unityContainer.Resolve<IEventBasedProxy>().BeforeCalled(arg =>
                {
                    BeforeSubscription?.Invoke(arg);
                });

            if (AfterSubscription != null)
                unityContainer.Resolve<IEventBasedProxy>().AfterCalled(arg =>
                {
                    AfterSubscription?.Invoke(arg);
                });
            return unityContainer;
        }
        /// <summary>
        /// Adds the service interception.
        /// </summary>
        /// <param name="unityContainer">The unity container.</param>
        /// <returns></returns>
        public static IUnityContainer AddServiceInterception(this IUnityContainer unityContainer)
        {
            unityContainer.RegisterType<IFunctionProxyEnablerFactory, FunctionProxyEnablerFactory>(new SingletonLifetimeManager());
            unityContainer.RegisterType<IFunctionProxyFactory, FunctionProxyFactory>(new SingletonLifetimeManager());
            unityContainer.RegisterType<ILogger, Logger>(new SingletonLifetimeManager());
            unityContainer.RegisterType<IEventBasedProxy, EventBasedProxy>(new SingletonLifetimeManager());
            unityContainer.RegisterType<ISmartResourceDestroyerServiceFactory, SmartResourceDestroyerServiceFactory>(new SingletonLifetimeManager());
            unityContainer.RegisterType<IDisposableTracker, DisposableTracker>(new SingletonLifetimeManager());
            unityContainer.RegisterType<IGenericScopingFactory, GenericScopingFactory>(new SingletonLifetimeManager());
            // unityContainer.AddNewExtension<Interception>();
            unityContainer.AddExtension(new InterceptorInstancesExtension());
            unityContainer.RegisterType<IConfiguration, Configuration>(new SingletonLifetimeManager());
            return unityContainer;
        }
    }
}
