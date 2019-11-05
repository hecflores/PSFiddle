using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Extensions;
using OpenQA.Selenium.Firefox;
using MC.Track.TestSuite.Interfaces.Driver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MC.Track.TestSuite.Interfaces.Services;
using System.Text.RegularExpressions;
using MC.Track.TestSuite.Model.Helpers;
using OpenQA.Selenium.Interactions;
using Newtonsoft.Json;
using MC.Track.TestSuite.Interfaces.Config;

namespace MC.Track.TestSuite.Driver
{
    public class TrackBrowser : ITrackBrowser
    {
        public int _defaultTimeoutSec = 5;
        protected IWebElementWrapper _scoped { get; set; }

        protected IWebDriverWrapper _webDriver;
        protected String _solutionPath;
        protected String _javascriptModuleFolderPath;
        private IFileManager fileManager;
        private IGenericScopingFactory genericScopingFactory;
        protected readonly IResolver resolver;
        protected IConfiguration configuration;
        public TrackBrowser(
            int defaultTimeoutSec,
            String SolutionPath,
            String JavascriptModuleFolderPath,
            IFileManager fileManager,
            IGenericScopingFactory genericScopingFactory,
            IResolver resolver
            )
        {
            this._solutionPath = SolutionPath;
            this.fileManager = fileManager;
            this.genericScopingFactory = genericScopingFactory;
            this.resolver = resolver;
            this._javascriptModuleFolderPath = JavascriptModuleFolderPath;
            this._defaultTimeoutSec = defaultTimeoutSec;
            this.configuration = resolver.Resolve<IConfiguration>();


        }
        public IWebDriver SeleniumDriver()
        {
            return this._webDriver;
        }
        public void SwitchTabs(int tabIndex)
        {
            var tabs = _webDriver.WindowHandles;
            _webDriver.SwitchTo().Window(tabs[tabIndex]);
        }
        public IScoper ScopeToElement(IWebElementWrapper webElement)
        {
            var oldScope = this._scoped;
            this._scoped = webElement;
            return this.genericScopingFactory.Create(
            () =>
            {
                this._scoped = oldScope;
            });
        }
        public bool GetElements(String Name, By by,  Action<List<IWebElementWrapper>> callback, int? WaitForSelectorTimeout = null, int minimumFound = 0, int maximumFound = Int32.MaxValue)
        {
            WaitForSelectorTimeout = WaitForSelectorTimeout.HasValue ? WaitForSelectorTimeout : _defaultTimeoutSec;
            List <IWebElement> elements;
            if(!this._webDriver.GetElements(Name, by, WaitForSelectorTimeout.Value,  out elements, _scoped, minimumFound, maximumFound))
            {
                return false;
            }

            callback(elements.Select(b=>WebElementWrapper.ConvertToWrapper(resolver,_webDriver, Name, b)).Cast<IWebElementWrapper>().ToList());
            return true;
        }
        public bool Focus(String Name, By by)
        {
            return this._webDriver.Focus(Name, by, this._defaultTimeoutSec, this._scoped);
        }
        public bool IsSelected(String Name, By by)
        {
            if(!this.Focus(Name, by))
            {
                throw new Exception("Cant get is Selected because focus broke");
            }
            return this.GetElement(Name, by).Selected;
        }

        public bool GetElements(By by, Action<List<IWebElementWrapper>> callback)
        {
            return this.GetElements(by.ToString(), by, callback);
        }
        public void Dispose()
        {
            this._webDriver.Dispose();
        }
        public void DefaultTimeoutSec(int sec)
        {
            this._defaultTimeoutSec = sec;
        }        
        public virtual void Initialize()
        {
            this._webDriver = new WebDriverWrapper(resolver, (IWebDriverAllInterfaces)(new ChromeDriver(this._solutionPath)));
        }
        public bool ClickElement(String Name, By by, int? WaitForSelectorTimeout = null)
        {
            WaitForSelectorTimeout = WaitForSelectorTimeout.HasValue ? WaitForSelectorTimeout : _defaultTimeoutSec;

            var result = this._webDriver.ClickElement(Name, by, WaitForSelectorTimeout.Value, _scoped);
            return result;
        }
        public bool TypeIn(String Name, By by, String Text, int WaitForSelectorTimeout)
        {
            var result = this._webDriver.TypeIn(Name, by, Text, WaitForSelectorTimeout, _scoped);
            return result;
        }

