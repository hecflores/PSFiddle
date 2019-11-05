using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class MatchingContext
    {
        public List<ComplianceEntity> Compliance { get; set; }
        public UserType InitiatedUser { get; set; }
        public GenericFileType GenericFileType { get; set; }
        public FileMetadata FileMetaData { get; set; }
        public List<MatchType> Matches { get; set; }



    }
}
