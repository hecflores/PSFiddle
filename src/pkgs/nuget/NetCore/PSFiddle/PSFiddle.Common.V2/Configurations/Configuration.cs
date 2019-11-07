using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NETFRAMEWORK
using System.Web.Configuration;
#endif

namespace PSFiddle.Common.Configurations
{
    public class Configuration : IConfiguration
    {
#if NETCORE
        private Microsoft.Extensions.Configuration.IConfiguration _configuration;
        public Configuration(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            this._configuration = configuration;
        }
#endif
        public string this[string key] { get => Get(key);}

        public string Get(string Key)
        {
#if NETCORE
            return this._configuration[Key];
#endif


#if NETFRAMEWORK
            if (AppDomain.CurrentDomain.GetAssemblies().Count(b => b.FullName == typeof(WebConfigurationManager).Assembly.FullName) > 0)
                return WebConfigurationManager.AppSettings[Key];
            else
                return ConfigurationManager.AppSettings[Key];
#endif
        }
    }
}
