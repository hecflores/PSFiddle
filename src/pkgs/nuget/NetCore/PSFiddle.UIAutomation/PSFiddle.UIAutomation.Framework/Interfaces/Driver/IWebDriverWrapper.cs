using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Driver
{
    public interface IWebDriverWrapper : IWebDriverAllInterfaces
    {
        bool RunJS(String Description, String Javascript, int timeOut, out Object returnType, List<Object> arguments = null);
        bool Focus(String Name, By by, int timeOut, IWebElement scoped = null);
        bool GetElements(String Name, By by, int timeOut, out List<IWebElement> elements, IWebElement scoped = null, int minimumFound = 0, int maximumFound = Int32.MaxValue);
        bool ClickLocation(String Name, int x, int y, int timeOut, IWebElement scoped = null);
        bool HoverLocation(String Name, int x, int y, int timeOut, IWebElement scoped = null);
        bool ClickElement(String Name, By by, int timeOut, IWebElement scoped = null);
        bool TypeIn(String Name, By by, String Text, int timeOut, IWebElement scoped = null);
        bool WaitUntil(String Name, int sec, Func<IWebDriver, bool> callback);
        bool WaitForIt<T>(String Name, int sec, Func<IWebDriver, T> callback, out T result);
        bool GoTo(String Url);
        IWebDriver RawDriver();
    }
}
