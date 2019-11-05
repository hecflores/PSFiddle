using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Helpers;
using MC.Track.TestSuite.Services.Services;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Factories
{
    [AthenaFactoryRegister(typeof(IServiceBusTopicListener), arguments: new Type[] { typeof(String), typeof(String), typeof(String), typeof(Func<BrokeredMessage, Task>) })]
    public class ServiceBusTopicListenerFactory : BaseMagicFactory<IServiceBusTopicListener, String, String, String, Func<BrokeredMessage, Task>>
    {
        public ServiceBusTopicListenerFactory(IDisposableTracker disposableTracker) : base(disposableTracker)
        {
        }

        protected override IServiceBusTopicListener OnCreate(string arg1, string arg2, string arg3, Func<BrokeredMessage, Task> arg4)
        {
            return new ServiceBusTopicListener(arg1, arg2, arg3, arg4);
        }

        protected override void OnDestroy(IServiceBusTopicListener obj)
        {
            XConsole.WriteLine($"Stopping Service Bus Listener");
            obj.Abort();

        }
    }
}
