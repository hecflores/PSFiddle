using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class RelationshipInviteStatus
    {
        public long OrganizationKey { get; set; }
        public bool IsOnboarded { get; set; }
        public string InviteStatus { get; set; }
        public string OrganizationName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string RelationshipType { get; set; }
        public string Invitation { get; set; }
    }
}
