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
    public class SubscriptionRequestPutDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long SubscriptionRequestKey { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public string PSSubscriptionType { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public string PSSubscriptionStatus { get; set; }
    }
}
