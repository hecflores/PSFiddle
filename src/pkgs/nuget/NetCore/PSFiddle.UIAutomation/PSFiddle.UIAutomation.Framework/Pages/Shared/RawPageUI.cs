using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Toolkit.Extensions;
using MC.Track.TestSuite.Interfaces.Config;
using MC.Track.TestSuite.Interfaces.Services.Functional;
using MC.Track.TestSuite.UI.Types;
using MC.Track.Shared;
using PSFiddle.UIAutomation.Framework.Shared;
using System.IO;
using OpenQA.Selenium;
using MC.Track.TestSuite.Toolkit.Dependencies;
using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Interfaces.Pages;
using MC.Track.TestSuite.Interfaces.Pages.Shared;


using Newtonsoft.Json;
using System.Reflection;
using PSFiddle.UIAutomation.Framework.Exceptions;
using PSFiddle.UIAutomation.Framework.Extensions;
using MC.Track.TestSuite.Model.Helpers;

namespace MC.Track.TestSuite.Toolkit.Pages.Shared
{
    public abstract class RawPage : IRawPage
    {
        private  IStateManagment stateManagment;
        private  IElementDiscovery elementDiscovery;
        private  IParameterParser parameterParser;
        private  IEmailService emailService;
        private  IFileManager fileManager;
        private  IFileSaverService fileSaverService;
        protected IResolver resolver;
        protected IConfiguration config;
        private  IDependencies Dependency;
        private bool _isSetup = false;

        private void CheckSetup()
        {
            Assert.IsTrue(_isSetup, $"The Page {this.GetType().Name} was not setup correctly");
        }
        public bool isSetup()
        {
            return _isSetup;
        }
        public void Setup(IResolver resolver)
        {
            if (this._isSetup) return;
            this.stateManagment = resolver.Resolve<IStateManagment>();
            this.elementDiscovery = resolver.Resolve<IElementDiscovery>();
            this.parameterParser = resolver.Resolve<IParameterParser>();
            this.emailService = resolver.Resolve<IEmailService>();
            this.fileManager = resolver.Resolve<IFileManager>();
            this.fileSaverService = resolver.Resolve<IFileSaverService>();
            this.config = resolver.Resolve<IConfiguration>();
            this.Dependency = resolver.Resolve<IDependencies>();
            this.resolver = resolver;
            this._isSetup = true;

        }

        
        public T CreatePage<T>() where T : class, IPageBase
        {
            var page = resolver.Resolve<T>();
            page.Setup(resolver);
            return page;
        }
        public void SwitchTabs(int tabIndex)
        {
            WaitFor(() =>
            {
                Browser.SwitchTabs(tabIndex);
            });
        }
        
        public virtual void ExpectSpinner()
        {
            CheckSetup();
            try
            {
                this.VerifyElementIsVisable(LocatorNames.Global_Spinner);
            }
            catch(Exception e)
            {
                XConsole.WriteLine("Spinner did not show up in the page"); //Todo :Need fix correctly.
            }
        }
        public virtual void ExpectNoSpinner()
        {
            CheckSetup();
            this.VerifyElementIsHidden(LocatorNames.Global_Spinner);
        }
        public virtual T TransitionPage<T>() where T : class, IPageBase
        {
            CheckSetup();

            var page = PageNewer.Create<T>();
            page = resolver.ApplyIntercepts(page);
            page.Setup(resolver);
            page.ValidatePage();
            return page;
        }
        public virtual T PartialPage<T>(IProtectedWebElement webElement) where T : class, IScopedPageUI
        {
            CheckSetup();

            var page = PageNewer.Create<T>();
            page = resolver.ApplyIntercepts(page);
            page.Setup(resolver, webElement, this);
            page.ValidatePage();
            return page;

        }
        
