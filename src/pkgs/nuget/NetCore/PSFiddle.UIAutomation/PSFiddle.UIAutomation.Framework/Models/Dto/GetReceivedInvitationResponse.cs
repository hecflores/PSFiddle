using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
    /// <summary>
    /// Get received inviation response
    /// </summary>
    public class GetReceivedInvitationResponse
    {
        /// <summary>
        /// Organization key of organization that sent the invitation
        /// </summary>
        public long OrganizationKey { get; set; }
        /// <summary>
        /// Invite Status
        /// </summary>
        public string InviteStatus { get; set; }
        /// <summary>
        /// Organization Name
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// Address Line 1
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Address Line 2
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// City 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }
    }
}
