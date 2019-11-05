using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Pages.Shared;
using MC.Track.TestSuite.Interfaces.Proxies;
using MC.Track.TestSuite.Interfaces.Resources;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Model.Domain;
using MC.Track.TestSuite.Model.Enums;
using MC.Track.TestSuite.Model.EventArgs;
using MC.Track.TestSuite.Model.Helpers;
using MC.Track.TestSuite.Model.Types;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MC.Track.TestSuite.Driver;
using System.Collections.ObjectModel;
using MC.Track.TestSuite.Interfaces.Dependencies;
using TechTalk.SpecFlow;

namespace MC.Track.TestSuite.Interfaces.Services
{
    /// <summary>
    /// Used to manage the custom implmentations to the dynamic user pool managment system
    /// </summary>
    public class DynamicUserCustomManagment {
        public static String ParameterName = "__EnableDynamicUserChecking__";
        private readonly IResolver resolver;

        public bool IsEnabled { get; set; }
        public List<String> SkipFreeing { get; set; }
        public DynamicUserCustomManagment(IResolver resolver)
        {
            IsEnabled = false;
            SkipFreeing = new List<string>();
            this.resolver = resolver;
        }

        /// <summary>
        /// This will inject different functonality into the test suite so that when a user is request for any means,
        /// a fake user is created. This means that they are users that solution wise are the same but are not able to login
        /// to the portal
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public void EnableDynamicUserCustomManagment()
        {
            IsEnabled = true;
        }

        /// <summary>
        /// This will inject different functonality into the test suite so that when a user is request for any means,
        /// a fake user is created. This means that they are users that solution wise are the same but are not able to login
        /// to the portal
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public void DisableDynamicUserCustomManagment()
        {
            IsEnabled = false;
        }

        /// <summary>
        /// This will inject different functonality into the test suite so that when a user is request for any means,
        /// a fake user is created. This means that they are users that solution wise are the same but are not able to login
        /// to the portal
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public void Setup()
        {
            resolver.InterceptClass<IDynamicUserFactoryService>().Action<String>(b => b.FreeUser).BeforeCalled(arg =>
              {
                  if (!IsEnabled)
                      return;

                  var userEmail = arg.GetArgmentByName("email");

                  // If its considered a email to skip then we done call the real function
                  if (SkipFreeing.Contains(userEmail))
                  {
                      arg.Execute = false;
                  }
              });
        }

        /// <summary>
        /// This will inject different functonality into the test suite so that when a user is request for any means,
        /// a fake user is created. This means that they are users that solution wise are the same but are not able to login
        /// to the portal
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public IScoper UseFakeUserPool()
        {
            this.IsEnabled = true;

            var eventDistroyerService = resolver.InterceptClass<IDynamicUserFactoryService>()
                                                .Func<DynamicUserType>(b => b.TakeUser)
                                                .BeforeCalled(arg =>
                                                {
                                                    arg.Execute = false;

                                                    var guideID = Guid.NewGuid().ToString().Replace("-", "");
                                                    var dynamicUser = new DynamicUserType()
                                                    {
                                                        email = $"dummy_{guideID}@email.com",
                                                        firstName = "Dummy",
                                                        lastName = $"{guideID}",
                                                        inUse = true,
                                                        password = "DummyPassword",
                                                        userID = $"{guideID}"
                                                    };
                                                    arg.ReturnObj = dynamicUser;
                                                    SkipFreeing.Add(dynamicUser.email);
                                                });
            return resolver.Resolve<IGenericScopingFactory>().Create(() =>
            {
                eventDistroyerService.DistroyNow();
            });
        }
    }

    public static class ResolverExtensions
    {

        public static void EnableWebElementWrapping(this IResolver resolver)
        {
            
        }

        public static void LoggingOnSelnium(this IResolver resolver)
        {
            //resolver.InterceptClass<IWebDriverWrapperService>().Func<By, IWebElement>(b => b.FindElement).BeforeCalled(arg =>
            //{
            //    resolver.Resolve<ILogger>().LogTrace(arg.ReturnObj.ToString());
            //});
            //resolver.InterceptClass<IWebElementWrapperService>().Func<By, IWebElement>(b => b.FindElement).BeforeCalled(arg =>
            //{
            //    IWebElementWrapper webElement = (IWebElementWrapper)arg.Arguments("element");
            //    IWebElementWrapper currentOne = (IWebElementWrapper)arg.TargetObj;
            //    resolver.Resolve<ILogger>().LogTrace($"SCOPED('{webElement.Name}') > '{currentOne.Name}'");
            //});
        }

