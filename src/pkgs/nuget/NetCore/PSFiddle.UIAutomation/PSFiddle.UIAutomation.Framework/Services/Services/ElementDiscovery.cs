using MC.Track.TestSuite.Interfaces.Services;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.Shared;
using PSFiddle.UIAutomation.Framework.Shared;
using Jane;

namespace MC.Track.TestSuite.Services.Services
{
    public class ElementDiscovery : IElementDiscovery
    {
        public IResult<By> Discover(ref string Name)
        {
            var locationPath = LocatorHelper.GetLocationPathFromName(ref Name);
            return Result.Success<By>(By.XPath($".{locationPath}"));
        }
    }
}
