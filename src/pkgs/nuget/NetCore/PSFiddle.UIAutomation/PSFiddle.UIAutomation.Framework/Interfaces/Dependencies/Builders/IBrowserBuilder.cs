using MC.Track.TestSuite.Interfaces.Dependencies.Builders.Shared;
using MC.Track.TestSuite.Interfaces.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Dependencies.Builders
{
    public interface IBrowserBuilder : IBaseBuilder<ITestableTrackBrowser, IBrowserBuilder>, IDependencyBuilder
    {
        IBrowserBuilder StartBrowser();
        IBrowserBuilder Focus();
    }
}
