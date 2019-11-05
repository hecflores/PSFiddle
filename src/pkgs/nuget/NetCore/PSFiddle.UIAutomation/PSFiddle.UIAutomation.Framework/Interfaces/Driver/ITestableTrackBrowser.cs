using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Driver
{
    public interface ITestableTrackBrowser : ITrackBrowser, ITesterRunningHelper
    {
        bool Start();
        bool VerifyLabel(String Name, int sec, By by);
        bool VerifyLabel(String Name, By by);
        bool VerifyLabel(String Name, By by, String text = null);
        bool WaitFor(String Name, By by);
        bool VerifyAlertMessage(String Message);
        bool GoToHost();
        bool GoToRoute(String Route);
    }
}
