using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
     public class SmartResourceDestroyerServiceFactory : ISmartResourceDestroyerServiceFactory
    {
        private readonly IResolver resolver;

        public SmartResourceDestroyerServiceFactory(IResolver resolver) {
            this.resolver = resolver;
        }

        public ISmartResourceDestroyerService<T> Create<T>(T resource, Action<T> disposeCallback)
        {
            return new SmartResourceDestroyerService<T>(resolver, resource, disposeCallback);
        }
    }
}
