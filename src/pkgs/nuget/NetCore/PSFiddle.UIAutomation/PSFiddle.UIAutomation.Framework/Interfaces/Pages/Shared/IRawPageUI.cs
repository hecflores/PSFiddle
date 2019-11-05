using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Pages.Shared
{
    public interface IRawPage : IPageBase
    {
        void VerifyElementText(String Name, String TestingText);
        void SwitchTabs(int tabIndex);
        T TransitionPage<T>() where T : class, IPageBase;
        T PartialPage<T>(IProtectedWebElement webElement) where T : class, IScopedPageUI;
        bool IsSelected(String Name);
        void VerifyElementIsHidden(String Name);
        void VerifyElementIsVisable(String Name);
        IScoper Scope(IProtectedWebElement webElement);
        T Get<T>(String Name);
        List<IProtectedWebElement> GetElements(String Name, int minimumFound = 0, int maximumFound = Int32.MaxValue);
        IProtectedWebElement GetElement(String Name);
        void Set(String Name, Object obj);
        T WaitFor<T>(Func<T> action, String messageIfFailed = null, int? timeout = null);
        ITestableTrackBrowser Browser { get; set; }
        void WaitFor(Action action, String messageIfFailed = null, int? timeout = null);
        void SwitchToBrowser(ITestableTrackBrowser browser);
        void GoToHomePage();
        void CloseCurrentBrowserAndSwitch(ITestableTrackBrowser browser);
        void ClickElement(String ElementName);
        void TypeIn(string Text, String ElementName);
        void VerifyElement(String Name);
        void VerifyElementNotExists(String Name);
        String Text(String Name);
        void VerifyAlert();
        void EnterTextAndVerifyAlert(String text);
        void EnterTextInActiveWindow(String text);
        T RunDefaultJSModuleFunction<T>(String Module, String Function, params Object[] arguments);
        T RunJSModuleFunction<T>(String JsRelativeFilePath, String Module, String Function, params Object[] arguments);
        void ClickLocation(String Name, int x, int y);
        
        bool isSetup();
    }
}
