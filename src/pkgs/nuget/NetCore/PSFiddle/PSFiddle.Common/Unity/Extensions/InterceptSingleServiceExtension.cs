using MC.Track.TestSuite.Interfaces.Proxies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Events;
using Unity.Extension;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity;

namespace PSFiddle.Common.Unity.Extensions
{
    public class InterceptSingleServiceExtension : UnityContainerExtension
    {
        IInterceptionBehavior _interceptor;
        private readonly Type injectType;

        public InterceptSingleServiceExtension(IInterceptionBehavior interceptor, Type injectType)
        {
            this._interceptor = interceptor;
            this.injectType = injectType;
        }
        private void ApplyToAlreadyRegisteredServices(IUnityContainer container)
        {
            var wantedInterceptions = container.Registrations.Where(b => b.RegisteredType == this.injectType);
            foreach (var registration in wantedInterceptions)
            {
                try
                {
                    if (registration.RegisteredType.IsInterface)
                    {
                        Trace.WriteLine($"Registering - {registration.RegisteredType.FullName} > {registration.MappedToType.FullName} '{registration.Name}'");

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

            if (container.Parent != null)
                ApplyToAlreadyRegisteredServices(container.Parent);
        }
        protected override void Initialize()
        {
            Context.ChildContainerCreated += Context_ChildContainerCreated;
            Context.Registering += Registering;
            this.ApplyToAlreadyRegisteredServices(Context.Container);
        }
        private void Context_ChildContainerCreated(object sender, ChildContainerCreatedEventArgs e)
        {
            e.ChildContainer.AddExtension(new InterceptSingleServiceExtension(e.ChildContainer.Resolve<IEventBasedProxy>(), this.injectType));
        }
        private void Registering(object sender, RegisterEventArgs e)
        {
            if (e.TypeFrom == this.injectType)
            {
                try
                {
                    if (e.TypeFrom.IsInterface)
                    {
                        var interceptorEnabler = new Interceptor<InterfaceInterceptor>();
                        interceptorEnabler.AddPolicies(e.TypeFrom, e.TypeTo, e.Name, Context.Policies);

                        var interceptor = new InterceptionBehavior(_interceptor);
                        interceptor.AddPolicies(e.TypeFrom, e.TypeTo, e.Name, Context.Policies);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Exception: {ex.Message}");
                }

            }

        }


    }
}
