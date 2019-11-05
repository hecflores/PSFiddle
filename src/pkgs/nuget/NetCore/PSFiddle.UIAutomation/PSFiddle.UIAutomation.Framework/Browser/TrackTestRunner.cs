using MC.Track.TestSuite.Interfaces.Config;
using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Model.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Driver
{
    public class TrackTestRunner : ITrackTestRunner
    {
        private ILogger logger;
        private readonly IConfiguration config;

        public TrackTestRunner(
                ILogger logger, IConfiguration config)
        {
            this.logger = logger;
            this.config = config;
        }

        /// <summary>
        /// Retries the block.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="func">The function.</param>
        /// <param name="retryTimes">The retry times.</param>
        /// <param name="DelayBetweenWaits">The delay between waits.</param>
        /// <returns></returns>
        public bool RetryBlock(String Name, Func<bool> action, Int32? retryTimes = null, TimeSpan? DelayBetweenWaits = null)
        {
            if (!retryTimes.HasValue)
                retryTimes = config.DefaultRetryAttempts;
            if (!DelayBetweenWaits.HasValue)
                DelayBetweenWaits = TimeSpan.FromMilliseconds(100);

            for (var retryCount = 0; retryCount < retryTimes; retryCount++)
            {
                try
                {
                    using (logger.Section($"{Name} - Retrying [ Attempt(s) = {retryCount}, Delay Between Waits = {DelayBetweenWaits}], MaxAttempts = {retryTimes}"))
                    {
                        if (action())
                            return true;
                    }
                }
                catch (Exception e)
                {
                    this.logger.LogError($"Failed {Name}", e);
                }

                Thread.Sleep((int)DelayBetweenWaits.Value.TotalMilliseconds);
            }
            return false;
        }

        /// <summary>
        /// Waits the until.
        /// </summary>
        /// <param name="Title">The title.</param>
        /// <param name="action">The action.</param>
        /// <param name="Timeout">The timeout.</param>
        /// <param name="DelayBetweenWaits">The delay between waits.</param>
        /// <returns></returns>
        public bool WaitUntil(String Title, Func<bool> action, TimeSpan? Timeout = null, TimeSpan? DelayBetweenWaits = null)
        {
            if (!Timeout.HasValue)
                Timeout = TimeSpan.FromSeconds(config.DefaultTimeoutTime);
            if (!DelayBetweenWaits.HasValue)
                DelayBetweenWaits = TimeSpan.FromMilliseconds(100);

            string timeoutFormat = Timeout.Value.ToString();
            string delayBetweenWaits = DelayBetweenWaits.Value.ToString();

            using (this.logger.Section($"{Title} - Waiting [ Timeout = {timeoutFormat}, Delay Between Waits = {delayBetweenWaits}]"))
            {
                Exception lastException = null;
                DateTime rightNow = DateTime.Now;
                do
                {
                    try
                    {
                        if (action())
                            return true;
                    }
                    catch (Exception e)
                    {
                        lastException = e;
                    }
                    Thread.Sleep((int)DelayBetweenWaits.Value.TotalMilliseconds);
                } while (DateTime.Now.Subtract(rightNow).TotalSeconds <= Timeout.Value.TotalSeconds);

                if(lastException!=null)
                    logger.LogError(lastException);
                return false;
            }

        }
    }
}
