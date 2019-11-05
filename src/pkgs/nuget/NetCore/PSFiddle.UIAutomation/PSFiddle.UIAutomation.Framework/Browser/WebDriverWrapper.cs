using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Html5;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MC.Track.TestSuite.Driver
{
    public class WebDriverWrapper : IWebDriverWrapper
    {
        private readonly IWebDriverWrapperService webDriverWrapperService;
        private readonly IResolver resolver;
        private readonly IWebDriver webDriver;

         public WebDriverWrapper(IResolver resolver, IWebDriver webDriver):base()
        {
            this.resolver = resolver;
            this.webDriver = webDriver;
            this.webDriverWrapperService = resolver.Resolve<IWebDriverWrapperService>();
        }

        public string Url { get => webDriverWrapperService.GetUrl(webDriver); set => webDriverWrapperService.SetUrl(webDriver, value); }

        public string Title => webDriverWrapperService.Title(webDriver);

        public string PageSource => webDriverWrapperService.PageSource(webDriver);

        public string CurrentWindowHandle => webDriverWrapperService.CurrentWindowHandle(webDriver);

        public ReadOnlyCollection<string> WindowHandles => webDriverWrapperService.WindowHandles(webDriver);

        public SessionId SessionId => webDriverWrapperService.SessionId(webDriver);

        public bool IsActionExecutor => webDriverWrapperService.IsActionExecutor(webDriver);

        public bool HasApplicationCache => webDriverWrapperService.HasApplicationCache(webDriver);

        public IApplicationCache ApplicationCache => webDriverWrapperService.ApplicationCache(webDriver);

        public IFileDetector FileDetector { get => webDriverWrapperService.GetFileDetector(webDriver); set => webDriverWrapperService.SetFileDetector(webDriver, value); }

        public bool HasLocationContext => webDriverWrapperService.HasLocationContext(webDriver);

        public ILocationContext LocationContext => webDriverWrapperService.LocationContext(webDriver);

        public bool HasWebStorage => webDriverWrapperService.HasWebStorage(webDriver);

        public IWebStorage WebStorage => webDriverWrapperService.WebStorage(webDriver);

        public ICapabilities Capabilities => webDriverWrapperService.Capabilities(webDriver);

        public IKeyboard Keyboard => webDriverWrapperService.Keyboard(webDriver);

        public IMouse Mouse => webDriverWrapperService.Mouse(webDriver);

        public void Close()
        {
            webDriverWrapperService.Close(webDriver);
        }

        public void Dispose()
        {
            webDriverWrapperService.Dispose(webDriver);
        }

        public object ExecuteAsyncScript(string script, params object[] args)
        {
            return webDriverWrapperService.ExecuteAsyncScript(webDriver, script, args);
        }

        public object ExecuteScript(string script, params object[] args)
        {
            return webDriverWrapperService.ExecuteScript(webDriver, script, args);
        }
        private IWebElementWrapper ConvertToWrapper(IWebElement element, String name)
        {
            if (!(element is IWebElementWrapper))
                return resolver.ApplyIntercepts<IWebElementWrapper>(new WebElementWrapper(resolver,this, name, (element)));

            return (IWebElementWrapper)element;
        }
        public IWebElement FindElement(By by)
        {
            return ConvertToWrapper(webDriverWrapperService.FindElement(webDriver, by), by.ToString());
        }

        public IWebElement FindElementByClassName(string className)
        {
            return ConvertToWrapper(webDriverWrapperService.FindElementByClassName(webDriver, className),className);
        }

        public IWebElement FindElementByCssSelector(string cssSelector)
        {
            return ConvertToWrapper(webDriverWrapperService.FindElementByCssSelector(webDriver, cssSelector), cssSelector);
        }

        public IWebElement FindElementById(string id)
        {
            return ConvertToWrapper(webDriverWrapperService.FindElementById(webDriver, id), id);
        }

        public IWebElement FindElementByLinkText(string linkText)
        {
            return ConvertToWrapper(webDriverWrapperService.FindElementByLinkText(webDriver, linkText), linkText);
        }

        public IWebElement FindElementByName(string name)
        {
            return ConvertToWrapper(webDriverWrapperService.FindElementByName(webDriver, name), name);
        }

        public IWebElement FindElementByPartialLinkText(string partialLinkText)
        {
            return ConvertToWrapper(webDriverWrapperService.FindElementByPartialLinkText(webDriver, partialLinkText), partialLinkText);
        }

        public IWebElement FindElementByTagName(string tagName)
        {
            return ConvertToWrapper(webDriverWrapperService.FindElementByTagName(webDriver, tagName), tagName);
        }

        public IWebElement FindElementByXPath(string xpath)
        {
            return ConvertToWrapper(webDriverWrapperService.FindElementByXPath(webDriver, xpath), xpath);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            int count = 0;
            return new ReadOnlyCollection<IWebElement>(webDriverWrapperService.FindElements(webDriver, by)
                        .Select(b => ConvertToWrapper(b, $"{by.ToString()} [{count++}]"))
                        .Cast<IWebElement>().ToList());
        }

        public ReadOnlyCollection<IWebElement> FindElementsByClassName(string className)
        {
            int count = 0;
            return new ReadOnlyCollection<IWebElement>(webDriverWrapperService.FindElementsByClassName(webDriver, className)
                            .Select(b => ConvertToWrapper(b, $"{className.ToString()} [{count++}]"))
                            .Cast<IWebElement>().ToList());
        }

        public ReadOnlyCollection<IWebElement> FindElementsByCssSelector(string cssSelector)
        {
            int count = 0;
            return new ReadOnlyCollection<IWebElement>(webDriverWrapperService.FindElementsByCssSelector(webDriver, cssSelector)
                .Select(b => ConvertToWrapper(b, $"{cssSelector.ToString()} [{count++}]"))
                            .Cast<IWebElement>().ToList());
        }

        public ReadOnlyCollection<IWebElement> FindElementsById(string id)
        {
            int count = 0;
            return new ReadOnlyCollection<IWebElement>(webDriverWrapperService.FindElementsById(webDriver, id)
                .Select(b => ConvertToWrapper(b, $"{id.ToString()} [{count++}]"))
                            .Cast<IWebElement>().ToList());
        }

        public ReadOnlyCollection<IWebElement> FindElementsByLinkText(string linkText)
        {
            int count = 0;
            return new ReadOnlyCollection<IWebElement>(webDriverWrapperService.FindElementsByLinkText(webDriver, linkText)
                .Select(b => ConvertToWrapper(b, $"{linkText.ToString()} [{count++}]"))
                            .Cast<IWebElement>().ToList());
        }

        public ReadOnlyCollection<IWebElement> FindElementsByName(string name)
        {
            int count = 0;
            return new ReadOnlyCollection<IWebElement>(webDriverWrapperService.FindElementsByName(webDriver, name)
                .Select(b => ConvertToWrapper(b, $"{name.ToString()} [{count++}]"))
                            .Cast<IWebElement>().ToList());
        }

        public ReadOnlyCollection<IWebElement> FindElementsByPartialLinkText(string partialLinkText)
        {
            int count = 0;
            return new ReadOnlyCollection<IWebElement>(webDriverWrapperService.FindElementsByPartialLinkText(webDriver, partialLinkText)
                .Select(b => ConvertToWrapper(b, $"{partialLinkText.ToString()} [{count++}]"))
                            .Cast<IWebElement>().ToList());
        }

        public ReadOnlyCollection<IWebElement> FindElementsByTagName(string tagName)
        {
            int count = 0;
            return new ReadOnlyCollection<IWebElement>(webDriverWrapperService.FindElementsByTagName(webDriver, tagName)
                .Select(b => ConvertToWrapper(b, $"{tagName.ToString()} [{count++}]"))
                            .Cast<IWebElement>().ToList());
        }

        public ReadOnlyCollection<IWebElement> FindElementsByXPath(string xpath)
        {
            int count = 0;
            return new ReadOnlyCollection<IWebElement>(webDriverWrapperService.FindElementsByXPath(webDriver, xpath)
                .Select(b => ConvertToWrapper(b, $"{xpath.ToString()} [{count++}]"))
                            .Cast<IWebElement>().ToList());
        }

        public Screenshot GetScreenshot()
        {
            return webDriverWrapperService.GetScreenshot(webDriver);
        }

        public IOptions Manage()
        {
            return webDriverWrapperService.Manage(webDriver);
        }

        public INavigation Navigate()
        {
            return webDriverWrapperService.Navigate(webDriver);
        }

        public void PerformActions(IList<ActionSequence> actionSequenceList)
        {
            webDriverWrapperService.PerformActions(webDriver, actionSequenceList);
        }

        public void Quit()
        {
            webDriverWrapperService.Quit(webDriver);
        }

        

        public ITargetLocator SwitchTo()
        {
            return webDriverWrapperService.SwitchTo(webDriver);
        }
        
        public bool RunJS(String Description, String Javascript, int timeOut, out Object returnType, List<Object> arguments = null)
        {
            return webDriverWrapperService.RunJS(webDriver, Description, Javascript, timeOut, out returnType, arguments);
        }
        
        public bool Focus(String Name, By by, int timeOut, IWebElement scoped = null)
        {
            return webDriverWrapperService.Focus(webDriver, Name, by, timeOut, scoped);
        }
        public bool GetElements(String Name, By by, int timeOut, out List<IWebElement> elements, IWebElement scoped = null, int minimumFound = 0, int maximumFound = Int32.MaxValue)
        {
            return webDriverWrapperService.GetElements(webDriver, Name, by, timeOut, out elements, scoped, minimumFound, maximumFound);
        }
        public bool HoverLocation(String Name, int x, int y, int timeOut, IWebElement scoped = null)
        {
            return webDriverWrapperService.HoverLocation(webDriver, Name, x, y, timeOut, scoped);
        }
        public bool ClickLocation(String Name, int x, int y, int timeOut, IWebElement scoped = null)
        {
            return webDriverWrapperService.ClickLocation(webDriver, Name, x, y, timeOut, scoped);
        }
        public bool ClickElement(String Name, By by, int timeOut, IWebElement scoped = null)
        {
            return webDriverWrapperService.ClickElement(webDriver, Name, by, timeOut, scoped);
        }
        public bool TypeIn(String Name, By by, String Text, int timeOut, IWebElement scoped = null)
        {
            return webDriverWrapperService.TypeIn(webDriver, Name, by, Text, timeOut, scoped);
        }
        public bool WaitUntil(String Name, int sec, Func<IWebDriver, bool> callback)
        {
            return webDriverWrapperService.WaitUntil(webDriver, Name, sec, callback);
        }
        public bool WaitForIt<T>(String Name, int sec, Func<IWebDriver, T> callback, out T result)
        {
            return webDriverWrapperService.WaitForIt<T>(webDriver, Name, sec, callback, out result);
        }
        public bool GoTo(String Url)
        {
            return webDriverWrapperService.GoTo(webDriver, Url);
        }

        public void ResetInputState()
        {
            webDriverWrapperService.ResetInputState(webDriver);
        }
        public IWebDriver RawDriver()
        {
            return webDriver;
        }
    }
}
