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
        private readonly IGenericScopingFactory genericScopingFactory;
        private readonly IDisposableTracker disposableTracker;

        public SmartResourceDestroyerServiceFactory(IGenericScopingFactory genericScopingFactory, IDisposableTracker disposableTracker) {
            this.genericScopingFactory = genericScopingFactory;
            this.disposableTracker = disposableTracker;
        }

        public ISmartResourceDestroyerService<T> Create<T>(T resource, Action<T> disposeCallback)
        {
            return new SmartResourceDestroyerService<T>(genericScopingFactory, disposableTracker, resource, disposeCallback);
        }
    }
}
