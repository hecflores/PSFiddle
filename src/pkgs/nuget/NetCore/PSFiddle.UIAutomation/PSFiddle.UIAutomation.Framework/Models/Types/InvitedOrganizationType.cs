using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class InvitedOrganizationsType
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Organizationkey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsOnboarded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }
        public long? StatusKey { get; set; }
        public string CorrelationId { get; set; }
    }
}
