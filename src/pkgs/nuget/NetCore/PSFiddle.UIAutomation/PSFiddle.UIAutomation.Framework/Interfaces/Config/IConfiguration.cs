using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Config
{
    public interface IConfiguration
    {
        String SolutionPath { get; }
        String HostUrl { get; }
        int DefaultTimeoutTime { get; }
        int DefaultRetryAttempts { get; }
        String DatabaseConnectionString { get;  }
        String MPGSMerchantId { get; }
        String CoreServicesStorageAccountConnectionString { get; }
        String TestServicesStorageAccountConnectionString { get; }
        String TestUsersXML { get; }
        string CoreServicesServiceBusConnectionString { get; }
    }
}
