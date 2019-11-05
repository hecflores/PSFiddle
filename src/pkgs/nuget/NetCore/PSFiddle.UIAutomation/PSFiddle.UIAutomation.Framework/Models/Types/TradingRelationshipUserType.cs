using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class TradingRelationshipUserType
    {
        public long? TradingRelationshipKey { get; set; }
        public UserType Buyer { get; set; }
        public UserType Supplier { get; set; }
    }
}
