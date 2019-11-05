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
        private Dictionary<Type, List<Type>> _dependencies = new Dictionary<Type, List<Type>>();
        public DisposableTracker(ILogger logger)
        {
            this.logger = logger;
        }
        public List<Type> DestroyType(Type type)
        {
            var items = disposables.Where(b=> b.resource!=null)
                                   .Where(b => (type.IsAssignableFrom(b.resource.GetType()) || b.resource.GetType().IsAssignableFrom(type))).ToList();
            Destroy(items);
            return items.Select(b => b.resource?.GetType()).ToList();
        }
        public List<Type> GetDependenciesTypes(Type type)
        {
            List<Type> dependencies = new List<Type>();
            foreach (var dependency in _dependencies)
            {
                if (dependency.Key.IsAssignableFrom(type) || type.IsAssignableFrom(dependency.Key))
                    dependencies.AddRange(dependency.Value);
            }
            return dependencies;
        }
        
        public bool HasDependency(Type dependsOnSomething, Type theDependent)
        {
            var dependencies = GetDependenciesTypes(dependsOnSomething);
            return dependencies.Contains(theDependent);
        }
        public void AddDependency(Type dependsOnSomething, Type theDependent)
        {
            if (HasDependency(dependsOnSomething, theDependent))
                return;
            if (dependsOnSomething.IsAssignableFrom(theDependent) || theDependent.IsAssignableFrom(dependsOnSomething)) // Depends on its self
                return;

            if (!_dependencies.ContainsKey(dependsOnSomething))
                _dependencies.Add(dependsOnSomething, new List<Type>());
            _dependencies[dependsOnSomething].Add(theDependent);
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
        public virtual bool TryDestroyTypes(List<Type> types)
        {
            bool success = true;
            using (this.logger.Section($"Disposing of all {types.Count} types "))
            {
                for (var count = 0; count < types.Count; count++)
                {
                    var dependent = types[count];
                    this.logger.LogTrace($"Disposing type [{count}]: type {dependent}");
                    try
                    {
                        this.DestroyType(dependent);
                    }
                    catch (Exception e)
                    {
                        this.logger.LogError($"Failed to dispose type [{count}] ({dependent.FullName})", e);
                        success = false;
                    }
                }
            }

            return success;
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
                        this.logger.LogTrace($"Disposing item [{count}]: Item is marked as disposed already... Skipping");
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

                        // Destroy Depencies
                        using (this.logger.Section($"Disposing dependency types if any exists... "))
                        {
                            if (!this.TryDestroyTypes(this.GetDependenciesTypes(disposibleItem.resource.GetType())))
                            {
                                this.logger.LogError($"Failed to dispose all dependencies for item [{count}]");
                            }
                        }

                        // Destroy Item
                        disposibleItem.disposeDelegate(disposibleItem.resource);

                        disposibleItem.isDisposed = true;
                    }
                    catch (Exception e)
                    {
                        this.logger.LogError($"Failed to dispose item [{count}]", e);
                        success = false;
                    }
                }
                disposables.Clear();
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
