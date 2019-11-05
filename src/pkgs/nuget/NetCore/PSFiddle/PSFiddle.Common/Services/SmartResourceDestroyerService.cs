using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class SmartResourceDestroyerService<T> : ISmartResourceDestroyerService<T>
    {
        private readonly IGenericScopingFactory genericScopingFactory;
        private readonly IDisposableTracker disposableTracker;
        private readonly T resource;
        private readonly Action<T> disposeCallback;

        public SmartResourceDestroyerService(IGenericScopingFactory genericScopingFactory,  IDisposableTracker disposableTracker, T resource, Action<T> disposeCallback)
        {
            this.resource = resource;
            this.disposeCallback = disposeCallback;
            this.genericScopingFactory = genericScopingFactory;
            this.disposableTracker = disposableTracker;
        }
        public T Value { get => resource; }
        public void DistroyWithSuite()
        {
            this.disposableTracker.Add(new Model.Types.DisposableResourceType()
            {
                resource = resource,
                disposeDelegate = (obj) => disposeCallback((T)obj)
            });
        }
        public void DistroyNow()
        {
            this.disposeCallback(resource);
        }
        public IScoper ScopedDistroy()
        {
            return genericScopingFactory.Create(() =>
            {
                this.DistroyNow();
            });
        }
    }
}
