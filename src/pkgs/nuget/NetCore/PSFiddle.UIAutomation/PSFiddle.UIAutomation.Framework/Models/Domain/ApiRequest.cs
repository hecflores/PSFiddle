using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Domain
{
    public class ApiRequest
    {
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public string ContentType { get; set; }
        public string DataPath { get; set; }
    }
}
