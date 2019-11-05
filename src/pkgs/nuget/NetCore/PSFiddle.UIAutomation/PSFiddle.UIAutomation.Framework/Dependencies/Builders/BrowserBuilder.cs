using MC.Track.TestSuite.Interfaces.Dependencies.Builders;
using MC.Track.TestSuite.Interfaces.Dependencies.Builders.Shared;
using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Toolkit.Dependencies.Builders.Shared;
using MC.Track.TestSuite.Toolkit.Dependencies.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Toolkit.Dependencies.Builders
{
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    public class BrowsersBuilder : BaseListBuilder<ITestableTrackBrowser, IBrowsersBuilder>, IBrowsersBuilder
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private readonly IStateManagment stateManagment;
        private IResolver resolver;
        public BrowsersBuilder(IResolver resolver) {
            this.stateManagment = resolver.Resolve<IStateManagment>();
            this.resolver = resolver;
        }

        public IBrowsersBuilder ChromeBrowser(Action<IBrowserBuilder> buildIt = null)
        {
            return this.AddBuildStep("Chrome Browser", (browsers) =>
            {
                browsers = browsers == null ? new List<ITestableTrackBrowser>() : browsers;
                var browser = FactoryHelper.Create<ITestableTrackBrowser>(this.resolver, "Chrome Browser");
                var builder = new BrowserBuilder(browser, this.stateManagment);
                buildIt?.Invoke(builder);
                browser = builder.Build();
                browsers.Add(browser);
                return browsers;
            });
        }
        public IBrowsersBuilder IncognitoChromeBrowser(Action<IBrowserBuilder> buildIt = null)
        {
            return this.AddBuildStep("Chrome Browser", (browsers) =>
            {
                browsers = browsers == null ? new List<ITestableTrackBrowser>() : browsers;
                var browser = FactoryHelper.Create<ITestableTrackBrowser>(this.resolver, "Incognito Chrome Browser");
                var builder = new BrowserBuilder(browser, this.stateManagment);
                buildIt?.Invoke(builder);
                browser = builder.Build();
                browsers.Add(browser);
                return browsers;
            });
           
        }
    }
    public class BrowserBuilder : BaseBuilder<ITestableTrackBrowser, IBrowserBuilder>, IBrowserBuilder
    {
        private readonly IStateManagment stateManagment;

        public BrowserBuilder(
            ITestableTrackBrowser initial,
            IStateManagment stateManagment
            ) :base(initial) {
            this.stateManagment = stateManagment;
        }

        public IBrowserBuilder StartBrowser()
        {
            return this.AddBuildStep("Starting Browser", (browser) =>
            {
                browser.Start();
                return browser;
            });
        }

        public IBrowserBuilder Focus()
        {
            return this.AddBuildStep("Focus on Browser", (browser) =>
            {
                this.stateManagment.Set("__BROWSER__", browser);
                return browser;
            });
        }
        
    }
}