        public bool GetAllOptions(String Name, By by, String Text,Action<List<string>> callback)
        {
            List<IWebElement> elements;
            if (!this._webDriver.GetElements(Name, by, this._defaultTimeoutSec, out elements, _scoped))
            {
                return false;
            }
            callback(elements.Select(b => b.Text).ToList());
            return true;
        }
        public bool WaitUntil(String Name, int sec, Func<IWebDriver, bool> callback)
        {
            return this._webDriver.WaitUntil(Name, sec, callback);
        }
        public bool WaitForIt<T>(String Name, int sec, Func<IWebDriver, T> callback, out T result)
        {
            return this._webDriver.WaitForIt<T>(Name, sec, callback, out result);
        }
        public bool ClickElement(By by)
        {
            return this.ClickElement(by.ToString(), by, this._defaultTimeoutSec);
        }
        public bool getText(String Name, By by, out String text)
        {
            List<IWebElementWrapper> elements = new List<IWebElementWrapper>();
            if (!this.GetElements(Name, by, (items) => elements = items, minimumFound: 1, maximumFound: 1))
            {
                text = null;
                XConsole.WriteLine("Failed to get Elements");
                return false;
            }
           
            text = elements[0].Text;
            return true;

        }
        public IWebElementWrapper GetElement(String Name, By by)
        {
            List<IWebElementWrapper> elements = new List<IWebElementWrapper>();
            if (!this.GetElements(Name, by, (items) => elements = items,minimumFound: 1,maximumFound: 1))
            {
                throw new Exception("Failed to get Elements");
            }
            
            
            return elements[0];

        }
        public bool TypeIn(String Name, By by, String Text)
        {
            return this.TypeIn(Name, by, Text, this._defaultTimeoutSec);
        }
        
        public bool WaitUntil(String Name, Func<IWebDriver, bool> callback)
        {
            return this.WaitUntil(Name, this._defaultTimeoutSec, callback);
        }
        
        
        public bool WaitForIt<T>(String Name, Func<IWebDriver, T> callback, out T result)
        {
            return this.WaitForIt<T>(Name, this._defaultTimeoutSec, callback, out result);
        }
        public bool WaitForIt<T>(String Name, Func<IWebDriver, T> callback)
        {
            T variableNotNeeded;
            return this.WaitForIt<T>(Name, callback, out variableNotNeeded);
        }
        public bool GoTo(String Url)
        {
            return this._webDriver.GoTo(Url);
        }
        
        public String GetHostUrl()
        {
            return this._webDriver.Url;
        }
        public String GetRoute()
        {
            return Regex.Replace(this._webDriver.Url, @"^(.*?)(\/.*|$)$", "$2");
        }


        public virtual string TakeScreenshot(String Name)
        {
            var fileName = this.fileManager.GenerateFileName("Images", Name, "jpg");
            ITakesScreenshot screenCapture = (ITakesScreenshot)this._webDriver;

            try
            {
                // Capture
                var capture = screenCapture.GetScreenshot();
                capture.SaveAsFile(fileName);

            }
            catch (Exception e)
            {
                String logPath = Path.ChangeExtension(fileName, "big.err");
                File.WriteAllText(logPath, $"Screen Capture Failed\nMessage...:{e.Message}\n{e.StackTrace}");

                fileName = logPath;
            }

            return fileName;
        }
        protected IAlert getAlert()
        {
            IAlert alert;
            if (!this.WaitForIt<IAlert>("Alert", (driver) => driver.SwitchTo().Alert(), out alert))
            {
                return null;
            }
            return alert;
        }
        public bool AcceptAlert()
        {
            try
            {
                IAlert alert = getAlert();
                if (alert == null)
                {
                    return false;
                }
                alert.Accept();
                return true;
            }
            catch(Exception e)
            {
                XConsole.WriteLine($"*ERROR*, Failed to accept the alter: \n{e.Message}\n{e.StackTrace}");
                return false;
            }
        }

       
        public bool EnterTextForAlert(string Text)
        {
            try
            {
                
                IAlert alert = getAlert();
                if (alert == null)
                {
                    return false;
                }
                alert.SendKeys(Text);
                return true;
            }
            catch (Exception e)
            {
                XConsole.WriteLine($"*ERROR*, Failed to type in the alter: \n{e.Message}\n{e.StackTrace}");
                return false;
            }
        }

