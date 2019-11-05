using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Driver
{
    public interface ITrackTestRunner
    {

        /// <summary>
        /// Retries the block.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="func">The function.</param>
        /// <param name="RetryTimes">The retry times.</param>
        /// <param name="DelayBetweenWaits">The delay between waits.</param>
        /// <returns></returns>
        bool RetryBlock(String Name, Func<bool> func, Int32? RetryTimes = null, TimeSpan? DelayBetweenWaits = null);
        /// <summary>
        /// Waits the until.
        /// </summary>
        /// <param name="MessageIfError">The message if error.</param>
        /// <param name="action">The action.</param>
        /// <param name="Timeout">The timeout.</param>
        /// <param name="DelayBetweenWaits">The delay between waits.</param>
        /// <returns></returns>
        bool WaitUntil(String MessageIfError, Func<bool> action, TimeSpan? Timeout = null, TimeSpan? DelayBetweenWaits = null);
    }
}
