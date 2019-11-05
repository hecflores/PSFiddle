using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{ 
    public class InvitationsGetDto
    {
        /// <summary>
        /// If IsOnboarded is true then this will be Org Key else it is StagingOrgKey
        /// </summary>
        public long? OrganizationKey { get; set; }
        /// <summary>
        /// Is Onboarded
        /// </summary>
        public bool IsOnboarded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? InvitingOrganization { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InviteStatus { get; set; }
        /// <summary>
        /// Organization Name
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Organization Address 1
        /// </summary>
        public string Address1 { get; set; }
        /// <summary>
        /// Organization Address 2
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Organization Address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Organization City
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Organization State
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Organization Country
        /// </summary>
        public string Country { get; set; }
    }
}
