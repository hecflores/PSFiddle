using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types.CoreServices
{
    [DataContract]
    public class IngestionMessage
    {
        /// <summary>
        /// Gets or sets the correllation.
        /// </summary>
        /// <value>
        /// The correllation.
        /// </value>
        [DataMember]
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the BLOB location.
        /// </summary>
        /// <value>
        /// The BLOB location.
        /// </value>
        [DataMember]
        public string BlobLocation { get; set; }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [DataMember]
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        [DataMember]
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Gets or sets the message sender.
        /// </summary>
        /// <value>
        /// The message sender.
        /// </value>
        [DataMember]
        public string MessageSender { get; set; }
        /// <summary>
        /// Gets or sets the message identifier.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        [DataMember]
        public string MessageId { get; set; }
        /// <summary>
        /// Gets or sets the type of the media.
        /// </summary>
        /// <value>
        /// The type of the media.
        /// </value>
        [DataMember]
        public string MediaType { get; set; }
        /// <summary>
        /// Gets or sets the organization key.
        /// </summary>
        /// <value>
        /// The organization key.
        /// </value>
        [DataMember]
        public long? OrganizationKey { get; set; }
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        [DataMember]
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        [DataMember]
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the NTP incoming folder identifier.
        /// </summary>
        /// <value>
        /// The NTP incoming folder identifier.
        /// </value>
        [DataMember]
        public MetaData MetaData { get; set; }

        /// <summary>
        /// Gets or sets the message recepient.
        /// </summary>
        /// <value>
        /// The message recepient.
        /// </value>
        [DataMember]
        public string MessageRecepient { get; set; }

        /// <summary>
        /// Gets or sets the message recepient.
        /// </summary>
        /// <value>
        /// The message recepient.
        /// </value>
        [DataMember]
        public Organization InitiatingParty { get; set; }

    }

    [DataContract]
    public class MetaData
    {
        /// <summary>
        /// Gets or sets the incoming folder.
        /// </summary>
        /// <value>
        /// The incoming folder.
        /// </value>
        [DataMember]
        public int IncomingFolder { get; set; }

        /// <summary>
        /// Gets or sets the type of the document.
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
        [DataMember]
        public string DocumentType { get; set; }
    }
}
