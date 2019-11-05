using MC.Track.TestSuite.UI.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class UserType
    {
        public String Email { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String UserID { get; set; }
        public bool IsOnboarded { get; set; }
        public AlphaUserType AlphaUserType { get; set; }

        public UserType Buyer { get; set; }
        public UserType Supplier { get; set; }       
        public OrganizationType OnboardedWithOrganization { get; set; }
        public StagingOrganizationType OnboardedWithStagingOrganization { get; set; }
        public long? CardInstitutationId { get; set; }
        public long? AquiringBankId { get; set; }
    }
}
