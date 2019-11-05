using MC.Track.TestSuite.Interfaces.Attributes;
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
        private readonly IDisposableTracker disposableTracker;
        private readonly IResolver resolver;
        private readonly T resource;
        private readonly Action<T> disposeCallback;

        public SmartResourceDestroyerService(IResolver resolver, T resource, Action<T> disposeCallback)
        {
            this.resolver = resolver;
            this.resource = resource;
            this.disposeCallback = disposeCallback;
            this.disposableTracker = resolver.Resolve<IDisposableTracker>();
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
            return resolver.Resolve<IGenericScopingFactory>().Create(() =>
            {
                this.DistroyNow();
            });
        }
    }
}