        public bool EnterTextInActiveWindow(string Text)
        {
            try
            {

                IWebElementWrapper element;
                if (!this.WaitForIt<IWebElementWrapper>("ActiveElement", (driver) => new WebElementWrapper(resolver,_webDriver, null, driver.SwitchTo().ActiveElement()), out element))
                {
                    return false;
                }

                element.SendKeys(Text);
                return true;
            }
            catch (Exception e)
            {
                XConsole.WriteLine($"*ERROR*, Failed to type in the alter: \n{e.Message}\n{e.StackTrace}");
                return false;
            }
        }
        public bool ClickLocation(String Name, int x, int y)
        {
            return _webDriver.ClickLocation(Name, x, y, this._defaultTimeoutSec, this._scoped);
        }
        public T RunDefaultJSModuleFunction<T>(String Module, String Function, params Object[] arguments)
        {
            return RunJSModuleFunction<T>("Scripts/Javascript", Module, Function, arguments);
        }

        public T RunJSModuleFunction<T>(String JsRelativeFilePath, String Module, String Function, params Object[] arguments)
        {
            var jsFile = Path.Combine(configuration.SolutionPath, JsRelativeFilePath);
            Object returnType = null;
            Assert.IsTrue(RunJSModule((result) => returnType = result, jsFile, Module, Function, arguments.ToList()), $"Failed to run JS module '{Module}', '{Function}'");
            Assert.IsTrue(returnType != null, $"JS module '{Module}', '{Function}' returned a null object, not expected");
            Assert.IsTrue(returnType is String, $"JS module '{Module}', '{Function}' return type '{returnType.GetType()}' != execpted return type '{typeof(String)}'");

            String strReturnType = (String)returnType;
            if (!(typeof(T).Equals(typeof(String))))
            {
                return JsonConvert.DeserializeObject<T>(strReturnType);
            }
            return (T)returnType;
        }
        public bool RunDefaultJSModule(Action<Object> callback, String ModuleName, String FunctionName, List<Object> arguments)
        {
            return RunJSModule(callback, _javascriptModuleFolderPath, ModuleName, FunctionName, arguments);
        }
        public bool RunJSModule(Action<Object> callback, String JsFile, String ModuleName, String FunctionName, List<Object> arguments)
        {
            try
            { 
                var javascriptFile = Path.Combine(JsFile, $"{ModuleName}.js");
                if (!File.Exists(javascriptFile))
                {
                    throw new Exception($"*ERROR*, Failed to locate javascript module file '{javascriptFile}'");
                }
                var javascriptContent = File.ReadAllText(javascriptFile);
                Func<String, String> wrappedJavascriptForCallback = (endingCall) => @"
    var argumentArray=[];
    for (let item in arguments){
        if (!arguments.hasOwnProperty(item)) {
            break;
        }
        argumentArray.push(arguments[item]);
    }

    var callingFunc=function(){"
       + javascriptContent + @"
    }

    var obj = callingFunc()['" + FunctionName + "']" + endingCall + @"
    return JSON.stringify(obj);";

                // Final Javascript snippets
                var callbackJavascript = wrappedJavascriptForCallback(".callback.apply(null, argumentArray)");
                var descriptionJavascript = wrappedJavascriptForCallback(".description");

                // Getting Description
                Object descriptionObj = null;
                if (!this._webDriver.RunJS($"Load JS Module '{ModuleName}', '{FunctionName}'", descriptionJavascript,_defaultTimeoutSec,  out descriptionObj, arguments))
                {
                    throw new Exception($"*ERROR*, Unable to load module '{ModuleName}', '{FunctionName}' description");
                }

                if (!(descriptionObj is String))
                {
                    throw new Exception($"*ERROR*, Description from JS Module '{ModuleName}', '{FunctionName}' was not of type string ");
                }

                var description = (String)descriptionObj;

                if (String.IsNullOrEmpty(description))
                {
                    throw new Exception($"*ERROR*, Description from JS Module '{ModuleName}', '{FunctionName}' was null or empty");
                }

                // Getting Callback
                Object returnType = null;
                if (!this._webDriver.RunJS(description, callbackJavascript, _defaultTimeoutSec, out returnType, arguments))
                {
                    throw new Exception($"*ERROR*, Unable to call module '{ModuleName}', '{FunctionName}' callback");
                }

                callback(returnType);
                return true;
            }
            catch(Exception e)
            {
                XConsole.WriteLine($"*ERROR*, Failed to run module '{ModuleName}', '{FunctionName}' \n{e.Message}\n{e.StackTrace}");
                return false;
            }
        }

        
    }
}
