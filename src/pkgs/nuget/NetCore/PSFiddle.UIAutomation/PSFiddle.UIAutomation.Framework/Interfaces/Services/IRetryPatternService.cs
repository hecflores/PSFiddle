using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    /// <summary>
    /// This service is meant to manage and control everywhere in the suite that need to implment retry...
    /// </summary>
    public interface IRetryPatternService
    {
        bool Retry(Action Action = null,
                   Action Cleanup = null,
                   int TimeoutInSeconds = -1,
                   int RetryGapTimeInSeconds = -1);
    }
}