        public static void EnablePrettyHtmlReporting(this IResolver resolver)
        {
            var fileCreator = resolver.Resolve<IMagicFactory<ITemporaryFileResource, TemporaryFileSettings>>();
            var htmlSummaryFile = fileCreator.Create(new TemporaryFileSettings()
            {
                Extension = "html",
                Name = " Summary",
                Title = "HTML Summary File",
                UploadToTestCloadStorageWhenFinished = true,
                SaveFileWhenFinished = true
            });


        }
        public static void UploadSaveFilesToBlobStorage(this IResolver resolver)
        {
            resolver.InterceptClass<IFileSaverService>().Action<String>(b => b.Save).BeforeCalled(arg =>
            {
                var filePath = (String)arg.GetArgmentByName("FilePath");
                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;
                var day = DateTime.Now.Day;
                
                var path = $"{Path.GetFileName(filePath).Replace(" ", "_")}";
                var stgService = resolver.Resolve<IStorageService>(StorageServiceTypes.TestServices);
                var result = stgService.UploadFile("testlogs", path, filePath).GetAwaiter().GetResult();
                XConsole.WriteLine($"  File  - {result}");
            });
        }
        public static void DontCloseBrowserOnExit(this IResolver resolver)
        {
            resolver.InterceptClass<IWebDriverWrapperService>().Action<IWebDriver>(b => b.Dispose).Disable();
            resolver.InterceptClass<IWebDriverWrapperService>().Action<IWebDriver>(b => b.Close).Disable();
        }
        public static void EnableScreenshotOnBrowserClose(this IResolver resolver)
        {
            resolver.InterceptClass<IWebDriverWrapperService>().Action<IWebDriver>(b => b.Dispose).BeforeCalled(arg =>
            {
                try
                {
                    IWebDriver webDriver = (IWebDriver)arg.GetArgmentByName("webDriver");
                    var webDriverService = (IWebDriverWrapperService)arg.TargetObj;
                    resolver.Resolve<IDependencies>().Browser().CurrentBrowser.TakeScreenshot("Closing");
                }
                catch(Exception e)
                {
                    resolver.Resolve<ILogger>().LogError(e);
                }
            });
        }
        public static void EnableScreenshotOnClicks(this IResolver resolver)
        {
            
            resolver.InterceptClass<ITrackBrowser>().Func<String, By, int?, bool>(b => b.ClickElement).AfterCalled(arg =>
            {
                resolver.Resolve<ILogger>().IndentDown();
            });
            resolver.InterceptClass<ITrackBrowser>().Func<String, By,int?, bool>(b => b.ClickElement).BeforeCalled(arg =>
            {
                var name = (String)arg.GetArgmentByName("Name");
                resolver.Resolve<ILogger>().Section($"PrettyHeader 209|Clicking|{name}");
            });
            resolver.InterceptClass<ITrackBrowser>().Func<String, By, int?, bool>(b => b.ClickElement).AfterCalled(arg =>
            {
                resolver.Resolve<ILogger>().IndentDown();
            });
            resolver.InterceptClass<ITrackBrowser>().Func<String, By, String, bool>(b => b.TypeIn).BeforeCalled(arg =>
            {
                var name = (String)arg.GetArgmentByName("Name");
                var text = (String)arg.GetArgmentByName("Text");
                resolver.Resolve<ILogger>().Section($"PrettyHeader 123|Typing|'{text}' in {name}");
            });
            resolver.InterceptClass<ITrackBrowser>().Func<String, By, String, bool>(b => b.TypeIn).AfterCalled(arg =>
            {
                resolver.Resolve<ILogger>().IndentDown();
            });
            resolver.InterceptClass<ITestableTrackBrowser>().Func<String, By, bool>(b => b.VerifyLabel).BeforeCalled(arg =>
            {
                var name = (String)arg.GetArgmentByName("Name");
                resolver.Resolve<ILogger>().Section($"PrettyHeader 54|Verifying|{name}");
            });
            resolver.InterceptClass<ITestableTrackBrowser>().Func<String, By, bool>(b => b.VerifyLabel).AfterCalled(arg =>
            {
                resolver.Resolve<ILogger>().IndentDown();
            });
            resolver.InterceptClass<ITestableTrackBrowser>().Func<String, By,Action<List<IWebElementWrapper>>, int?,int,int, bool>(b => b.GetElements).BeforeCalled(arg =>
            {
                var name = (String)arg.GetArgmentByName("Name");
                var minimumFound = (int)arg.GetArgmentByName("minimumFound");
                var maximumFound = (int)arg.GetArgmentByName("maximumFound");
                if (minimumFound == 1 && maximumFound == 1) {
                    resolver.Resolve<ILogger>().Section($"PrettyHeader 42|Getting Element|{name}");
                }
                else
                {
                    resolver.Resolve<ILogger>().Section($"PrettyHeader 23|Getting all Element|{name}");
                }
            });
            resolver.InterceptClass<ITestableTrackBrowser>().Func<String, By, Action<List<IWebElementWrapper>>, int?, int, int, bool>(b => b.GetElements).AfterCalled(arg =>
            {
                resolver.Resolve<ILogger>().IndentDown();
            });


            // ****************************
            resolver.Resolve<IStateManagment>().Set("__WAITING_TO_VERIFY_SOMETHING__", true);
           
            resolver.InterceptClass<ITrackBrowser>().Func<String, By, int?, bool>(b => b.ClickElement).BeforeCalled(arg =>
            {
                IWebElementWrapper webElement = null;
                if (resolver.Resolve<IDependencies>().Browser().CurrentBrowser.GetElements((String)arg.GetArgmentByName("Name"), (By)arg.GetArgmentByName("by"), (elements) => webElement = elements[0], (int?)arg.GetArgmentByName("WaitForSelectorTimeout"), 1, 1))
                {
                    resolver.Resolve<IDependencies>().Browser().CurrentBrowser.RunDefaultJSModuleFunction<String>("UIHooking", "HighlightElement", webElement.CompletlyUnprotected(), "rgba(55,155,255,1)", 3);
                }

                String name = (String)arg.GetArgmentByName("Name");
                resolver.Resolve<IStateManagment>().Set("__WAITING_TO_VERIFY_SOMETHING__", true);
                resolver.Resolve<IDependencies>().Browser().CurrentBrowser.TakeScreenshot($"Clicking {name}");
            });
            resolver.InterceptClass<ITrackBrowser>().Func<String, By, String, bool>(b => b.TypeIn).BeforeCalled(arg =>
            {
                IWebElementWrapper webElement = null;
                if (resolver.Resolve<IDependencies>().Browser().CurrentBrowser.GetElements((String)arg.GetArgmentByName("Name"), (By)arg.GetArgmentByName("by"), (elements) => webElement = elements[0], minimumFound: 1, maximumFound: 1))
                {
                    resolver.Resolve<IDependencies>().Browser().CurrentBrowser.RunDefaultJSModuleFunction<String>("UIHooking", "HighlightElement", webElement.CompletlyUnprotected(), "rgba(55,155,255,1)", 1);
                }

                String name = (String)arg.GetArgmentByName("Name");
                if (resolver.Resolve<IStateManagment>().Get<bool>("__WAITING_TO_VERIFY_SOMETHING__"))
                {
                    resolver.Resolve<IStateManagment>().Set("__WAITING_TO_VERIFY_SOMETHING__", true);
                    resolver.Resolve<IDependencies>().Browser().CurrentBrowser.TakeScreenshot($"Typeing in {name}");
                }
            });
            resolver.InterceptClass<ITestableTrackBrowser>().Func<String, By, bool>(b => b.VerifyLabel).AfterCalled(arg =>
            {
                IWebElementWrapper webElement = null;
                if (resolver.Resolve<IDependencies>().Browser().CurrentBrowser.GetElements((String)arg.GetArgmentByName("Name"), (By)arg.GetArgmentByName("by"), (elements) => webElement = elements[0], minimumFound: 1, maximumFound: 1))
                {
                    resolver.Resolve<IDependencies>().Browser().CurrentBrowser.RunDefaultJSModuleFunction<String>("UIHooking", "HighlightElement", webElement.CompletlyUnprotected(), "rgba(55,255,55,1)", 5);
                }

                String name = (String)arg.GetArgmentByName("Name");
                if (resolver.Resolve<IStateManagment>().Get<bool>("__WAITING_TO_VERIFY_SOMETHING__"))
                {
                    resolver.Resolve<IStateManagment>().Set("__WAITING_TO_VERIFY_SOMETHING__", false);
                    resolver.Resolve<IDependencies>().Browser().CurrentBrowser.TakeScreenshot($"Verifying {name}");
                }
            });
        }
        /// <summary>
        /// This will inject different functonality into the test suite so that when a user is request for any means,
        /// a fake user is created. This means that they are users that solution wise are the same but are not able to login
        /// to the portal
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public static DynamicUserCustomManagment DynamicUserPoolManagment(this IResolver resolver)
        {
            lock (resolver)
            {
                if (!resolver.Resolve<IStateManagment>().Has(DynamicUserCustomManagment.ParameterName))
                {
                    resolver.Resolve<IStateManagment>().Set(DynamicUserCustomManagment.ParameterName, new DynamicUserCustomManagment(resolver));
                    resolver.DynamicUserPoolManagment().Setup();
                }

                return resolver.Resolve<IStateManagment>().Get<DynamicUserCustomManagment>(DynamicUserCustomManagment.ParameterName);
            }
        }

        public static void CaptureSqlActivity(this IResolver resolver)
        {
            var fileCreator = resolver.Resolve<IMagicFactory<ITemporaryFileResource, TemporaryFileSettings>>();
            var htmlSummaryFile = fileCreator.Create(new TemporaryFileSettings()
            {
                Extension = "sql",
                Name = " Sql",
                Title = "Sql Activity",
                UploadToTestCloadStorageWhenFinished = false,
                SaveFileWhenFinished = true
            });

            //resolver.InterceptClass<ISqlCommandService>().Func<int>(b => b.ExecuteNonQuery).BeforeCalled(arg =>
            //  {
            //      arg.
            //  });
        }

        public static void EnableHtmlSummary(this IResolver resolver)
        {
            

        }
    }
}
