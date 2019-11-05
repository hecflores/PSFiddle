using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Driver
{
    public interface IWebElementWrapper : IWebElement, IProtectedWebElement, IToolTip, ILocatable
    {
        IWebDriverWrapper Driver();
        bool Focus();
        IWebElement CompletlyUnprotected();
    }
}
