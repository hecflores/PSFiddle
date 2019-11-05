using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    [AthenaRegister(typeof(IEnvironmentVariables))]
    public class EnvironmentVariables : IEnvironmentVariables
    {
        /// <summary>
        /// Gets the <see cref="String"/> environment variable with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="String"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public String this[String name]
        {
            get
            {
                return GetEnvironmentVariable(name);
            }
        }

        /// <summary>
        /// Gets the environment variable.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <returns></returns>
        public String GetEnvironmentVariable(String Name)
        {
#if DEBUG
            var environmentConfig = EnvironmentVariableTarget.User;
#else
            var environmentConfig = EnvironmentVariableTarget.Process;
#endif
            return Environment.GetEnvironmentVariable(Name, environmentConfig);

        }
    }
}
