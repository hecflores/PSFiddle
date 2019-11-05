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
    //public class TestablePhantomJsBrowser : TestableTrackBrowser
    //{
    //    public TestablePhantomJsBrowser(
    //            IConfiguration configuration,
    //            ILogger logger,
    //            IFileManager fileManager,
    //            IFileSaverService fileSaverService,
    //            ITrackTestRunner trackTestRunner,
    //            IGenericScopingFactory genericScopingFactory,
    //            IResolver resolver) :
    //        base(
    //            configuration,
    //            logger,
    //            fileManager,
    //            fileSaverService,
    //            trackTestRunner,
    //            genericScopingFactory,
    //            resolver)
    //    {
    //    }

    //    public override void Initialize()
    //    {
    //        PhantomJSOptions options = new PhantomJSOptions();
    //        this._webDriver = new WebDriverWrapper(resolver, new PhantomJSDriver(this._solutionPath));
    //    }
    //}
}
