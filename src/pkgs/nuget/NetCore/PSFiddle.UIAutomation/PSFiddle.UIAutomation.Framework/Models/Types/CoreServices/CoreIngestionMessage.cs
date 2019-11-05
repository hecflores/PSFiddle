using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types.CoreServices
{
    /// <summary>
    /// Core Ingestion Message
    /// </summary>
    /// <seealso cref="MC.CoreAPI.Conversion.Domain.IngestionMessage" />
    [DataContract]
    public class CoreIngestionMessage : IngestionMessage
    {
        /// <summary>
        /// Gets or sets the type of the ingestion.
        /// </summary>
        /// <value>
        /// The type of the ingestion.
        /// </value>
        [DataMember]
        public string IngestionType { get; set; }
    }
}
