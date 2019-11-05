using System;

namespace MC.Track.TestSuite.Interface.Services
{
    public interface IServiceBusManager
    {
        void AddMessageObjectToTopic(String Topic, Object obj);
        void AddMessageObjectToTopicAsString(String Topic, Object obj);
        void CreateTopicIfNotExists(string Topic);
        void CreateTopicSubscriptionIfNotExists(string Topic, string Subscription);
        
        void RevertChanges();
    }
}