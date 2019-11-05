using MC.Track.TestSuite.Model.Types.CoreServices.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types.CoreServices
{
    /// <summary>
    /// Create Notification Message
    /// </summary>
    public class CreateNotificationMessage
    {
        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        /// <value>
        /// The message body.
        /// </value>
        public CreateNotification MessageBody { get; set; }

        /// <summary>
        /// Gets or sets the message history identifier.
        /// </summary>
        /// <value>
        /// The message history identifier.
        /// </value>
        public int MessageHistoryId { get; set; }
    }
}
