using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types.CoreServices
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class MPGSInputMessage
    {
        /// <summary>
        /// Gets or sets the payment initiation TXNS key.
        /// </summary>
        /// <value>
        /// The payment initiation TXNS key.
        /// </value>
        [DataMember]
        public long PaymentInitiationTxnsKey { get; set; }

        /// <summary>
        /// Gets or sets the alpha user key.
        /// </summary>
        /// <value>
        /// The alpha user key.
        /// </value>
        [DataMember]
        public long AlphaUserKey { get; set; }

        /// <summary>
        /// Gets or sets the approval time stamp.
        /// </summary>
        /// <value>
        /// The approval time stamp.
        /// </value>
        [DataMember]
        public string ApprovalTimeStamp { get; set; }
    }
}
