using MC.Track.TestSuite.Interfaces.Services;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Driver
{
    public interface ITrackBrowser: IDisposable
    {
        /// <summary>
        /// Gives you the raw selenium driver
        /// </summary>
        /// <returns></returns>
        IWebDriver SeleniumDriver();

        void SwitchTabs(int tabIndex);
        /// <summary>
        /// Initialize the browser. This should already be handled by factories.
        /// This will create the lower level libraries not start any real browser
        /// </summary>
        void Initialize();

        /// <summary>
        /// Sets the default timeout for any operatioins with the webdriver. In Seconds
        /// </summary>
        /// <param name="sec">Timeout in seconds</param>
        void DefaultTimeoutSec(int sec);

        /// <summary>
        /// Checks if the particular element is selected
        /// </summary>
        /// <param name="Name">Name of the element</param>
        /// <param name="by">Selector of the element</param>
        /// <returns>If Selected</returns>
        bool IsSelected(String Name, By by);

        /// <summary>
        /// Scope all element selection under a specific web element
        /// </summary>
        /// <param name="webElement">Scoped element</param>
        /// <returns>A Disposable scopper that when disposed will disable this scoping that was applied</returns>
        /// 
        IScoper ScopeToElement(IWebElementWrapper webElement);

        /// <summary>
        /// Get all the elements
        /// </summary>
        /// <param name="Name">Name of elements</param>
        /// <param name="by">Selector</param>
        /// <param name="WaitForSelectorTimeout">Wait of seconds</param>
        /// <param name="callback">Callback that will give the list of elements</param>
        /// <returns>If the operation was successfull</returns>
        bool GetElements(String Name, By by, Action<List<IWebElementWrapper>> callback, int? WaitForSelectorTimeout= null, int minimumFound = 0, int maximumFound = Int32.MaxValue);


        /// <summary>
        /// Get all the elements
        /// </summary>
        /// <param name="by">Selector</param>
        /// <param name="callback">Callback that will give the list of elements</param>
        /// <returns>If the operation was successfull</returns>
        bool GetElements(By by, Action<List<IWebElementWrapper>> callback);

        /// <summary>
        /// Clicks the element.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="by">The by.</param>
        /// <param name="WaitForSelectorTimeout">The wait for selector timeout.</param>
        /// <returns></returns>
        bool ClickElement(String Name, By by, int? WaitForSelectorTimeout = null);

        /// <summary>
        /// Click an element
        /// </summary>
        /// <param name="by">Selector</param>
        /// <returns>If the operation was successfull</returns>
        bool ClickElement(By by);

        /// <summary>
        /// Get the text from an element
        /// </summary>
        /// <param name="Name">Name of element</param>
        /// <param name="by">Selector</param>
        /// <param name="text">Output text</param>
        /// <returns>If the operation was successfull</returns>
        bool getText(String Name, By by, out String text);

        /// <summary>
        /// Type in text inside of an element
        /// </summary>
        /// <param name="by">Selector</param>
        /// <param name="Text">Text that types in</param>
        /// <param name="WaitForSelectorTimeout">Number of seconds to wait</param>
        /// <returns>If the operation was successfull</returns>
        bool TypeIn(String Name, By by, String Text, int WaitForSelectorTimeout);

        /// <summary>
        /// Type in text inside of an element
        /// </summary>
        /// <param name="by">Selector</param>
        /// <param name="Text">Text that types in</param>
        /// <returns>If the operation was successfull</returns>
        bool TypeIn(String Name, By by, String Text);      

        /// <summary>
        /// Wait until a specific 
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="sec">Number of seconds</param>
        /// <param name="callback">Callback that is used to determine the actual to wait for</param>
        /// <returns>If the operation was successfull</returns>
        bool WaitUntil(String Name, int sec, Func<IWebDriver, bool> callback);

        /// <summary>
        /// Wait until a specific 
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="callback">Callback that is used to determine the actual to wait for</param>
        /// <returns>If the operation was successfull</returns>
        bool WaitUntil(String Name, Func<IWebDriver, bool> callback);

        /// <summary>
        /// Wait to get a specific callback item.
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="sec">Number of seconds</param>
        /// <param name="callback">Callback that is used to determine the actual to wait for</param>
        /// <returns>If the operation was successfull</returns>
        bool WaitForIt<T>(String Name, int sec, Func<IWebDriver, T> callback, out T result);

        /// <summary>
        /// Wait to get a specific callback item.
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="callback">Callback that is used to wait for</param>
        /// <param name="result">Output of the wait for it function/param>
        /// <returns>If the operation was successfull</returns>
        bool WaitForIt<T>(String Name, Func<IWebDriver, T> callback, out T result);

        /// <summary>
        /// Wait to get a specific callback item.
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="callback">Callback that is used to wait for</param>
        /// <returns>If the operation was successfull</returns>
        bool WaitForIt<T>(String Name, Func<IWebDriver, T> callback);

        /// <summary>
        /// Accepts a popup alert
        /// </summary>
        /// <returns>If the operation was successfull</returns>
        bool AcceptAlert();

        /// <summary>
        /// Enters the input to a popup
        /// </summary>
        /// <param name="Text">Text to enter</param>
        /// <returns>If the operation was successfull</returns>
        bool EnterTextForAlert(String Text);

        /// <summary>
        /// Go to a specific url
        /// </summary>
        /// <param name="Url">URL to go to</param>
        /// <returns>If the operation was successfull</returns>
        bool GoTo(String Url);

        /// <summary>
        /// Take a screenshot of the browser
        /// </summary>
        /// <param name="Name">The name of the screenshot</param>
        /// <returns>The path of the image that was taken</returns>
        String TakeScreenshot(String Name);

        /// <summary>
        /// Get the current url of the browser
        /// </summary>
        /// <returns>The current url of the browser</returns>
        String GetHostUrl();

        /// <summary>
        /// Get the current route
        /// </summary>
        /// <returns></returns>
        String GetRoute();

        bool EnterTextInActiveWindow(string Text);

        bool RunDefaultJSModule(Action<dynamic> callback, String ModuleName, String FunctionName, List<Object> arguments);
        bool RunJSModule(Action<Object> callback, String JsFile, String ModuleName, String FunctionName, List<Object> arguments);

        bool ClickLocation(String Name, int x, int y);

        T RunDefaultJSModuleFunction<T>(String Module, String Function, params Object[] arguments);
        T RunJSModuleFunction<T>(String JsRelativeFilePath, String Module, String Function, params Object[] arguments);
    }
}