        public virtual bool IsSelected(String Name)
        {
            CheckSetup();
            var selector = this.elementDiscovery.Discover(ref Name);

            Assert.IsTrue(selector.Ok);
            return this.Browser.IsSelected(Name, selector.Value);
        }
        public virtual void VerifyIsSelected(String Name)
        {
            WaitFor(() =>
            {
                String displayName = Name;
                var selector = this.elementDiscovery.Discover(ref displayName);
                Assert.IsTrue(this.IsSelected(Name), $"Element {displayName} was expected to be selected but wasn't");
            });
        }
        public virtual void VerifyIsNotSelected(String Name)
        {
            WaitFor(() =>
            {
                String displayName = Name;
                var selector = this.elementDiscovery.Discover(ref displayName);
                Assert.IsFalse(this.IsSelected(Name), $"Element {displayName} was expected to be not selected but Selected");
            });
        }
        public T WaitFor<T>(Func<T> action, String messageIfFailed = null, int? timeout = null)
        {
            CheckSetup();

            var numberOfRetries    = timeout = timeout * 2;
            var delayBetweenReties = TimeSpan.FromMilliseconds(TimeSpan.FromSeconds(1).TotalMilliseconds / 2);

            return Dependency.Runner().RetryAction(
                EvaluationAction: action,
                DefaultOutput: () => default(T),
                Title: "Waiting for Generic Action",
                OnFailureOfIteration: (error) => XConsole.WriteLine(messageIfFailed ==null?$"Failed - {error.Message}":$"{messageIfFailed} - {error.Message}"),
                NumberOfRetries: numberOfRetries,
                DelayBetweenWaits: delayBetweenReties,
                ExceptionOnEndingFailure: (e) => messageIfFailed != null ? new Exception(messageIfFailed) : e
            );

        }
        public void WaitFor(Action action, String messageIfFailed = null, int? timeout = null)
        {
            CheckSetup();

            var numberOfRetries = timeout = timeout * 2;
            var delayBetweenReties = TimeSpan.FromMilliseconds(TimeSpan.FromSeconds(1).TotalMilliseconds / 2);

            Dependency.Runner().RetryAction(
                EvaluationAction: action,
                Title: "Waiting for Generic Action",
                NumberOfRetries: numberOfRetries,
                OnFailureOfIteration: (error) => XConsole.WriteLine(messageIfFailed == null ? $"Failed - {error.Message}" : $"{messageIfFailed} - {error.Message}"),
                DelayBetweenWaits: delayBetweenReties,
                ExceptionOnEndingFailure: (e) => messageIfFailed != null ? new Exception(messageIfFailed) : e
            );
        }
        public virtual void VerifyElementIsHidden(String Name)
        {
            CheckSetup();
            var element = this.GetElement(Name);
            element.VerifyElementIsHidden();
        }
        public virtual void VerifyElementIsVisable(String Name)
        {
            CheckSetup();
            var element = this.GetElement(Name);
            element.VerifyElementIsVisable();
        }
        private IScoper ScopeRaw(IWebElementWrapper element)
        {
            CheckSetup();
            return this.Browser.ScopeToElement(element);
        }
        public virtual IScoper Scope(IProtectedWebElement webElement)
        {
            CheckSetup();
            return this.ScopeRaw(webElement.Unprotected());
        }
        public T Get<T>(String Name)
        {
            CheckSetup();
            return this.stateManagment.Get<T>(Name);
        }
        private List<IWebElementWrapper> GetRawElements(String Name, int minimumFound = 0, int maximumFound = Int32.MaxValue)
        {
            CheckSetup();
            var selector = this.elementDiscovery.Discover(ref Name);
            List<IWebElementWrapper> returnElements = new List<IWebElementWrapper>();
            Assert.IsTrue(selector.Ok);

            if(!Browser.GetElements(Name, selector.Value, (elements) =>
            {
                returnElements = elements;
            }, minimumFound: minimumFound, maximumFound: maximumFound))
            {
                throw new PageObjectException(this.GetType().Name.UndoCamelCase(), $"Unable to fetch element {Name}");
            }

            return returnElements;
        }

