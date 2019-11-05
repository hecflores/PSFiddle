using MC.Track.TestSuite.Interface.Services;
using MC.Track.TestSuite.Model.Helpers;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class ServiceBusManager : IServiceBusManager
    {
        private readonly string connectionString;
        private List<Action> revertActions = new List<Action>();

        public ServiceBusManager(String ConnectionString)
        {
            connectionString = ConnectionString;
        }
        public void CreateTopicIfNotExists(String Topic)
        {
            var namespaceItem = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceItem.TopicExists(Topic))
            {
                revertActions.Add(() =>
                {
                    XConsole.WriteLine($"Deleting Topic {Topic}");
                    namespaceItem.DeleteTopic(Topic);
                });
                namespaceItem.CreateTopic(Topic);
            }
        }
        public void CreateTopicSubscriptionIfNotExists(String Topic, String Subscription)
        {
            this.CreateTopicIfNotExists(Topic);

            var namespaceItem = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceItem.SubscriptionExists(Topic, Subscription))
            {
                revertActions.Add(() =>
                {
                    XConsole.WriteLine($"Deleting Subscription {Subscription} from Topic {Topic}");
                    namespaceItem.DeleteSubscription(Topic,Subscription);
                });
                namespaceItem.CreateSubscription(Topic, Subscription);
            }
        }
        private static string ConvertUTF16ToUTF8(String str)
        {
            byte[] utf = Encoding.Unicode.GetBytes(str);
            byte[] utf8 = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, utf);
            return Encoding.Default.GetString(utf8);
        }

        private static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        public void AddMessageObjectToTopic(String Topic, Object obj)
        {
            string message = JsonConvert.SerializeObject(obj);
            var bodyJson = ConvertUTF16ToUTF8(message);
            var Stream = GenerateStreamFromString(bodyJson);

            var client = TopicClient.CreateFromConnectionString(connectionString, Topic);
            client.Send(new BrokeredMessage(Stream));
        }
        public void AddMessageObjectToTopicAsString(String Topic, Object obj)
        {
            string message = JsonConvert.SerializeObject(obj);
            var client = TopicClient.CreateFromConnectionString(connectionString, Topic);
            client.Send(new BrokeredMessage(message));
        }
        public void RevertChanges()
        {
            revertActions.Reverse();
            revertActions.ForEach((item) =>
            {
                item();
            });
            revertActions.Clear();
        }
    }
}
