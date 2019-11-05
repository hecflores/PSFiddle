using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class DisposableTracker : IDisposableTracker
    {
        private List<DisposableResourceType> disposables = new List<DisposableResourceType>();
        private bool isDisposed = false;
        private ILogger logger;
        public DisposableTracker(ILogger logger)
        {
            this.logger = logger;
        }
        public virtual void Add(DisposableResourceType obj)
        {
            if (isDisposed)
            {
                throw new Exception("Unable to add more disposable resources after disposing the disposable tracker");
            }
            this.disposables.Add(obj);
        }

        public virtual List<T> DestroyType<T>()
        {
            var items = disposables.Where(b => b.resource is T).ToList();
            Destroy(items);
            return items.Select(b => (T)b.resource).ToList();
        }
        private void Destroy(List<DisposableResourceType> disposables)
        {
            bool success = true;

            using (this.logger.Section($"Disposing of all {disposables.Count} items "))
            {
                for (var count = 0; count < disposables.Count; count++)
                {
                    var disposibleItem = disposables[count];
                    if (disposibleItem.isDisposed)
                    {
                        this.logger.LogWarning($"Disposing item [{count}]: Item is marked as disposed already... Skipping");
                        continue;
                    }
                    try
                    {
                        if (disposibleItem.resource == null)
                        {
                            this.logger.LogWarning($"Disposing item [{count}]: Item is null... Unexpected");
                            success = false;
                            continue;
                        }

                        this.logger.LogTrace($"Disposing item [{count}]: type {disposibleItem.resource.GetType().Name}");
                        disposibleItem.disposeDelegate(disposibleItem.resource);

                        disposibleItem.isDisposed = true;
                    }
                    catch (Exception e)
                    {
                        this.logger.LogError($"Failed to dispose item [{count}]", e);
                        success = false;
                    }
                }
                this.disposables.Clear();
            }

            if (!success)
            {
                throw new Exception("Not all resources we disposed of properly");
            }
        }
        public virtual void Dispose()
        {
            if (isDisposed) return;

            Destroy(disposables);

            isDisposed = true;
        }
    }
}
