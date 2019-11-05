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
    public class SubscriptionStatusReturnDto
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
        public string PSSubscriptionRequestType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PSSubscriptionStatus { get; set; }

    }
}
