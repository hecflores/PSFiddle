using MC.Track.TestSuite.Driver;
using MC.Track.TestSuite.Interfaces;
using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Config;
using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Factories
{
    
    public class ChromeBrowserMagicFactory<E> : BaseMagicFactory<ITestableTrackBrowser> where E : TestableTrackBrowser
    {
        private IConfiguration config;
        private ILogger logger;
        private IDisposableTracker disposableTracker;
        private IFileManager fileManager;
        private IFileSaverService fileSaverService;
        private IGenericScopingFactory genericScopingFactory;
        private ITrackTestRunner trackTestRunner;
        private readonly IResolver resolver;

        public ChromeBrowserMagicFactory(IDisposableTracker disposableTracker,
                                   IConfiguration config,
                                   ILogger logger,
                                   IFileManager fileManager,
                                   IFileSaverService fileSaverService,
                                   IGenericScopingFactory genericScopingFactory,
                                   ITrackTestRunner trackTestRunner,
                                   IResolver resolver
                                   ) : base(disposableTracker)
        {
            this.config = config;
            this.logger = logger;
            this.fileManager = fileManager;
            this.disposableTracker = disposableTracker;
            this.fileSaverService = fileSaverService;
            this.genericScopingFactory = genericScopingFactory;
            this.trackTestRunner = trackTestRunner;
            this.resolver = resolver;
        }
        protected override ITestableTrackBrowser OnCreate()
        {
            ConstructorInfo classConstructor = typeof(E).GetConstructor(new Type[] 
            {   this.config.GetType(),
                this.logger.GetType(),
                this.fileManager.GetType(),
                this.fileSaverService.GetType(),
                this.trackTestRunner.GetType(),
                this.genericScopingFactory.GetType(),
                this.resolver.GetType()});

            ITestableTrackBrowser browser = (ITestableTrackBrowser)classConstructor.Invoke(new object[]
            {   this.config,
                this.logger,
                this.fileManager,
                this.fileSaverService,
                this.trackTestRunner,
                this.genericScopingFactory,
                this.resolver});

            browser = resolver.ApplyIntercepts<ITestableTrackBrowser>(browser);

            browser.Initialize();

            return browser;
        }

        protected override void OnDestroy(ITestableTrackBrowser obj)
        {
            resolver.Resolve<IBrowserDependencies>().CurrentBrowser = obj;
            obj.Dispose();
        }
    }

    [AthenaFactoryRegister(typeof(ITestableTrackBrowser), Name:BrowserTypes.ChromeBrowser)]
    public class DefaultTestableChromeBrowserFactory : ChromeBrowserMagicFactory<TestableChromeBrowser>
    {
        public DefaultTestableChromeBrowserFactory(IDisposableTracker disposableTracker, IConfiguration config, ILogger logger, IFileManager fileManager, IFileSaverService fileSaverService, IGenericScopingFactory genericScopingFactory, ITrackTestRunner trackTestRunner, IResolver resolver) : base(disposableTracker, config, logger, fileManager, fileSaverService, genericScopingFactory, trackTestRunner, resolver)
        {
        }
    }

    [AthenaFactoryRegister(typeof(ITestableTrackBrowser), Name: BrowserTypes.IncognitoChromeBrowser)]
    public class DefaultIncognitoTestableChromeBrowserFactory : ChromeBrowserMagicFactory<TestableChromeIncognitoBrowser>
    {
        public DefaultIncognitoTestableChromeBrowserFactory(IDisposableTracker disposableTracker, IConfiguration config, ILogger logger, IFileManager fileManager, IFileSaverService fileSaverService, IGenericScopingFactory genericScopingFactory, ITrackTestRunner trackTestRunner, IResolver resolver) : base(disposableTracker, config, logger, fileManager, fileSaverService, genericScopingFactory, trackTestRunner, resolver)
        {
        }
    }

    

}
