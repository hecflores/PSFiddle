using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types.CoreServices
{
    [DataContract]
    public class IngestionErrorMessage
    {

        /// <summary>
        /// Gets or sets the offending line.
        /// </summary>
        /// <value>
        /// The offending line.
        /// </value>
        [DataMember]
        public string OffendingLine { get; set; }
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        [DataMember]
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets the UTC date time stamp.
        /// </summary>
        /// <value>
        /// The UTC date time stamp.
        /// </value>
        [DataMember]
        public DateTime UtcDateTimeStamp { get; set; }
        /// <summary>
        /// Gets or sets the message sender.
        /// </summary>
        /// <value>
        /// The message sender.
        /// </value>
        [DataMember]
        public string MessageSender { get; set; }
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
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [DataMember]
        public List<string> ValidationErrorMessages { get; set; }

        
    }
}
