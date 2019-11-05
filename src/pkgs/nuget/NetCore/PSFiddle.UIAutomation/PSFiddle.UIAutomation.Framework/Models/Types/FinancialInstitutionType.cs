using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    /// <summary>
    /// All the bank account information for a particular entity
    /// </summary>
    public class FinancialInstitutionType
    {
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
        public string FinancialInstitutionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Address1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Address2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Zip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Country { get; set; }
    }
}
