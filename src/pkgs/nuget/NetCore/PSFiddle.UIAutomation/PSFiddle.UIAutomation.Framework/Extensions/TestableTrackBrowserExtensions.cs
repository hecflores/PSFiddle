using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;

namespace MC.Track.TestSuite.Toolkit.Extensions
{
    public static class TestableTrackBrowserExtensions
    {
        public static void ScreenshotOnExceptions(this ITestableTrackBrowser browser, Action callback)
        {
            try
            {
                callback();
            }
            catch(Exception e)
            {
                browser.TakeScreenshot($"Exception - {e.Message}");
            }
        }
        public static IScoper DisableScreenshots(this IResolver resolver)
        {
            return resolver.InterceptClass<ITestableTrackBrowser>()
                           .Func<String, String>(b => b.TakeScreenshot)
                           .Disable()
                           .ScopedDistroy();
        }

        public static void TrackLogin(this ITestableTrackBrowser browser,IResolver resolver, String Username, String Password, String HostUrl)
        {
            Assert.IsNotNull(Username);
            Assert.IsNotNull(Password);

            // User User name
            resolver.Resolve<IDependencies>().Runner().RetryAction(
                EvaluationAction: () =>
                {
                    browser.ScreenshotOnExceptions(() => // Will take a screenshot if exception... Note, if exception happends the disabling will be disposed and screenshots will be enabled again
                    {
                        using (resolver.DisableScreenshots()) // Will disable screenshots on every click....
                        {
                            if (browser.ClickElement("Sign in with different account", Selectors.Alpha_SignInWithDifferentAccount, 2))
                                XConsole.WriteLine("*** Clicked Sign in with Different account *** ");

                            if (browser.ClickElement("Use another account", Selectors.Alpha_UseAnother, 2))
                                XConsole.WriteLine("*** Clicked Use another *** ");

                            Assert.IsTrue(browser.ClickElement("User Name", Selectors.Alpha_LoginUserName, 2));
                            Assert.IsTrue(browser.TypeIn("User Name", Selectors.Alpha_LoginUserName, Keys.Control + "a", 2));
                            Assert.IsTrue(browser.TypeIn("User Name", Selectors.Alpha_LoginUserName, Username, 2));

                            Assert.IsTrue(browser.ClickElement("Login button", Selectors.Alpha_LoginButton, 2));

                            Assert.IsFalse(browser.VerifyLabel("Username Error", 2, Selectors.Alpha_UsernameError));
                        }
                    });
                },
                Title: "Enter User name...",
                NumberOfRetries: 5
            );

            resolver.Resolve<IDependencies>().Runner().RetryAction(
                EvaluationAction: () =>
                {
                    browser.ScreenshotOnExceptions(() =>
                    {
                        using (resolver.DisableScreenshots())
                        {
                            Assert.IsTrue(browser.ClickElement("Password", Selectors.Alpha_LoginPassword, 2));
                            Assert.IsTrue(browser.TypeIn("Password", Selectors.Alpha_LoginPassword, Keys.Control + "a", 2));
                            Assert.IsTrue(browser.TypeIn("Password", Selectors.Alpha_LoginPassword, Password));

                            Assert.IsTrue(browser.ClickElement("Login button", Selectors.Alpha_LoginButton));

                            Assert.IsFalse(browser.VerifyLabel("Password Error", 2, Selectors.Alpha_PasswordError));

                            if (browser.ClickElement("Request Change Button", Selectors.Alpha_RequestChangePopup, 2))
                                XConsole.WriteLine("*** Clicked Request Change Button *** ");

                            if (browser.ClickElement("Accept Conditions", Selectors.Alpha_LoginButton, 2))
                                XConsole.WriteLine("*** Clicked Accept Conditions *** ");

                            if (browser.ClickElement("Continue as is", Selectors.Alpha_LoginButton, 2))
                                XConsole.WriteLine("*** Clicked Continue *** ");
                        }
                    });
                },
                Title: "Enter Password...",
                NumberOfRetries: 5
            );

            browser.ScreenshotOnExceptions(() =>
            {
                using (resolver.DisableScreenshots()) // Will disable screenshots on every click....
                {
                    browser.ClickElement("Accept Content", Selectors.Alpha_AcceptContent, 1);
                    browser.WaitFor("I agree button", Selectors.Alpha_I_Agree);
                    browser.ClickElement("I agree button", Selectors.Alpha_I_Agree, 1);
                }
            });

        }
    }
}
