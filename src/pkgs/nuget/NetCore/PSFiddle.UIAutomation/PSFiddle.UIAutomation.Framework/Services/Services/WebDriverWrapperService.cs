using MC.Track.TestSuite.Interfaces.Driver;
using OpenQA.Selenium;
using OpenQA.Selenium.Html5;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Model.Helpers;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Internal;
using MC.Track.TestSuite.Interfaces.Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MC.Track.TestSuite.Services.Services
{
    [AthenaRegister(typeof(IWebDriverWrapperService), Model.Enums.AthenaRegistrationType.Singleton)]
    public class WebDriverWrapperService : IWebDriverWrapperService
    {
        private readonly IRunnerDependencies runnerDependencies;

        public WebDriverWrapperService(IRunnerDependencies dependencies)
        {
            this.runnerDependencies = dependencies;
        }
        public string GetUrl(IWebDriver webDriver) {
            return webDriver.Url;
        }
        public void SetUrl(IWebDriver webDriver, String Url)
        {
            webDriver.Url = Url;
        }

        public string Title(IWebDriver webDriver)
        {
            return webDriver.Title;
        }

        public string PageSource(IWebDriver webDriver)
        {
            return webDriver.PageSource;
        }

        public string CurrentWindowHandle(IWebDriver webDriver)
        {
            return webDriver.CurrentWindowHandle;
        }

        public ReadOnlyCollection<string> WindowHandles(IWebDriver webDriver)
        {
            return webDriver.WindowHandles;
        }

        public SessionId SessionId(IHasSessionId webDriver)
        {
            return webDriver.SessionId;
        }

        public bool IsActionExecutor(IActionExecutor webDriver)
        {
            return webDriver.IsActionExecutor;
        }

        public bool HasApplicationCache(IHasApplicationCache webDriver)
        {
            return webDriver.HasApplicationCache;
        }

        public IApplicationCache ApplicationCache(IHasApplicationCache webDriver)
        {
            return webDriver.ApplicationCache;
        }

        public IFileDetector GetFileDetector(IAllowsFileDetection webDriver)
        {
            return webDriver.FileDetector;
        }
        public void SetFileDetector(IAllowsFileDetection webDriver, IFileDetector detector)
        {
            webDriver.FileDetector = detector;
        }
       

        public bool HasLocationContext(IHasLocationContext webDriver)
        {
            return webDriver.HasLocationContext;
        }

        public ILocationContext LocationContext(IHasLocationContext webDriver)
        {
            return webDriver.LocationContext;
        }

        public bool HasWebStorage(IHasWebStorage webDriver)
        {
            return webDriver.HasWebStorage;
        }

        public IWebStorage WebStorage(IHasWebStorage webDriver)
        {
            return webDriver.WebStorage;
        }

        public ICapabilities Capabilities(IHasCapabilities webDriver)
        {
            return webDriver.Capabilities;
        }

        public IKeyboard Keyboard(IHasInputDevices webDriver)
        {
            return webDriver.Keyboard;
        }

        public IMouse Mouse(IHasInputDevices webDriver)
        {
            return webDriver.Mouse;
        }

        public void Close(IWebDriver webDriver)
        {
            webDriver.Close();
        }

        public void Dispose(IWebDriver webDriver)
        {
            webDriver.Dispose();
        }

        public object ExecuteAsyncScript(IJavaScriptExecutor webDriver, string script, params object[] args)
        {
            return webDriver.ExecuteAsyncScript(script, args);
        }

        public object ExecuteScript(IJavaScriptExecutor webDriver, string script, params object[] args)
        {
            return webDriver.ExecuteScript(script, args);
        }

        public IWebElement FindElement(IWebDriver webDriver, By by)
        {
            return webDriver.FindElement(by);
        }

        public IWebElement FindElementByClassName(IFindsByClassName webDriver, string className)
        {
            return webDriver.FindElementByClassName(className);
        }

        public IWebElement FindElementByCssSelector(IFindsByCssSelector webDriver, string cssSelector)
        {
            return webDriver.FindElementByCssSelector(cssSelector);
        }

        public IWebElement FindElementById(IFindsById webDriver, string id)
        {
            return webDriver.FindElementById(id);
        }

        public IWebElement FindElementByLinkText(IFindsByLinkText webDriver, string linkText)
        {
            return webDriver.FindElementByLinkText(linkText);
        }

        public IWebElement FindElementByName(IFindsByName webDriver, string name)
        {
            return webDriver.FindElementByName(name);
        }

        public IWebElement FindElementByPartialLinkText(IFindsByPartialLinkText webDriver, string partialLinkText)
        {
            return webDriver.FindElementByPartialLinkText(partialLinkText);
        }

        public IWebElement FindElementByTagName(IFindsByTagName webDriver, string tagName)
        {
            return webDriver.FindElementByTagName(tagName);
        }

        public IWebElement FindElementByXPath(IFindsByXPath webDriver, string xpath)
        {
            return webDriver.FindElementByXPath(xpath);
        }

        public ReadOnlyCollection<IWebElement> FindElements(IWebDriver webDriver, By by)
        {
            return webDriver.FindElements(by);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByClassName(IFindsByClassName webDriver, string className)
        {
            return webDriver.FindElementsByClassName(className);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByCssSelector(IFindsByCssSelector webDriver, string cssSelector)
        {
            return webDriver.FindElementsByCssSelector(cssSelector);
        }

        public ReadOnlyCollection<IWebElement> FindElementsById(IFindsById webDriver, string id)
        {
            return webDriver.FindElementsById(id);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByLinkText(IFindsByLinkText webDriver, string linkText)
        {
            return webDriver.FindElementsByLinkText(linkText);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByName(IFindsByName webDriver, string name)
        {
            return webDriver.FindElementsByName(name);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByPartialLinkText(IFindsByPartialLinkText webDriver, string partialLinkText)
        {
            return webDriver.FindElementsByPartialLinkText(partialLinkText);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByTagName(IFindsByTagName webDriver, string tagName)
        {
            return webDriver.FindElementsByTagName(tagName);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByXPath(IFindsByXPath webDriver, string xpath)
        {
            return webDriver.FindElementsByXPath(xpath);
        }

        public Screenshot GetScreenshot(ITakesScreenshot webDriver)
        {
            return webDriver.GetScreenshot();
        }

        public IOptions Manage(IWebDriver webDriver)
        {
            return webDriver.Manage();
        }

        public INavigation Navigate(IWebDriver webDriver)
        {
            return webDriver.Navigate();
        }

        public void PerformActions(IActionExecutor webDriver,IList<ActionSequence> actionSequenceList)
        {
            webDriver.PerformActions(actionSequenceList);
        }

        public void Quit(IWebDriver webDriver)
        {
            webDriver.Quit();
        }

        public void ResetInputState(IActionExecutor webDriver)
        {
            webDriver.ResetInputState();
        }

        public ITargetLocator SwitchTo(IWebDriver webDriver)
        {
            return webDriver.SwitchTo();
        }

        /// <summary>
        /// Will run a peice of javascript given a description and arguments
        /// </summary>
        /// <param name="driver">Web Driver</param>
        /// <param name="Name">The name of the element</param>
        /// <param name="by">The selector</param>
        /// <param name="timeOut">A timeout</param>
        /// <param name="scoped">Scopped element</param>
        /// <returns></returns>
        public bool RunJS(IWebDriver webDriver, String Description, String Javascript, int timeOut, out Object returnType, List<Object> arguments = null)
        {
            return runnerDependencies.TryTimeoutAction(
                EvaluationAction: () =>
                 {
                     IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)webDriver;
                     return javaScriptExecutor.ExecuteScript(Javascript, arguments.ToArray());
                 },
                Output: out returnType,
                OnFailure: (error) => XConsole.WriteLine($"*BROKE* - RunJS '{Description}' - {error.Message} "),
                OnSuccess: (result) => XConsole.WriteLine($"  Ok    - RunJS '{Description}' - {result}"),
                Timeout: TimeSpan.FromSeconds(timeOut),
                Title: $"**RunJS** - {Description}",
                ExceptionOnEndingFailure: (_exception) => throw new Exception($"Failed to execute javascript - {_exception?.Message}"));
        }

        /// <summary>
        /// Will Focus on the element selected. Focusing meaning scrolling element into view
        /// </summary>
        /// <param name="driver">Web Driver</param>
        /// <param name="Name">The name of the element</param>
        /// <param name="by">The selector</param>
        /// <param name="timeOut">A timeout</param>
        /// <param name="scoped">Scopped element</param>
        /// <returns></returns>
        public bool Focus(IWebDriver webDriver, String Name, By by, int timeOut, IWebElement scoped = null)
        {
            return runnerDependencies.TryTimeoutAction(
                    EvaluationAction: () =>
                    {
                        List<IWebElement> elements = new List<IWebElement>();
                        if (!GetElements(webDriver, Name, by, timeOut, out elements, scoped, 1, 1))
                            throw new Exception("Failed to get element to focus on");

                        var clickElement = elements.First();
                        if (clickElement is IWebElementWrapper)
                            clickElement = ((IWebElementWrapper)clickElement).CompletlyUnprotected();

                        IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
                        js.ExecuteScript("arguments[0].scrollIntoView(true);", clickElement);

                    },
                    Timeout: TimeSpan.FromSeconds(timeOut),
                    OnFailureOfIteration: (error) => XConsole.WriteLine($"*BROKE* - Focus[{Name}] - {error.Message} "),
                    OnSuccess: () => XConsole.WriteLine($"  Ok    - Focus[{Name}])"),
                    Title: $"**Focusing On Element** - {Name} | _{by.ToString()}_"
                );

        }
        public bool GetElements(IWebDriver webDriver, String Name, By by, int timeOut, out List<IWebElement> elements, IWebElement scoped = null, int minimumFound = 0, int maximumFound = Int32.MaxValue)
        {
            return runnerDependencies.TryTimeoutAction(
                    EvaluationAction: () =>
                    {
                        var foundElements = scoped != null ? scoped.FindElements(by) : webDriver.FindElements(by);

                        if (foundElements.Count >= minimumFound && foundElements.Count <= maximumFound)
                            return foundElements.ToList();

                        throw new Exception($"Found {foundElements.Count} elements. {(maximumFound == Int32.MaxValue ? $"Expected more then {minimumFound}" : $"Expected between {minimumFound} to {maximumFound}")} to be found... ");
                    },
                    Timeout: TimeSpan.FromSeconds(timeOut),
                    DefaultOutput: () => new List<IWebElement>(),
                    Output: out elements,
                    OnFailure: (error) => XConsole.WriteLine($"*BROKE* - GetElements[{Name}] - *ERROR*({error.Message}) "),
                    OnSuccess: (found) => XConsole.WriteLine($"  Ok    - GetElements[{Name}][{found.Count}]"),
                    Title: (minimumFound == 1 && maximumFound == 1) ? ($"**Get Element** - {Name} | _{by.ToString()}_") :
                                                                      ($"**Get Elements** - {Name} | _{by.ToString()}_")
               );

        }

        public bool HoverLocation(IWebDriver webDriver, String Name, int x, int y, int timeOut, IWebElement scoped = null)
        {
            return runnerDependencies.TryTimeoutAction(
                    EvaluationAction: () =>
                    {
                        Actions actions = new Actions(webDriver);
                        if (scoped != null)
                            actions.MoveToElement(scoped);

                        actions.MoveByOffset(x, y)
                               .Build()
                               .Perform();
                    },
                    Timeout: TimeSpan.FromSeconds(timeOut),
                    OnFailure: (error) => XConsole.WriteLine($"*BROKE* - Hover[{x},{y}] - {error.Message} "),
                    OnSuccess: () => XConsole.WriteLine($"  Ok    - Hover[{x},{y}] "),
                    Title: $"**Hover over location** - [{x},{y}] on ({Name})"
               );
        }
        public bool ClickLocation(IWebDriver webDriver, String Name, int x, int y, int timeOut, IWebElement scoped = null)
        {
            return runnerDependencies.TryTimeoutAction(
                    EvaluationAction: () =>
                    {
                        Actions actions = new Actions(webDriver);
                        if (scoped != null)
                            actions.MoveToElement(scoped, x, y);
                        else
                            actions.MoveByOffset(x, y);

                        actions.Click()
                               .Build()
                               .Perform();
                    },
                    Timeout: TimeSpan.FromSeconds(timeOut),
                    OnFailure: (error) => XConsole.WriteLine($"*BROKE* - Click[{x},{y}] - {error.Message} "),
                    OnSuccess: () => XConsole.WriteLine($"  Ok    - Click[{x},{y}] "),
                    Title: $"**Click over location** - [{x},{y}] on ({Name})"
               );
        }
        public bool ClickElement(IWebDriver webDriver, String Name, By by, int timeOut, IWebElement scoped = null)
        {
            return runnerDependencies.TryTimeoutAction(
                    EvaluationAction: () =>
                    {
                        List<IWebElement> elements = new List<IWebElement>();
                        if (!GetElements(webDriver, Name, by, timeOut, out elements, scoped, 1, 1))
                            throw new Exception("Failed to get element to focus on");

                        IWebElement clickElement = elements.First();

                        // Focus on it
                        Focus(webDriver, Name, by, timeOut, scoped);

                        if (!(clickElement.Displayed && clickElement.Enabled))
                            throw new Exception("Not visable");

                        // Click it
                        clickElement.Click();
                    },
                    Timeout: TimeSpan.FromSeconds(timeOut),
                    OnFailure: (error) => XConsole.WriteLine($"*BROKE* - Click[{Name}] - {error.Message} "),
                    OnSuccess: () => XConsole.WriteLine($"  Ok    - Click[{Name}]: "),
                    Title: $"**Click element** - {Name} | _{by.ToString()}_"
               );
        }
        public bool TypeIn(IWebDriver webDriver, String Name, By by, String Text, int timeOut, IWebElement scoped = null)
        {
            return runnerDependencies.TryTimeoutAction(
                   EvaluationAction: () =>
                   {
                       List<IWebElement> elements = new List<IWebElement>();
                       if (!GetElements(webDriver, Name, by, timeOut, out elements, scoped, 1, 1))
                           throw new Exception("Failed to get element to focus on");

                       IWebElement typeInElement = elements.First();

                       // Focus on it
                       Focus(webDriver, Name, by, timeOut, scoped);

                       // Type in it
                       typeInElement.SendKeys(Text);
                   },
                   Timeout: TimeSpan.FromSeconds(timeOut),
                   OnFailure: (error) => XConsole.WriteLine($"*BROKE* - Type[{Name}][{Text}] - {error.Message} "),
                   OnSuccess: () => XConsole.WriteLine($"  Ok    - Type[{Name}][{Text}]: "),
                   Title: $"**Typing in element** - Text(**{Text}**), Element({Name} | _{by.ToString()}_)"
               );
        }
        public bool WaitUntil(IWebDriver webDriver, String Name, int sec, Func<IWebDriver, bool> callback)
        {
            return runnerDependencies.TryTimeoutAction(
                   EvaluationAction: () =>
                   {
                       Assert.IsTrue(callback(webDriver), "Failed to call wait");
                   },
                   Timeout: TimeSpan.FromSeconds(sec),
                   OnFailure: (error) => XConsole.WriteLine($"*BROKE* - WaitUntil[{Name}] - {error.Message} "),
                   OnSuccess: () => XConsole.WriteLine($"  Ok    - WaitUntil[{Name}] "),
                   Title: $"**Generic Waiting Until** - {Name}"
               );
        }
        public bool WaitForIt<T>(IWebDriver webDriver, String Name, int sec, Func<IWebDriver, T> callback, out T result)
        {
            return runnerDependencies.TryTimeoutAction(
                   EvaluationAction: () =>
                   {
                       return callback(webDriver);
                   },
                   Output: out result,
                   Timeout: TimeSpan.FromSeconds(sec),
                   OnFailure: (error) => XConsole.WriteLine($"*BROKE* - WaitForIt[{Name}] - {error.Message} "),
                   OnSuccess: (item) => XConsole.WriteLine($"  Ok    - WaitForIt[{Name}] "),
                   Title: $"**Generic Waiting For It** - {Name}"
               );
        }
        public bool GoTo(IWebDriver webDriver, String Url)
        {
            return runnerDependencies.TryTimeoutAction(
                   EvaluationAction: () =>
                   {
                       webDriver.Navigate().GoToUrl(Url);
                   },
                   OnFailure: (error) => XConsole.WriteLine($"*BROKE* - GoTo[{Url}] - {error.Message}"),
                   OnSuccess: () => XConsole.WriteLine($"  Ok    - GoTo[{Url}] "),
                   Title: $"**Go to URL** - {Url}"
               );
        }

        public SessionId SessionId(IWebDriver webDriver) => SessionId((IHasSessionId)webDriver);
        
        public bool HasApplicationCache(IWebDriver webDriver) => HasApplicationCache((IHasApplicationCache)webDriver);

        public bool IsActionExecutor(IWebDriver webDriver) => IsActionExecutor((IActionExecutor)webDriver);

        public IApplicationCache ApplicationCache(IWebDriver webDriver) => ApplicationCache((IHasApplicationCache)webDriver);

        public void SetFileDetector(IWebDriver webDriver, IFileDetector value) => SetFileDetector((IAllowsFileDetection)webDriver, value);

        public IFileDetector GetFileDetector(IWebDriver webDriver) => GetFileDetector((IAllowsFileDetection)webDriver);

        public bool HasLocationContext(IWebDriver webDriver) => HasLocationContext((IHasLocationContext)webDriver);

        public ILocationContext LocationContext(IWebDriver webDriver) => LocationContext((IHasLocationContext)webDriver);

        public bool HasWebStorage(IWebDriver webDriver) => HasWebStorage((IHasWebStorage)webDriver);

        public IWebStorage WebStorage(IWebDriver webDriver) => WebStorage((IWebDriver)webDriver);

        public ICapabilities Capabilities(IWebDriver webDriver) => Capabilities((IHasCapabilities)webDriver);

        public IKeyboard Keyboard(IWebDriver webDriver) => Keyboard((IHasInputDevices)webDriver);

        public IMouse Mouse(IWebDriver webDriver) => Mouse((IHasInputDevices)webDriver);

        public object ExecuteAsyncScript(IWebDriver webDriver, string script, object[] args) => ExecuteAsyncScript((IJavaScriptExecutor)webDriver, script, args);

        public object ExecuteScript(IWebDriver webDriver, string script, object[] args) => ExecuteScript((IJavaScriptExecutor)webDriver, script, args);

        public IWebElement FindElementByClassName(IWebDriver webDriver, string className) => FindElementByClassName((IFindsByClassName)webDriver, className);

        public IWebElement FindElementByCssSelector(IWebDriver webDriver, string cssSelector) => FindElementByCssSelector((IFindsByCssSelector)webDriver, cssSelector);


        public IWebElement FindElementById(IWebDriver webDriver, string id) => FindElementById((IFindsById)webDriver, id);

        public IWebElement FindElementByLinkText(IWebDriver webDriver, string linkText)
        {
            return FindElementByLinkText((IFindsByLinkText)webDriver, linkText);
        }

        public IWebElement FindElementByName(IWebDriver webDriver, string name)
        {
            return FindElementByName((IFindsByName)webDriver, name);
        }

        public IWebElement FindElementByPartialLinkText(IWebDriver webDriver, string partialLinkText)
        {
            return FindElementByPartialLinkText((IFindsByPartialLinkText)webDriver, partialLinkText);
        }

        public void ResetInputState(IWebDriver webDriver)
        {
            ResetInputState((IActionExecutor)webDriver);
        }

        public IWebElement FindElementByTagName(IWebDriver webDriver, string tagName)
        {
            return FindElementByTagName((IFindsByTagName)webDriver, tagName);
        }

        public void PerformActions(IWebDriver webDriver, IList<ActionSequence> actionSequenceList)
        {
            PerformActions((IActionExecutor)webDriver, actionSequenceList);
        }

        public Screenshot GetScreenshot(IWebDriver webDriver)
        {
            return GetScreenshot((ITakesScreenshot)webDriver);
        }

        public IEnumerable<IWebElement> FindElementsByXPath(IWebDriver webDriver, string xpath)
        {
            return FindElementsByXPath((IFindsByXPath)webDriver, xpath);
        }

        public IWebElement FindElementByXPath(IWebDriver webDriver, string xpath)
        {
            return FindElementByXPath((IFindsByXPath)webDriver, xpath);
        }

        public IEnumerable<IWebElement> FindElementsByTagName(IWebDriver webDriver, string tagName)
        {
            return FindElementsByTagName((IFindsByTagName)webDriver, tagName);
        }

        public IEnumerable<IWebElement> FindElementsByPartialLinkText(IWebDriver webDriver, string partialLinkText)
        {
            return FindElementsByPartialLinkText((IFindsByPartialLinkText)webDriver, partialLinkText);
        }

        public IEnumerable<IWebElement> FindElementsByName(IWebDriver webDriver, string name)
        {
            return FindElementsByName((IFindsByName)webDriver, name);
        }

        public IEnumerable<IWebElement> FindElementsByLinkText(IWebDriver webDriver, string linkText)
        {
            return FindElementsByLinkText((IFindsByLinkText)webDriver, linkText);
        }

        public IEnumerable<IWebElement> FindElementsById(IWebDriver webDriver, string id)
        {
            return FindElementsById((IFindsById)webDriver, id);
        }

        public IEnumerable<IWebElement> FindElementsByClassName(IWebDriver webDriver, string className)
        {
            return FindElementsByClassName((IFindsByClassName)webDriver, className);
        }

        public IEnumerable<IWebElement> FindElementsByCssSelector(IWebDriver webDriver, string cssSelector)
        {
            return FindElementsByCssSelector((IFindsByCssSelector)webDriver, cssSelector);
        }
    }
}
