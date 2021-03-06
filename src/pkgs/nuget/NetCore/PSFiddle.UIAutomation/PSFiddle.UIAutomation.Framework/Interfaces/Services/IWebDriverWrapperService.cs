﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MC.Track.TestSuite.Interfaces.Driver;
using OpenQA.Selenium;
using OpenQA.Selenium.Html5;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IWebDriverWrapperService
    {
        IApplicationCache ApplicationCache(IHasApplicationCache webDriver);
        ICapabilities Capabilities(IHasCapabilities webDriver);
        bool HoverLocation(IWebDriver webDriver, String Name, int x, int y, int timeOut, IWebElement scoped = null);
        bool ClickElement(IWebDriver webDriver, string Name, By by, int timeOut, IWebElement scoped = null);
        bool ClickLocation(IWebDriver webDriver, string Name, int x, int y, int timeOut, IWebElement scoped = null);
        void Close(IWebDriver webDriver);
        string CurrentWindowHandle(IWebDriver webDriver);
        void Dispose(IWebDriver webDriver);
        object ExecuteAsyncScript(IJavaScriptExecutor webDriver, string script, params object[] args);
        object ExecuteScript(IJavaScriptExecutor webDriver, string script, params object[] args);
        IWebElement FindElement(IWebDriver webDriver, By by);
        IWebElement FindElementByClassName(IFindsByClassName webDriver, string className);
        IWebElement FindElementByCssSelector(IFindsByCssSelector webDriver, string cssSelector);
        IWebElement FindElementById(IFindsById webDriver, string id);
        IWebElement FindElementByLinkText(IFindsByLinkText webDriver, string linkText);
        IWebElement FindElementByName(IFindsByName webDriver, string name);
        SessionId SessionId(IWebDriver webDriver);
        IWebElement FindElementByPartialLinkText(IFindsByPartialLinkText webDriver, string partialLinkText);
        IWebElement FindElementByTagName(IFindsByTagName webDriver, string tagName);
        bool HasApplicationCache(IWebDriver webDriver);
        bool IsActionExecutor(IWebDriver webDriver);
        IWebElement FindElementByXPath(IFindsByXPath webDriver, string xpath);
        ReadOnlyCollection<IWebElement> FindElements(IWebDriver webDriver, By by);
        IApplicationCache ApplicationCache(IWebDriver webDriver);
        ReadOnlyCollection<IWebElement> FindElementsByClassName(IFindsByClassName webDriver, string className);
        void SetFileDetector(IWebDriver webDriver, IFileDetector value);
        IFileDetector GetFileDetector(IWebDriver webDriver);
        ReadOnlyCollection<IWebElement> FindElementsByCssSelector(IFindsByCssSelector webDriver, string cssSelector);
        bool HasLocationContext(IWebDriver webDriver);
        ReadOnlyCollection<IWebElement> FindElementsById(IFindsById webDriver, string id);
        ILocationContext LocationContext(IWebDriver webDriver);
        ReadOnlyCollection<IWebElement> FindElementsByLinkText(IFindsByLinkText webDriver, string linkText);
        bool HasWebStorage(IWebDriver webDriver);
        ReadOnlyCollection<IWebElement> FindElementsByName(IFindsByName webDriver, string name);
        IWebStorage WebStorage(IWebDriver webDriver);
        ReadOnlyCollection<IWebElement> FindElementsByPartialLinkText(IFindsByPartialLinkText webDriver, string partialLinkText);
        ICapabilities Capabilities(IWebDriver webDriver);
        ReadOnlyCollection<IWebElement> FindElementsByTagName(IFindsByTagName webDriver, string tagName);
        IKeyboard Keyboard(IWebDriver webDriver);
        IMouse Mouse(IWebDriver webDriver);
        ReadOnlyCollection<IWebElement> FindElementsByXPath(IFindsByXPath webDriver, string xpath);
        bool Focus(IWebDriver webDriver, string Name, By by, int timeOut, IWebElement scoped = null);
        bool GetElements(IWebDriver webDriver, string Name, By by, int timeOut, out List<IWebElement> elements, IWebElement scoped = null, int minimumFound = 0, int maximumFound = int.MaxValue);
        object ExecuteAsyncScript(IWebDriver webDriver, string script, object[] args);
        IFileDetector GetFileDetector(IAllowsFileDetection webDriver);
        Screenshot GetScreenshot(ITakesScreenshot webDriver);
        string GetUrl(IWebDriver webDriver);
        object ExecuteScript(IWebDriver webDriver, string script, object[] args);
        bool GoTo(IWebDriver webDriver, string Url);
        bool HasApplicationCache(IHasApplicationCache webDriver);
        bool HasLocationContext(IHasLocationContext webDriver);
        bool HasWebStorage(IHasWebStorage webDriver);
        bool IsActionExecutor(IActionExecutor webDriver);
        IKeyboard Keyboard(IHasInputDevices webDriver);
        ILocationContext LocationContext(IHasLocationContext webDriver);
        IOptions Manage(IWebDriver webDriver);
        IMouse Mouse(IHasInputDevices webDriver);
        INavigation Navigate(IWebDriver webDriver);
        string PageSource(IWebDriver webDriver);
        IWebElement FindElementByClassName(IWebDriver webDriver, string className);
        void PerformActions(IActionExecutor webDriver, IList<ActionSequence> actionSequenceList);
        void Quit(IWebDriver webDriver);
        bool RunJS(IWebDriver webDriver, string Description, string Javascript, int timeOut, out object returnType, List<object> arguments = null);
        IWebElement FindElementByCssSelector(IWebDriver webDriver, string cssSelector);
        SessionId SessionId(IHasSessionId webDriver);
        void SetFileDetector(IAllowsFileDetection webDriver, IFileDetector detector);
        IWebElement FindElementById(IWebDriver webDriver, string id);
        void SetUrl(IWebDriver webDriver, string Url);
        ITargetLocator SwitchTo(IWebDriver webDriver);
        IWebElement FindElementByLinkText(IWebDriver webDriver, string linkText);
        string Title(IWebDriver webDriver);
        bool TypeIn(IWebDriver webDriver, string Name, By by, string Text, int timeOut, IWebElement scoped = null);
        bool WaitForIt<T>(IWebDriver webDriver, string Name, int sec, Func<IWebDriver, T> callback, out T result);
        IWebElement FindElementByName(IWebDriver webDriver, string name);
        bool WaitUntil(IWebDriver webDriver, string Name, int sec, Func<IWebDriver, bool> callback);
        IWebStorage WebStorage(IHasWebStorage webDriver);
        IWebElement FindElementByPartialLinkText(IWebDriver webDriver, string partialLinkText);
        ReadOnlyCollection<string> WindowHandles(IWebDriver webDriver);
        void ResetInputState(IActionExecutor webDriver);
        void ResetInputState(IWebDriver webDriver);
        IWebElement FindElementByTagName(IWebDriver webDriver, string tagName);
        void PerformActions(IWebDriver webDriver, IList<ActionSequence> actionSequenceList);
        Screenshot GetScreenshot(IWebDriver webDriver);
        IEnumerable<IWebElement> FindElementsByXPath(IWebDriver webDriver, string xpath);
        IWebElement FindElementByXPath(IWebDriver webDriver, string xpath);
        IEnumerable<IWebElement> FindElementsByTagName(IWebDriver webDriver, string tagName);
        IEnumerable<IWebElement> FindElementsByPartialLinkText(IWebDriver webDriver, string partialLinkText);
        IEnumerable<IWebElement> FindElementsByName(IWebDriver webDriver, string name);
        IEnumerable<IWebElement> FindElementsByLinkText(IWebDriver webDriver, string linkText);
        IEnumerable<IWebElement> FindElementsById(IWebDriver webDriver, string id);
        IEnumerable<IWebElement> FindElementsByClassName(IWebDriver webDriver, string className);
        IEnumerable<IWebElement> FindElementsByCssSelector(IWebDriver webDriver, string cssSelector);
    }
}