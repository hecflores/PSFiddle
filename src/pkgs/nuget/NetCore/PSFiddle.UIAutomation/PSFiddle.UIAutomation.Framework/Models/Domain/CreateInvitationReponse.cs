using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Domain
{
    public class CreateInvitationResponse
    {
        public long InvitedOrganization { get; set; }
        public string InvitedOrganizationName { get; set; }
        public RelationshipStatusType RelationshipStatus { get; set; }
    }

    public enum RelationshipStatusType
    {
        InvitedNow = 1,     // Invitation is sent successfully in this request
        AlreadyInvited = 2, // Invitation is already pending
        NotInvited = 3,     // Invitation was never sent and received error in this requst.
        Complete = 4,       // Organization is already in this relationship.
    }
}
