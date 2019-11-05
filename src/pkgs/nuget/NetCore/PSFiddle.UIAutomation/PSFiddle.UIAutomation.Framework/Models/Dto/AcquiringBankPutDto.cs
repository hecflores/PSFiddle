using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
    /// <summary>
    /// Model to get PUT to the API.
    /// </summary>
    public class AcquiringBankPutDto
    {
        /// <summary>
        /// FinInstOrg ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// Acquiring bank ID
        /// </summary>
        public string BankID { get; set; }
        /// <summary>
        /// name of the Bank to be displayed
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// MPGS Merchant ID
        /// </summary>
        public string MerchantID { get; set; }
        /// <summary>
        /// Bank's merchant ID
        /// </summary>
        public string BankMerchantID { get; set; }
        /// <summary>
        /// Currency for this acquirer account.
        /// </summary>
        public string Currency { get; set; }
    }
}
