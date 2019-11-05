using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
    public class RemittancePostDto
    {
        public long TradingRelationshipKey { get; set; }
        public long? RemitFromId { get; set; }
        public long? RemitToId { get; set; }
        public string Currency { get; set; }
    }
}