        public virtual List<IProtectedWebElement> GetElements(String Name, int minimumFound = 0, int maximumFound = Int32.MaxValue)
        {
            CheckSetup();
            return GetRawElements(Name,minimumFound, maximumFound).Cast<IProtectedWebElement>().ToList();

        }
        private IWebElementWrapper GetRawElement(String Name)
        {
            CheckSetup();
            var elements = GetRawElements(Name, 1, 1);
            return elements[0];
        }
        public virtual IProtectedWebElement GetElement(String Name)
        {
            CheckSetup();
            var elements = GetElements(Name, 1, 1);
            return elements[0];
        }
        public void Set(String Name, Object obj)
        {
            this.stateManagment.Set(Name, obj);
        }
        public ITestableTrackBrowser Browser
        {
            get
            {
                CheckSetup();
                return Dependency.Browser().CurrentBrowser;
            }
            set
            {
                CheckSetup();
                Dependency.Browser().CurrentBrowser = value;
            }
        }
        public abstract void ValidatePage();
        public void SwitchToBrowser(ITestableTrackBrowser browser)
        {
            CheckSetup();
            Dependency.Browser().SwitchToBrowser(browser);
        }
        public void GoToHomePage()
        {
            CheckSetup();
            Browser.GoToHost();
        }
        public void CloseCurrentBrowserAndSwitch(ITestableTrackBrowser browser)
        {
            CheckSetup();
            Dependency.Browser().CloseCurrentBrowserAndSwitch(browser);
        }
        public virtual void ClickElement(String ElementName)
        {
            CheckSetup();
            var selector = this.elementDiscovery.Discover(ref ElementName);

            Assert.IsTrue(selector.Ok);
            MetisAssert.POMAssert.IsTrue(Browser.ClickElement(ElementName, selector.Value),this, $"Unable to click Element {ElementName}");
        }
        
        public virtual void ClickLocation(String Name, int x, int y)
        {
            CheckSetup();
            MetisAssert.POMAssert.IsTrue(Browser.ClickLocation(Name, x, y), this, $"Unable to click Location {x}, {y}");
        }
        public virtual void TypeIn(string Text, String ElementName)
        {
            CheckSetup();
            Text = this.parameterParser.Filter(Text);

            var selector = this.elementDiscovery.Discover(ref ElementName);

            Assert.IsTrue(selector.Ok);
            MetisAssert.POMAssert.IsTrue(Browser.TypeIn(ElementName, selector.Value, Text),this,$"Unable to type '{Text}' in element '{ElementName}'");
        }
        public virtual void VerifyElement(String Name)
        {
            CheckSetup();
            var selector = this.elementDiscovery.Discover(ref Name);
            Assert.IsTrue(selector.Ok);
            MetisAssert.POMAssert.IsTrue(Browser.VerifyLabel(Name, selector.Value),this, $"Element {Name} was expected to exists but didnt");
            
        }
        public virtual void VerifyElementText(String Name, String TestingText)
        {
            VerifyElement(Name);

            var ActualText = Text(Name);

            MetisAssert.POMAssert.IsTrue(ActualText == TestingText, this,$"Element {Name} was expected have to the text '{TestingText}' but instead contained '{ActualText}'");
        }
        public virtual void VerifyElementNotExists(String Name)
        {
            CheckSetup();
            var selector = this.elementDiscovery.Discover(ref Name);
            Assert.IsTrue(selector.Ok);
            MetisAssert.POMAssert.IsFalse(Browser.VerifyLabel(Name, selector.Value), this, $"Element {Name} was expected to not exists but did exists");
        }
        public virtual String Text(String Name)
        {
            CheckSetup();
            String value;
            var selector = this.elementDiscovery.Discover(ref Name);
            Assert.IsTrue(selector.Ok);
            MetisAssert.POMAssert.IsTrue(Browser.getText(Name, selector.Value, out value), this, $"Unable to get the text from element {Name}");
            Assert.IsNotNull(value);
            return value;
        }
        public void VerifyAlert()
        {
            CheckSetup();
            MetisAssert.POMAssert.IsTrue(Browser.AcceptAlert(), this, "Unable to accept browser alert");
        }
        public void EnterTextAndVerifyAlert(String text)
        {
            CheckSetup();
            MetisAssert.POMAssert.IsTrue(Browser.EnterTextForAlert(text), this, "Unable to enter any text in the alert");
            MetisAssert.POMAssert.IsTrue(Browser.AcceptAlert(), this, "Unable to accept browser alert");
        }
        public void EnterTextInActiveWindow(String text)
        {
            CheckSetup();
            MetisAssert.POMAssert.IsTrue(Browser.EnterTextInActiveWindow(text), this, "Unable to enter any text in the active window");
        }

