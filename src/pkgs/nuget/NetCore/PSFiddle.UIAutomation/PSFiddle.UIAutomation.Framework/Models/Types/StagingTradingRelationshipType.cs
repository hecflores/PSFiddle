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
    public class StagingTradingRelationshipType
    {
        /// <summary>
        /// 
        /// </summary>
        public long? TradingRelationshipKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? PrimaryOrganizationKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PrimaryOrganizationType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? PrimaryOrganizationTypeKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? SecondaryOrganizationKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SecondaryOrganizationType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SecondaryOrganizationTypeKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RemitFromID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RemitToID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CurrencyFrom { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CurrencyTo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TradingRelationship { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TradingDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifiedDate { get; set; }
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
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? CreatedByKey { get; set; }

    }
}
