using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Domain
{
    public class ApiResult
    {
        public string Data { get; set; }
        public HttpStatusCode ReturnValue { get; set; }
        public string Url { get; set; }
        public string HttpMethod { get; set; }
        public string ContentType { get; set; }
    }
}
