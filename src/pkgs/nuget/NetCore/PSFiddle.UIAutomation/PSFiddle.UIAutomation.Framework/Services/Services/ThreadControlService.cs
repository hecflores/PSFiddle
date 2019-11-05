using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    [AthenaRegister(typeof(IThreadControlService), Model.Enums.AthenaRegistrationType.Singleton)]
    public class ThreadControlService : IThreadControlService
    {
        private readonly IGenericScopingFactory genericScopingFactory;
        private object _mutex = new object();
        
        public ThreadControlService(IGenericScopingFactory genericScopingFactory)
        {
            this.genericScopingFactory = genericScopingFactory;
        }
        public IScoper Mutex(string Name)
        {
            Wait(Name);
            return genericScopingFactory.Create(() =>
            {
                Release(Name);
            });
        }

        public void Release(string semName)
        {
            lock (_mutex)
            {
                getSem(semName).Release();
            }
        }

        public void Wait(string semName)
        {
            lock (_mutex)
            {
                getSem(semName).WaitOne();
            }
        }
        public void Wait(string semName, int numberOfTimes)
        {
            for(int i = 0; i < numberOfTimes; i++)
            {
                Wait(semName);
            }
        }
        private Semaphore getSem(String name)
        {
            Semaphore result;
            if(Semaphore.TryOpenExisting(name, out result))
            {
                return result;
            }
            return new Semaphore(1, Int32.MaxValue, name);
        }
    }
}
