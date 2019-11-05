using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Config;
using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Model.Enums;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace MC.Track.TestSuite.Driver
{
    public class TestableChromeBrowser : TestableTrackBrowser
    {
        public TestableChromeBrowser(
                IConfiguration configuration,
                ILogger logger,
                IFileManager fileManager,
                IFileSaverService fileSaverService,
                ITrackTestRunner trackTestRunner,
                IGenericScopingFactory genericScopingFactory,
                IResolver resolver) : 
            base(
                configuration, 
                logger, 
                fileManager, 
                fileSaverService,
                trackTestRunner,
                genericScopingFactory,
                resolver)
        {
        }

        public override void Initialize()
        {
            ChromeOptions options = new ChromeOptions();
            this._webDriver = resolver.ApplyIntercepts<IWebDriverWrapper>(new WebDriverWrapper(resolver, new ChromeDriver(this._solutionPath)));
        }
    }
}
