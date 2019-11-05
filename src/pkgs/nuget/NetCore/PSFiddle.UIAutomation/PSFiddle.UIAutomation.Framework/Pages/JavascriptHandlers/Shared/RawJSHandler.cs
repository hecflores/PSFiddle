using MC.Track.TestSuite.Interfaces.Config;
using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Interfaces.Driver;

using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Services.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Toolkit.Pages.JavascriptHandlers.Shared
{
    public class RawJSHandler
    {
        private readonly IStateManagment stateManagment;
        private readonly IElementDiscovery elementDiscovery;
        private readonly IParameterParser parameterParser;
        private readonly IEmailService emailService;
        private readonly IFileManager fileManager;
        private readonly IFileSaverService fileSaverService;
        protected readonly IResolver resolver;
        protected readonly IConfiguration config;
        private readonly IDependencies Dependency;

        public RawJSHandler(IResolver resolver)
        {
            this.stateManagment = resolver.Resolve<IStateManagment>();
            this.elementDiscovery = resolver.Resolve<IElementDiscovery>();
            this.parameterParser = resolver.Resolve<IParameterParser>();
            this.emailService = resolver.Resolve<IEmailService>();
            this.fileManager = resolver.Resolve<IFileManager>();
            this.fileSaverService = resolver.Resolve<IFileSaverService>();
            this.config = resolver.Resolve<IConfiguration>();
            this.Dependency = resolver.Resolve<IDependencies>();
            this.resolver = resolver;
        }
        public ITestableTrackBrowser Browser
        {
            get
            {
                return Dependency.Browser().CurrentBrowser;
            }
            set
            {
                Dependency.Browser().CurrentBrowser = value;
            }
        }
    }
}
