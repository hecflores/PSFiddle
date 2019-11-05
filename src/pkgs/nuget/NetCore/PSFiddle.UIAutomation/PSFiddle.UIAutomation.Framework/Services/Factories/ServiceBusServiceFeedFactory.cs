using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Feeds;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Services.Services;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Factories
{
    [AthenaFactoryRegister(typeof(IServiceFeed<BrokeredMessage>), arguments: new Type[] { typeof(String), typeof(String), typeof(String) })]
    public class ServiceBusServiceFeedFactory : BaseMagicFactory<IServiceFeed<BrokeredMessage>, String, String, String>
    {
        private readonly IResolver resolver;

        public ServiceBusServiceFeedFactory(IResolver resolver, IDisposableTracker disposableTracker) : base(disposableTracker)
        {
            this.resolver = resolver;
        }

        protected override IServiceFeed<BrokeredMessage> OnCreate(string arg1, string arg2, string arg3)
        {
            var serviceBusFeed =  new ServiceBusServiceFeed(resolver, arg1, arg2, arg3);
            serviceBusFeed.Create();
            return serviceBusFeed;
        }

        protected override void OnDestroy(IServiceFeed<BrokeredMessage> obj)
        {
            obj.Dispose();
        }
    }
}
