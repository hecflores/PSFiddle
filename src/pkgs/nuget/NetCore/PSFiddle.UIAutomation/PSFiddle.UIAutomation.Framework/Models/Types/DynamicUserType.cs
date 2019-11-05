using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class DynamicUserType
    {
        public String email { get; set; }
        public String password { get; set; }
        public String userID { get; set; }
        public String firstName { get; set; }
        public String lastName { get; set; }
        public bool inUse { get; set; }
        public bool fromPool { get; set; }
    }
}
