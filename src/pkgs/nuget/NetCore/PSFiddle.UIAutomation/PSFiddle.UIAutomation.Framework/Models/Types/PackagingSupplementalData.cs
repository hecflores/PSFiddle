using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class PackagingSupplementalData
    {
        public string UserId { get; set; }
        public long OrganizationKey { get; set; }
        public long AlphaUserKey { get; set; }
        public string CorrelationId { get; set; }
    }
}
