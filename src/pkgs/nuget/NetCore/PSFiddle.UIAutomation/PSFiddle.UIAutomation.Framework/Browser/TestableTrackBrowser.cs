using MC.Track.TestSuite.Interfaces;
using MC.Track.TestSuite.Interfaces.Config;
using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Model.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Driver
{
    public class TestableTrackBrowser : TrackBrowser, ITestableTrackBrowser
    {
        private String _hostUrl;
        private IConfiguration configuration;
        private IFileManager fileManager;
        private readonly ILogger logger;
        private readonly IFileSaverService fileSaverService;
        private readonly ITrackTestRunner trackTestRunner;
        public TestableTrackBrowser(
                IConfiguration configuration,
                ILogger logger,
                IFileManager fileManager,
                IFileSaverService fileSaverService,
                ITrackTestRunner trackTestRunner,
                IGenericScopingFactory genericScopingFactory,
                IResolver resolver
                ) : base(configuration.DefaultTimeoutTime,
                         Path.Combine(configuration.SolutionPath, @"Drivers"), 
                         Path.Combine(configuration.SolutionPath,@"Scripts\Javascript"), 
                         fileManager, 
                         genericScopingFactory,
                         resolver)
        {
            this._hostUrl = configuration.HostUrl;
            this.logger = logger;
            this.fileSaverService = fileSaverService;
            this.trackTestRunner = trackTestRunner;
        }

        
        

        public bool Start()
        {
            return this.GoTo(this._hostUrl);
        }
        public bool GoToHost()
        {
            return this.GoTo(this._hostUrl);
        }
        public bool GoToRoute(String Route)
        {
            return this.GoTo(this._hostUrl + Route);
        }
        public bool VerifyLabel(String Name, By by)
        {
            return this.VerifyLabel(Name,this._defaultTimeoutSec, by, null);
        }
        
        public bool VerifyLabel(String Name, int sec, By by, String text = null)
        {
            IWebElement webElement;
            if (!this.WaitForIt<IWebElement>(Name, sec, (driver) => _scoped != null ? _scoped.FindElement(by) : driver.FindElement(by), out webElement))
            {
                return false;
            }

            try
            {
                WebElementWrapper.ConvertToWrapper(resolver, this._webDriver, Name, webElement).VerifyElementIsVisable();
            }
            catch(Exception e)
            {
                logger.LogError(e);
                return false;
            }
            

            // Text is null when we are just checking if the element exists...
            if (text == null)
            {
                return true;
            }

            // Check the text content
            if (webElement.Text == text)
            {
                return true;
            }

            return false;

            //IWebElementWrapper webElement = null;
            //if(!this.GetElements(Name, by, (elements) => webElement = elements[0] ,1, 1))
            //{
            //    return false;
            //}
            
            //// Sanity check
            //if (webElement == null)
            //   throw new Exception("Should Not Be Here: It was expected that the element was either set or we failed and returned. We should of never made it this far... Codeing issue... "+
            //                       "Possibly list is being populate with nulls or "+
            //                       "return value is not reflecting is actual step or "+
            //                       "the function is not honoring the input minimum and maximum values give");

            //// Verify we can see the element
            //webElement.VerifyElementIsVisable();

            //// Text is null when we are just checking if the element exists...
            //if (text == null)
            //{
            //    return true;
            //}

            //// Check the text content
            //if (webElement.Text == text)
            //{
            //    return true;
            //}

            //return false;
        }
        public bool WaitFor(String Name, By by)
        {
            return this.WaitForIt(Name, (driver) => _scoped != null ? _scoped.FindElement(by) : driver.FindElement(by));
        }

        public bool VerifyLabel(string Name, int sec, By by)
        {
            return this.VerifyLabel(Name, sec, by, null);
        }

        public bool VerifyLabel(string Name, By by, string text = null)
        {
            return this.VerifyLabel(Name, this._defaultTimeoutSec, by, null);
        }
        public override string TakeScreenshot(string Name)
        {
            var filePath = base.TakeScreenshot(Name);
            this.fileSaverService.Save(filePath);
            return filePath;
        }

        public bool VerifyAlertMessage(string Message)
        {
            try
            {
                IAlert alert = getAlert();
                if (alert == null)
                {
                    throw new Exception("Unable to get the alert");
                }
                return alert.Text.Contains(Message);
            }
            catch(Exception e)
            {
                XConsole.WriteLine($"*ERROR*, Failed to verify alert text: \n{e.Message}\n{e.StackTrace}");
                return false;
            }
        }
        
    }
}
