using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Resources;
using MC.Track.TestSuite.Model.Domain;
using MC.Track.TestSuite.Model.Helpers;
using MC.Track.TestSuite.Services.Resources;
using System.Configuration;

namespace MC.Track.TestSuite.Services.Factories
{
    [AthenaFactoryRegister(typeof(IAPIEndpoint), arguments: new Type[] { typeof(String), typeof(String) })]
    public class ApiEndpointFactory : BaseMagicFactory<IAPIEndpoint, String, String>
    {
        private readonly IResolver resolver;

        public ApiEndpointFactory(IDisposableTracker disposableTracker, IResolver resolver) : base(disposableTracker)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        protected override IAPIEndpoint OnCreate(string arg1, string arg2)
        {
            var hostUrl = String.Format(ConfigurationManager.AppSettings["apiStructure"], arg1);// Environment.GetEnvironmentVariable(arg1);
            var config = resolver.Resolve<AzureActiveDirectoryConfig>(arg2);
            Console.WriteLine("ClientId: " + config.ClientId);
            Console.WriteLine("AppKey: " + config.AppKey);

            return new APIEndpoint(hostUrl, config, resolver);
        }

        protected override void OnDestroy(IAPIEndpoint obj)
        {
            // Currently dont need to dispose a API endpoint but in the future we will need to...
            //    1. When we need to including mocking we will need to "undo" the mock....
            //    2. When we need to gather telemetry for APIs during tests we will need to grab those artifacts here...
            XConsole.WriteLine("API Endpoints have no disposable action... Yet....");
        }
    }
}
