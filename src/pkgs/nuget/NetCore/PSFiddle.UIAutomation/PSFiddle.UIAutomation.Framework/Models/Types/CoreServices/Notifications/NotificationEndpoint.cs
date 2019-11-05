using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types.CoreServices.Notifications
{
    public class NotificationEndpoint
    {
        /// <summary>
        /// Gets or sets the type of the notification.
        /// </summary>
        /// <value>
        /// The type of the notification.
        /// </value>
        public string NotificationType { get; set; }
        /// <summary>
        /// Gets or sets the endpoint to.
        /// </summary>
        /// <value>
        /// The endpoint to.
        /// </value>
        public string[] EndpointTo { get; set; }
        /// <summary>
        /// Gets or sets the end point cc.
        /// </summary>
        /// <value>
        /// The end point cc.
        /// </value>
        public string[] EndPointCC { get; set; }
    }
}
