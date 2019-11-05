using MC.Track.TestSuite.Interface.Services;
using MC.Track.TestSuite.Interfaces.Feeds;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Model.EventArgs;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class ServiceBusServiceFeed : IServiceFeed<BrokeredMessage>
    {
        private readonly ILogger logger;
        private readonly string connectionString;
        private readonly string topicName;
        private readonly string subscriptionName;
        private readonly IServiceBusManager serviceBusManager;
        private readonly IServiceBusTopicListener serviceBusTopicListener;

        private static object _lock = new object(); // a lock to keep the logging and running of a incoming message together

        public ServiceBusServiceFeed(IResolver resolver, String ConnectionString, String TopicName, String SubscriptionName)
        {
            connectionString = ConnectionString;
            topicName = TopicName;
            subscriptionName = SubscriptionName;
            serviceBusManager = resolver.Resolve<IMagicFactory<IServiceBusManager, string>>().Create(ConnectionString);
            serviceBusTopicListener = resolver.Resolve<IMagicFactory<IServiceBusTopicListener, String, String, String, Func<BrokeredMessage, Task>>>().Create(ConnectionString, TopicName, SubscriptionName, this.OnMessageIncoming);
            logger = resolver.Resolve<ILogger>();
        }
        public EventHandler<IncomingServiceFeedEventArgs<BrokeredMessage>> IncomingFeed { get; set; }

        private Task OnMessageIncoming(BrokeredMessage brokeredMessage)
        {
            lock (_lock)
            {
                logger.LogTrace($"************ Message consuming from topic {topicName} subscription {subscriptionName} *******************");
                try
                {
                    IncomingFeed?.Invoke(this, new IncomingServiceFeedEventArgs<BrokeredMessage>()
                    {
                        Data = brokeredMessage,
                        IncomingDate = DateTime.Now
                    });
                }
                catch (Exception e)
                {
                    logger.LogError(e);
                }
                return Task.FromResult(0);
            }
        }

        public void Create()
        {
            serviceBusManager.CreateTopicSubscriptionIfNotExists(topicName, subscriptionName);
        }

        public void Dispose()
        {
            serviceBusTopicListener.Abort();
            serviceBusManager.RevertChanges();
        }

        public void StartFeed()
        {
            serviceBusTopicListener.OpenAsync();
        }
    }
}
