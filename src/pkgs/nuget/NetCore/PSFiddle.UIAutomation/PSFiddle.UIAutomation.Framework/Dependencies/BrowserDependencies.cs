using MC.Track.TestSuite.Interfaces.Config;
using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Interfaces.Dependencies.Builders;
using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Toolkit.Dependencies.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Toolkit.Dependencies
{
    public class BrowserDependencies : IBrowserDependencies
    {
        private readonly IBrowsersBuilder Builder;
        private readonly IStateManagment stateManager;
        private readonly IConfiguration config;
        public BrowserDependencies(IResolver resolver)
        {
            this.stateManager = resolver.Resolve<IStateManagment>();
            this.Builder = resolver.Resolve<IBrowsersBuilder>();
            this.config = resolver.Resolve<IConfiguration>();
        }
        public void SwitchToBrowser(ITestableTrackBrowser browser)
        {
            CurrentBrowser = browser;
        }
        public void CloseCurrentBrowserAndSwitch(ITestableTrackBrowser browser)
        {
            if (this.stateManager.Has("__BROWSER__"))
            {
                CurrentBrowser.Dispose();
            }
            CurrentBrowser = browser;
        }
        public ITestableTrackBrowser CurrentBrowser
        {
            get
            {
                return this.stateManager.Get<ITestableTrackBrowser>("__BROWSER__");
            }
            set
            {
                this.stateManager.Set("__BROWSER__", value);
            }
        }


        
        public ITestableTrackBrowser ChromeBrowser()
        {
            return this.Builder.ChromeBrowser().BuildSingle();
        }
        public ITestableTrackBrowser IncognitoChromeBrowser()
        {
            return this.Builder.IncognitoChromeBrowser().BuildSingle();
        }
        public IBrowsersBuilder Factory()
        {
            return this.Builder;
        }
    }
}
