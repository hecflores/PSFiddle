using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    /// <summary>
    /// Return type of staging relationship table
    /// </summary>
    public class StagingRelationshipType
    {
        public long? StagingOrganizationKey { get; set; }
        public long? OrganizationKey { get; set; }
        public string ParameterValue { get; set; }
        public long? StagingOrganizationStatusValue { get; set; }
    }
}
