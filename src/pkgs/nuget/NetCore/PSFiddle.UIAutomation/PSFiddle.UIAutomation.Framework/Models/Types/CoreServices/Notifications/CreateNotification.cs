using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types.CoreServices.Notifications
{
    public class CreateNotification
    {
        /// <summary>
        /// Gets or sets the notification endpoint.
        /// </summary>
        /// <value>
        /// The notification endpoint.
        /// </value>
        public NotificationEndpoint[] NotificationEndpoint { get; set; }
        /// <summary>
        /// Gets or sets the notification date timestamp.
        /// </summary>
        /// <value>
        /// The notification date timestamp.
        /// </value>
        public string NotificationDateTimestamp { get; set; }
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the from user email name
        /// </summary>
        /// <value>
        /// The From User Email Name
        /// </value>
        public string FromUserEmailName { get; set; }

    }
}
