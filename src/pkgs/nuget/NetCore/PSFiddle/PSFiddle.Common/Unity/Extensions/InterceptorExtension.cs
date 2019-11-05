using PSFiddle.Common.Extensions;
using MC.Track.TestSuite.Interfaces.Proxies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Builder;
using Unity.Events;
using Unity.Extension;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.Interceptors;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.Lifetime;
using Unity.Policy;
using Unity.Resolution;

namespace PSFiddle.Common.Unity.Extensions
{
    public class InterceptorExtension : UnityContainerExtension
    {
        IInterceptionBehavior _interceptor;
        private readonly List<Type> includeTypes;
        private readonly List<Type> excludeTypes;

        public InterceptorExtension(IInterceptionBehavior interceptor, List<Type> IncludeTypes = null, List<Type> ExcludeTypes = null)
        {
            this._interceptor = interceptor;
            includeTypes = IncludeTypes==null?new List<Type>(): IncludeTypes;
            excludeTypes = ExcludeTypes==null?new List<Type>(): ExcludeTypes;
        }
        private bool IsAssemblyLoaded(string fullName, out Assembly lAssembly)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                if (assembly.FullName == fullName)
                {
                    lAssembly = assembly;
                    return true;
                }
            }

            lAssembly = null;
            return false;
        }
        public bool CanIntercept(Type t)
        {
            return t.IsClass && !t.IsSealed;
        }
        private bool TestMapFrom(Type type)
        {
            
            Trace.WriteLineIf(!AppDomain.CurrentDomain.GetAssemblies().Contains(type.Assembly), $"Assembly {type.Assembly.FullName} for type {type.FullName} was not in current domain");


            return type.IsPublic && type.IsInterface && !type.IsAnonymousType() && type != typeof(IEventBasedProxy);//  && !type.FullName.Contains("Microsoft.Extensions") && !type.FullName.Contains("Microsoft.AspNetCore") && !type.FullName.Contains("Swashbuckle.AspNetCore");
        }
        private bool TestMapTo(Type type)
        {
            
            Trace.WriteLineIf(!AppDomain.CurrentDomain.GetAssemblies().Contains(type.Assembly), $"Assembly {type.Assembly.FullName} for type {type.FullName} was not in current domain");

            return !type.IsInterface && !type.IsAnonymousType();// &&  !type.FullName.Contains("System.Private.CoreLib") && !type.FullName.Contains("Version=") && !type.FullName.Contains("`1") && CanIntercept(type);
        }

        private void ApplyToAlreadyRegisteredServices(IUnityContainer container)
        {
            foreach (var registration in container.Registrations)
            {
                if (!this.excludeTypes.Contains(registration.RegisteredType) && (includeTypes.Count == 0 || includeTypes.Contains(registration.RegisteredType)))
                {
                    try
                    {
                        
                        if (TestMapFrom(registration.RegisteredType) && TestMapTo(registration.MappedToType))
                        {
                            Trace.WriteLine($"Registering - {registration.RegisteredType.FullName} > {registration.MappedToType.FullName} ");

                            var interceptorEnabler = new Interceptor<InterfaceInterceptor>();
                            interceptorEnabler.AddPolicies(registration.RegisteredType, registration.MappedToType, registration.Name, Context.Policies);
                        
                            var interceptor = new InterceptionBehavior(_interceptor);
                            interceptor.AddPolicies(registration.RegisteredType, registration.MappedToType, registration.Name, Context.Policies);
                        }

                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine($"Exception: {e.Message}");
                    }
                }
            }
            if(container.Parent !=null)
                ApplyToAlreadyRegisteredServices(container.Parent);
        }
        protected override void Initialize()
        {
            Context.ChildContainerCreated += Context_ChildContainerCreated;
            // Context.Registering += Registering;
            // Context.RegisteringInstance += Context_RegisteringInstance;
            // this.ApplyToAlreadyRegisteredServices(Context.Container);
        }

        
        private void Context_ChildContainerCreated(object sender, ChildContainerCreatedEventArgs e)
        {
            e.ChildContainer.RegisterInstance<IUnityContainer>("ParentContainer", Context.Container, new ContainerControlledLifetimeManager());
            e.ChildContainer.MakeEverythingInterceptable(this.excludeTypes, this.includeTypes);
        }

        private void ClearPolicies(Type from, String Name)
        {
            var clearThese = Context.Container.Registrations.Where(b => b.RegisteredType == from && b.Name == Name);
            foreach(var registration in clearThese)
            {
                Context.Policies.Clear(registration.RegisteredType, registration.Name, typeof(IInterceptor));
                Context.Policies.Clear(registration.RegisteredType, registration.Name, typeof(IInterceptionBehavior));
            }
        }
        private void Context_RegisteringInstance(object sender, RegisterInstanceEventArgs e)
        {
            if (!this.excludeTypes.Contains(e.RegisteredType) && (includeTypes.Count == 0 || includeTypes.Contains(e.RegisteredType)))
            {
                try
                {
                    if (TestMapFrom(e.RegisteredType) )
                    {
                        Trace.WriteLine($"Registering - {e.RegisteredType.FullName} > {e.Instance.GetType()} : '{e.Name}' ");

                        var interceptorEnabler = new Interceptor<InterfaceInterceptor>();
                        interceptorEnabler.AddPolicies(e.RegisteredType, e.Instance.GetType(), e.Name, Context.Policies);

                        var interceptor = new InterceptionBehavior(_interceptor);
                        interceptor.AddPolicies(e.RegisteredType, e.Instance.GetType(), e.Name, Context.Policies);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Exception: {ex.Message}");
                }

            }

            
        }

        private void Registering(object sender, RegisterEventArgs e)
        {
            
            if (!this.excludeTypes.Contains(e.TypeFrom) && (includeTypes.Count == 0 || includeTypes.Contains(e.TypeFrom)) )
            {
                try
                {
                    if (TestMapFrom(e.TypeFrom) && TestMapTo(e.TypeTo) && Context.Container.Registrations.Count(b => b.RegisteredType == e.TypeFrom && b.Name == e.Name) <= 1)
                    {
                        Trace.WriteLine($"Registering - {e.TypeFrom.FullName} > {e.TypeTo.FullName} : '{e.Name}' ");

                        var interceptorEnabler = new Interceptor<InterfaceInterceptor>();
                        interceptorEnabler.AddPolicies(e.TypeFrom, e.TypeTo, e.Name, Context.Policies);
                        
                        var interceptor = new InterceptionBehavior(_interceptor);
                        interceptor.AddPolicies(e.TypeFrom, e.TypeTo, e.Name, Context.Policies);
                    }
                }
                catch(Exception ex)
                {
                    Trace.WriteLine($"Exception: {ex.Message}");
                }
                
            }
           
        }

    }
}
