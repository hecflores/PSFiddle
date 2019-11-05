using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Factories
{
    public abstract class BaseMagicFactory<T> : IMagicFactory<T>
    {
        private IDisposableTracker disposableTracker;
        public BaseMagicFactory(IDisposableTracker disposableTracker)
        {
            this.disposableTracker = disposableTracker;
        }
        public T Create()
        {
            T created = OnCreate();

            var disposableType = new DisposableResourceType()
            {
                disposeDelegate = (resource) => this.OnDestroy((T)resource),
                resource = created
            };

            this.disposableTracker.Add(disposableType);

            return created;
        }
        protected abstract T OnCreate();
        protected abstract void OnDestroy(T obj);
    }
    public abstract class BaseMagicFactory<T,T1> : IMagicFactory<T, T1>
    {
        private IDisposableTracker disposableTracker;
        public BaseMagicFactory(IDisposableTracker disposableTracker)
        {
            this.disposableTracker = disposableTracker;
        }
        public T Create(T1 arg)
        {
            T created = OnCreate(arg);

            var disposableType = new DisposableResourceType()
            {
                disposeDelegate = (resource) => this.OnDestroy((T)resource),
                resource = created
            };

            this.disposableTracker.Add(disposableType);

            return created;
        }
        protected abstract T OnCreate(T1 arg);
        protected abstract void OnDestroy(T obj);
    }
    public abstract class BaseMagicFactory<T, T1, T2> : IMagicFactory<T, T1, T2>
    {
        private IDisposableTracker disposableTracker;
        public BaseMagicFactory(IDisposableTracker disposableTracker)
        {
            this.disposableTracker = disposableTracker;
        }
        public T Create(T1 arg1, T2 arg2)
        {
            T created = OnCreate(arg1,arg2);

            var disposableType = new DisposableResourceType()
            {
                disposeDelegate = (resource) => this.OnDestroy((T)resource),
                resource = created
            };

            this.disposableTracker.Add(disposableType);

            return created;
        }
        protected abstract T OnCreate(T1 arg1, T2 arg2);
        protected abstract void OnDestroy(T obj);
    }
    public abstract class BaseMagicFactory<T, T1, T2, T3> : IMagicFactory<T, T1, T2, T3>
    {
        private IDisposableTracker disposableTracker;
        public BaseMagicFactory(IDisposableTracker disposableTracker)
        {
            this.disposableTracker = disposableTracker;
        }
        public T Create(T1 arg1, T2 arg2, T3 arg3)
        {
            T created = OnCreate(arg1, arg2, arg3);

            var disposableType = new DisposableResourceType()
            {
                disposeDelegate = (resource) => this.OnDestroy((T)resource),
                resource = created
            };

            this.disposableTracker.Add(disposableType);

            return created;
        }
        protected abstract T OnCreate(T1 arg1, T2 arg2, T3 arg3);
        protected abstract void OnDestroy(T obj);
    }
    public abstract class BaseMagicFactory<T, T1, T2, T3, T4> : IMagicFactory<T, T1, T2, T3, T4>
    {
        private IDisposableTracker disposableTracker;
        public BaseMagicFactory(IDisposableTracker disposableTracker)
        {
            this.disposableTracker = disposableTracker;
        }
        public T Create(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            T created = OnCreate(arg1, arg2, arg3, arg4);

            var disposableType = new DisposableResourceType()
            {
                disposeDelegate = (resource) => this.OnDestroy((T)resource),
                resource = created
            };

            this.disposableTracker.Add(disposableType);

            return created;
        }
        protected abstract T OnCreate(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
        protected abstract void OnDestroy(T obj);
    }
}
