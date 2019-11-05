using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{

    public class ComplianceEntity
    {
        public long StagingOrganizationKey { get; set; }
        public StagingOrganizationType StagingOrganization { get; set; }
        public String SourceIdentifier { get; set; }
        public String SourceIdentifierID1 { get; set; }
        public String SourceIdentifierID2 { get; set; }
        public String SourceIdentifierKey { get; set; }
        public double MatchScore { get; set; }
        public String ComplianceType { get; set; }
        public String ComplianceUniqueId { get; set; }
        public bool IsPurchased { get; set; }
    }
}
