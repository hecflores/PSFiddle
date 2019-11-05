using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types.CoreServices
{
    public class IngestionMessagePIFConversion : IngestionMessage
    {
        /// <summary>
        /// Gets or sets the invoice key.
        /// </summary>
        /// <value>
        /// The invoice key.
        /// </value>
        [DataMember]
        public long InvoiceKey { get; set; }
    }
}
