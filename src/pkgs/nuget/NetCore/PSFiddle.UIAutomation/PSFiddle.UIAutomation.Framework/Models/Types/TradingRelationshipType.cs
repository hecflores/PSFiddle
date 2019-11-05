using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class TradingRelationshipType
    {
        /// <summary>
        /// Key of the TR
        /// </summary>
        public long? TradingRelationshipKey { get; set; }
        /// <summary>
        /// primary alpha ID
        /// </summary>
        public string PrimaryAlphaID { get; set; }
        /// <summary>
        /// primary alpha ID
        /// </summary>
        public string SecondaryAlphaID { get; set; }
        /// <summary>
        /// First organization
        /// </summary>
        public long? PrimaryOrganizationKey { get; set; }
        /// <summary>
        /// Prim org type
        /// </summary>
        public string PrimaryOrganizationType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? PrimaryOrganizationTypeKey { get; set; }
        /// <summary>
        /// Whether the primary organization is onboarded or not.
        /// </summary>
        public bool? PrimaryOrganizationOnboarded { get; set; }
        /// <summary>
        /// Second org
        /// </summary>
        public long? SecondaryOrganizationKey { get; set; }
        /// <summary>
        /// secondary org type
        /// </summary>
        public string SecondaryOrganizationType { get; set; }
        /// <summary>
        /// Second org's role
        /// </summary>
        public long? SecondaryOrganizationTypeKey { get; set; }
        /// <summary>
        /// Whether the secondary organization is onboarded or not.
        /// </summary>
        public bool? SecondaryOrganizationOnboarded { get; set; }
        /// <summary>
        /// Remit from ID of the relationship
        /// </summary>
        public long? RemitFromID { get; set; }
        /// <summary>
        /// Remit to ID of the relationship
        /// </summary>
        public long? RemitToID { get; set; }
        /// <summary>
        /// Currency from
        /// </summary>
        public string CurrencyFrom { get; set; }
        /// <summary>
        /// Currency to
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
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public string ModifiedBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long ModifiedByKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long CreatedByKey { get; set; }
    }
}
