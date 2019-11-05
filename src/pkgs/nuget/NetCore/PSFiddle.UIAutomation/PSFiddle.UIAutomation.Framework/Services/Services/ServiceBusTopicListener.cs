using MC.Track.TestSuite.Interfaces.Services;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    /// <summary>
    /// ServiceBus Topic Listener
    /// </summary>
    public class ServiceBusTopicListener : IServiceBusTopicListener
    {

        /// <summary>
        /// The subscription client
        /// </summary>
        private SubscriptionClient _subscriptionClient;
        private readonly string connectionString;
        private readonly string topicName;
        private readonly string subscriptionName;

        /// <summary>
        /// The callback
        /// </summary>
        private Func<BrokeredMessage, Task> _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusTopicListener"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public ServiceBusTopicListener(String ConnectionString, String TopicName, String SubscriptionName, Func<BrokeredMessage, Task> callback)
        {
            connectionString = ConnectionString;
            topicName = TopicName;
            subscriptionName = SubscriptionName;
            _callback = callback;
        }
        /// <summary>
        /// This method causes the communication listener to close. Close is a terminal state and
        /// this method causes the transition to close ungracefully. Any outstanding operations
        /// (including close) should be canceled when this method is called.
        /// </summary>
        public void Abort()
        {
            this.Stop();
        }

        /// <summary>
        /// This method causes the communication listener to close. Close is a terminal state and
        /// this method allows the communication listener to transition to this state in a
        /// graceful manner.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task">Task</see> that represents outstanding operation.
        /// </returns>
        public Task CloseAsync(CancellationToken cancellationToken)
        {
            Stop();
            return Task.FromResult(true);
        }

        /// <summary>
        /// This method causes the communication listener to be opened. Once the Open
        /// completes, the communication listener becomes usable - accepts and sends messages.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task">Task</see> that represents outstanding operation. The result of the Task is
        /// the endpoint string.
        /// </returns>
        public Task OpenAsync()
        {
            _subscriptionClient = SubscriptionClient.CreateFromConnectionString(connectionString,topicName, subscriptionName);
            
            var onMessageOption = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(1)
            };

            _subscriptionClient.OnMessageAsync(_callback, onMessageOption);

            return Task.FromResult(0);

        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        private void Stop()
        {
            if (_subscriptionClient != null)
            {
                _subscriptionClient.Close();
                _subscriptionClient = null;
            }
        }
    }
}
