using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Dependencies.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Dependencies
{
    public interface IBrowserDependencies : IDependency
    {
        void SwitchToBrowser(ITestableTrackBrowser browser);
        void CloseCurrentBrowserAndSwitch(ITestableTrackBrowser browser);
        ITestableTrackBrowser CurrentBrowser { get; set; }
        ITestableTrackBrowser ChromeBrowser();
        ITestableTrackBrowser IncognitoChromeBrowser();
        IBrowsersBuilder Factory();
    }
}
