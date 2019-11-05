using MC.Track.TestSuite.Model.Types;
using MC.Track.TestSuite.UI.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Dependencies
{
    public interface IOrganizationDependencies : IDependency
    {
        OrganizationType GetOrgForUser(UserType user);
        StagingOrganizationType NewOrganization();
        StagingOrganizationType CreateRandomStagingTestOrganization();
        OrganizationType CreateRandomTestOrganization();
    }
}
