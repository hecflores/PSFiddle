using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    /// <summary>
    /// 
    /// </summary>
    public class FinancialInstitutionOrganizationType
    {
        /// <summary>
        /// 
        /// </summary>
        public long? FinInstOrgKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? OrganizationKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FinancialInstitutionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        // Mistake in Type definition? Previously a long
        public string FinancialInstitutionID{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AlphaID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AccountNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MerchantID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CardScheme { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CoFtoken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? StatusKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ModifiedBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? ModifiedByKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? CreatedByKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? ComplianceDataDefault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? PremiumMonitoringDefault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? PaymentServicesDefault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? BillingDefault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? MerchantIDDefault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int MerchantCategoryCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
	    public String Description { get; set; }
        /// <summary>
        /// Masked number of card
        /// </summary>
        public string MaskedNumber { get; set; }
    }
}
