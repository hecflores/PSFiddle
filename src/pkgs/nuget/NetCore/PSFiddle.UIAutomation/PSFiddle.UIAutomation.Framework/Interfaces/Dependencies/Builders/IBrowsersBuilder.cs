using MC.Track.TestSuite.Interfaces.Dependencies.Builders.Shared;
using MC.Track.TestSuite.Interfaces.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Dependencies.Builders
{
    public interface IBrowsersBuilder : IBaseListBuilder<ITestableTrackBrowser, IBrowsersBuilder>, IDependencyBuilder
    {
        IBrowsersBuilder ChromeBrowser(Action<IBrowserBuilder> buildIt = null);
        IBrowsersBuilder IncognitoChromeBrowser(Action<IBrowserBuilder> buildIt = null);
    }
}
