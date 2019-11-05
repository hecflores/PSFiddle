using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Domain
{
    public interface IServiceBusEndpointCredentials
    {
        String ConnectionString { get; set; }
        String Topic { get; set; }
        String Subscription { get; set; }
    }
}
