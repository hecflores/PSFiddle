using MC.Track.TestSuite.Interface.Services;
using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Factories
{
    [AthenaFactoryRegister(typeof(IServiceBusManager), arguments: new Type[] { typeof(String) })]
    public class ServiceBusManagerFactory : BaseMagicFactory<IServiceBusManager, String>
    {
        public ServiceBusManagerFactory(IDisposableTracker disposableTracker) : base(disposableTracker)
        {
        }

        protected override IServiceBusManager OnCreate(string arg)
        {
            return new ServiceBusManager(arg);
        }

        protected override void OnDestroy(IServiceBusManager obj)
        {
            obj.RevertChanges();
        }
    }
}
