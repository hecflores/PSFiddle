using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto { 
    /// <summary>
    /// 
    /// </summary>
    public class CCReturnDto
    {
        /// <summary>
        /// ID of the credit card.
        /// </summary>
        public long? id { get; set; }
        /// <summary>
        /// Card scheme e.g. Mastercard, Visa, etc.
        /// </summary>
        public string cardScheme { get; set; }
        ///// <summary>
        ///// Last four digits of the credit card.
        ///// </summary>
        //public string lastFour { get; set; }
        ///// <summary>
        ///// The masked token represented with * out numbers
        ///// </summary>
        //public string maskedToken { get; set; }
        ///// <summary>
        ///// Currency of the credit card. Currently null in all cases but still here incase it needs to be added later.
        ///// </summary>
        public string currency { get; set; }
        /// <summary>
        /// Current status of the card.
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// Is card compliance default.
        /// </summary>
        public bool isComplianceDefault { get; set; } = false;
        /// <summary>
        /// Is card premium monitoring default.
        /// </summary>
        public bool isPremiumMonitoringDefault { get; set; } = false;
        /// <summary>
        /// Is card subscription default.
        /// </summary>
        public bool isSubscriptionDefault { get; set; } = false;
        /// <summary>
        /// Is card billing default.
        /// </summary>
        public bool isBillingDefault { get; set; } = false;
        /// <summary>
        /// Masked card number.
        /// </summary>
        public string maskedNumber { get; set; }
    }
}
