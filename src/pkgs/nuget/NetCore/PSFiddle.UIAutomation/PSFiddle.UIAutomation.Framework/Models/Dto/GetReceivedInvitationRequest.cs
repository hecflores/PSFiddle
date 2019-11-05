using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
    /// <summary>
    /// Get received invitations request
    /// </summary>
    public class GetReceivedInvitationRequest
    {
        /// <summary>
        /// Organization key of organization for which received invitations need to be retrieved.
        /// </summary>
        public long OrgKey { get; set; }

        /// <summary>
        /// Fetch invitations as buyer or supplier.
        /// </summary>
        public bool? FetchBuyerInviation { get; set; }
    }
}
