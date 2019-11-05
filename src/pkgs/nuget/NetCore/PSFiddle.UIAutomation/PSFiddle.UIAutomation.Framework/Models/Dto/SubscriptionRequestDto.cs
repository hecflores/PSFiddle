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
    public class SubscriptionRequestDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long? PSSubscriptionKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? OrganizationKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? AlphaUserKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PSSubscriptionRequestType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? PSSubscriptionRequestTypeKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? TimeOfRequest { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PSSubscriptionStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? PSSubscriptionStatusKey { get; set; }
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
        public long? CreatedBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? CreatedByKey { get; set; }
    }
}
