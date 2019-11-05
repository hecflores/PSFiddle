using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Events;
using Unity.Extension;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.InterceptionBehaviors;

namespace MC.Track.TestSuite.Toolkit
{
    public class InterceptorExtension : UnityContainerExtension
    {
        Type _interceptorType;
        IInterceptionBehavior _interceptor;
        public InterceptorExtension(IInterceptionBehavior interceptor, Type interceptorType = null)
        {
            this._interceptorType = interceptorType;
            this._interceptor = interceptor;
        }
        private void ApplyToAlreadyRegisteredServices()
        {

            foreach (var registration in Context.Container.Registrations)
            {
                if (this._interceptorType == null || registration.RegisteredType.Equals(this._interceptorType))
                {
                    var member = new InterceptionBehavior(_interceptor);
                    member.AddPolicies(registration.RegisteredType, registration.MappedToType, registration.Name, Context.Policies);
                }
            }
        }
        protected override void Initialize()
        {
            Context.Registering += Registering;
            this.ApplyToAlreadyRegisteredServices();
        }

        private void Registering(object sender, RegisterEventArgs e)
        {
            if (this._interceptorType == null || e.TypeFrom.Equals(this._interceptorType))
            {
                var member = new InterceptionBehavior(_interceptor);
                member.AddPolicies(e.TypeFrom, e.TypeTo, e.Name, Context.Policies);
            }
           
        }
    }
}
