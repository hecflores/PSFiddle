using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
    /// <summary>
    /// This is the the model for the acquiring bank that gets returned in the API
    /// </summary>
    public class AcquiringBankReturnDto
    {
        /// <summary>
        /// FinInstOrg Key in the table
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The acquiring bank id.
        /// </summary>
        public string BankID { get; set; }
        /// <summary>
        /// Name of the bank.
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// MPGS merchant ID
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
        /// <summary>
        /// Status of the account.
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// True if the merchant id is default.
        /// </summary>
        public bool MerchantIDDefault { get; set; }
    }
}