        public T RunDefaultJSModuleFunction<T>(String Module, String Function, params Object[] arguments)
        {
            CheckSetup();
            return Browser.RunDefaultJSModuleFunction<T>(Module, Function, arguments);
        }
        public T RunJSModuleFunction<T>(String JsRelativeFilePath, String Module, String Function, params Object[] arguments)
        {
            CheckSetup();
            return Browser.RunJSModuleFunction<T>(JsRelativeFilePath, Module, Function, arguments);
        }

        private class PageNewer
        {
            public static T Create<T>() where T : class, IPageBase
            {
                return (T)Create(typeof(T), new List<KeyValuePair<Type, object>>() {});
            }
            public static T Create<T>(IResolver resolver) where T : class, IPageBase
            {
                return (T)Create(typeof(T), new List<KeyValuePair<Type, object>>() {
                        new KeyValuePair<Type, object>(typeof(IResolver), resolver) });
            }

            public static T Create<T, A1>(IResolver resolver, A1 a1) where T : class, IPageBase
            {
                return (T)Create(typeof(T), new List<KeyValuePair<Type, object>>() {
                        new KeyValuePair<Type, object>(typeof(IResolver), resolver),
                        new KeyValuePair<Type, object>(typeof(A1), a1)});
            }
            public static T Create<T, A1, A2>(IResolver resolver, A1 a1, A2 a2) where T : class, IPageBase
            {
                return (T)Create(typeof(T), new List<KeyValuePair<Type, object>>() {
                        new KeyValuePair<Type, object>(typeof(IResolver), resolver),
                        new KeyValuePair<Type, object>(typeof(A1), a1),
                        new KeyValuePair<Type, object>(typeof(A2), a2)});
            }
            public static T Create<T, A1, A2, A3>(IResolver resolver, A1 a1, A2 a2, A3 a3) where T : class, IPageBase
            {
                return (T)Create(typeof(T), new List<KeyValuePair<Type, object>>() {
                        new KeyValuePair<Type, object>(typeof(IResolver), resolver),
                        new KeyValuePair<Type, object>(typeof(A1), a1),
                        new KeyValuePair<Type, object>(typeof(A2), a2),
                        new KeyValuePair<Type, object>(typeof(A3), a3)});
            }
            public static T Create<T, A1, A2, A3, A4>(IResolver resolver, A1 a1, A2 a2, A3 a3, A4 a4) where T : class, IPageBase
            {
                return (T)Create(typeof(T), new List<KeyValuePair<Type, object>>() {
                        new KeyValuePair<Type, object>(typeof(IResolver), resolver),
                        new KeyValuePair<Type, object>(typeof(A1), a1),
                        new KeyValuePair<Type, object>(typeof(A2), a2),
                        new KeyValuePair<Type, object>(typeof(A3), a3),
                        new KeyValuePair<Type, object>(typeof(A4), a4)});
            }
            public static T Create<T, A1, A2, A3, A4, A5>(IResolver resolver, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5) where T : class, IPageBase
            {
                return (T)Create(typeof(T), new List<KeyValuePair<Type, object>>() {
                        new KeyValuePair<Type, object>(typeof(IResolver), resolver),
                        new KeyValuePair<Type, object>(typeof(A1), a1),
                        new KeyValuePair<Type, object>(typeof(A2), a2),
                        new KeyValuePair<Type, object>(typeof(A3), a3),
                        new KeyValuePair<Type, object>(typeof(A4), a4),
                        new KeyValuePair<Type, object>(typeof(A5), a5)});
            }
            private static IPageBase Create(Type creatingType, List<KeyValuePair<Type, Object>> parameters)
            {
                ConstructorInfo classConstructor = creatingType.GetConstructor(parameters.Select(b => b.Key).ToArray());

                IPageBase newed = (IPageBase)classConstructor.Invoke(parameters.Select(b => b.Value).ToArray());
                
                return newed;
            }
        }
    }
}
