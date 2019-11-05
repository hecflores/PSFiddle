using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public static class MerchantDefault
    {
        //default values for MerchantCategoryCode and Description are 5999 and MISCALLANEOUS
        /// <summary>
        /// 
        /// </summary>
        public const int MerchantCategoryCode = 5999;
        /// <summary>
        /// 
        /// </summary>
        public const string Description = "MISCELLANEOUS";
    }

    /// <summary>
    /// 
    /// </summary>
    public class FinancialInstitutionOrganizationDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long? OrganizationKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? FinancialInstitutionKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CardOnFile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CoFCVC2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CoFtoken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AccountNumber { get; set; }
        /// <summary>
        /// Merchant Id
        /// </summary>
        public string MerchantID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PaymentPreferences { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Currency { get; set; }
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
        public string FinancialInstitutionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? MerchantIDDefault { get; set; }
        private int? _MerchantCategoryCode;
        /// <summary>
        /// 
        /// </summary>
        public int? MerchantCategoryCode
        {
            get
            {
                if (!_MerchantCategoryCode.HasValue)
                    _MerchantCategoryCode = MerchantDefault.MerchantCategoryCode;
                return _MerchantCategoryCode;
            }
            set
            {
                if (!_MerchantCategoryCode.HasValue)
                    _MerchantCategoryCode = MerchantDefault.MerchantCategoryCode;
                _MerchantCategoryCode = value;
            }
        }
        private string _Description;
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get
            {
                if (String.IsNullOrEmpty(_Description))
                    _Description = MerchantDefault.Description;
                return _Description;
            }
            set
            {
                if (String.IsNullOrEmpty(_Description))
                    _Description = MerchantDefault.Description;
                else
                    _Description = value;

            }
        }

        /// <summary>
        /// Card scheme
        /// </summary>
        public string CardScheme { get; set; }

        /// <summary>
        /// Masked number of card
        /// </summary>
        public string MaskedNumber { get; set; }
    }
}

