using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MC.Track.TestSuite.Model.Enums;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MC.Track.TestSuite.Model.Helpers;
using OpenQA.Selenium.Interactions.Internal;
using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Interfaces.Util;

namespace MC.Track.TestSuite.Driver
{
    public class WebElementWrapper : IWebElementWrapper
    {
        private readonly IWebDriverWrapper webDriver;
        private readonly IRunnerDependencies runnerDependencies;
        private readonly IWebElementWrapperService webElementWrapperService;
        private readonly IResolver resolver;
        private readonly ILogger logger;
        private readonly string name;
        private readonly IWebElement webElement;
        public String Name { get => name; }

        public WebElementWrapper(IResolver resolver, IWebDriverWrapper webDriver, String Name, IWebElement webelement)
        {
            this.resolver = resolver;
            this.logger = resolver.Resolve<ILogger>();
            name = Name;
            this.webElement = webelement;
            this.webElementWrapperService = resolver.Resolve<IWebElementWrapperService>();
            this.runnerDependencies = resolver.Resolve<IRunnerDependencies>();
            this.webDriver = webDriver;
        }
        public IWebElement CompletlyUnprotected()
        {
            return webElement;
        }

        public string TagName => webElementWrapperService.TagName(webElement);

        public string Text => webElementWrapperService.Text(webElement);

        public bool Enabled => webElementWrapperService.Enabled(webElement);

        public bool Selected => webElementWrapperService.Selected(webElement);

        public Point Location => webElementWrapperService.Location(webElement);

        public Size Size => webElementWrapperService.Size(webElement);

        public bool Displayed => webElementWrapperService.Displayed(webElement);

        public Point LocationOnScreenOnceScrolledIntoView => webElementWrapperService.LocationOnScreenOnceScrolledIntoView(webElement);

        public ICoordinates Coordinates => webElementWrapperService.Coordinates(webElement);

        public void Clear()
        {
            webElementWrapperService.Clear(webElement);
        }

        public void Click()
        {
            webElementWrapperService.Click(webElement);
        }

        public IWebElement FindElement(By by)
        {
            return ConvertToWrapper(webElementWrapperService.FindElement(webElement, by));
        }
        private IWebElementWrapper ConvertToWrapper(IWebElement element)
        {
            if (!(element is IWebElementWrapper))
                return resolver.ApplyIntercepts<IWebElementWrapper>(new WebElementWrapper(resolver, this.webDriver, this.name, (element)));

            return (IWebElementWrapper)element;
        }
        public static IWebElementWrapper ConvertToWrapper(IResolver resolver, IWebDriverWrapper webDriver, string name, IWebElement element)
        {
            if (!(element is IWebElementWrapper))
                return resolver.ApplyIntercepts<IWebElementWrapper>(new WebElementWrapper(resolver, webDriver, name, (element)));

            return (IWebElementWrapper)element;
        }
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return new ReadOnlyCollection<IWebElement>(webElementWrapperService.FindElements(webElement, by)
                        .Select(b => ConvertToWrapper(b))
                        .Cast<IWebElement>().ToList());
        }
        
        public string GetAttribute(string attributeName)
        {
            return webElementWrapperService.GetAttribute(webElement, attributeName);
        }

        public string GetCssValue(string propertyName)
        {
            return webElementWrapperService.GetCssValue(webElement, propertyName);
        }

        public string GetProperty(string propertyName)
        {
            return webElementWrapperService.GetProperty(webElement, propertyName);
        }

        public void SendKeys(string text)
        {
            webElementWrapperService.SendKeys(webElement, text);
        }

        public void Submit()
        {
            webElementWrapperService.Submit(webElement);
        }


        public void VerifyElementIsNotEnabled()
        {
            runnerDependencies.RetryAction(
                EvaluationAction: () => Assert.IsFalse(Enabled),
                Title: $"Verify Element Is Not Enabled: **{name}**",
                ExceptionOnEndingFailure: (e) =>  new Exception($"Expected element [{name}] to not be enabled"));
        }

        public void VerifyFAIcon(FAIcons iconType)
        {
            // Can come back with null
            var iconClassAttr = webElementWrapperService.GetAttribute(webElement, "class");
            // Check for null here
            Assert.IsNotNull(iconClassAttr, "Expected class attribute; not found.");

            // Check if class contains fas
            Assert.IsTrue(iconClassAttr.Contains("fas"), "Invalid FA icon.");

            switch (iconType)
            {
                case FAIcons.faLock:
                    Assert.IsTrue(iconClassAttr.Contains("fa-lock"), "Could not find fa-lock icon in element.");
                    break;
                case FAIcons.faQuestionCircle:
                    Assert.IsTrue(iconClassAttr.Contains("fa-question-circle"), "Could not find fa-question-circle icon in element.");
                    break;
                case FAIcons.faTriangle:
                    Assert.IsTrue(iconClassAttr.Contains("fa-exclamation-triangle"), "Could not find fa-exclamation-triangle icon in element.");
                    break;
                case FAIcons.faExclamationMark:
                    Assert.IsTrue(iconClassAttr.Contains("fa-exclamation"), "Could not find fa-exclamation icon in element.");
                    break;
                case FAIcons.faBan:
                    Assert.IsTrue(iconClassAttr.Contains("fa-ban"), "Could not find fa-ban icon in element.");
                    break;
                default:
                    Assert.IsFalse(false, "Given iconType is not a valid iconType.");
                    break;
            }
        }

        public void VerifyElementIsVisable()
        {
            runnerDependencies.RetryAction(
                EvaluationAction: () =>
                {
                    Assert.IsTrue(Focus());
                    Assert.IsTrue(Displayed);
                },
                Title: $"Verify Element Is Visable: **{name}**",
                ExceptionOnEndingFailure: (e) => new Exception($"Expected element [{name}] to be visable but it wasnt or was not able to be scrolled to..."));
        }

        public void VerifyElementIsHidden()
        {
            runnerDependencies.RetryAction(
                EvaluationAction: () =>Assert.IsFalse(Displayed),
                Title: $"Verify Element Is Hidden: **{name}**",
                ExceptionOnEndingFailure: (e) => new Exception($"Expected element [{name}] to be hidden but it wasnt"));
        }
        public void VerifyElementHasText()
        {
            runnerDependencies.RetryAction(
                EvaluationAction: () => Assert.IsTrue(!String.IsNullOrEmpty(Text), $"Expected element [{name}] to have text but didnt"),
                Title: $"Verify Element Has Text: **{name}**",
                ExceptionOnEndingFailure: (e) => new Exception($"Expected element [{name}] to have text but didnt"));
        }
        public void ClickIt()
        {
            runnerDependencies.RetryAction(
                EvaluationAction: () =>
                {
                    this.VerifyElementIsVisable();
                    this.Click();
                },
                Title: $"Clicking Verify Element: **{name}**",
                ExceptionOnEndingFailure: (e) => new Exception($"Unable to click element [{name}]"));
        }

        public String GetText()
        {
            return Text;
        }
        public IWebElementWrapper Unprotected()
        {
            return this;
        }

        public IToolTip ToolTip()
        {
            runnerDependencies.RetryAction(
                EvaluationAction: () =>Assert.IsTrue(this.HasToolTip(), $"[{name}] seems to not have a tooltip"),
                Title: $"Getting Tooltip: **{name}**",
                ExceptionOnEndingFailure: (e) => new Exception($"[{name}] seems to not have a tooltip"));
            
            return this;
        }

        public void Hover()
        {
            Point location = Location;
            webDriver.HoverLocation(Name, location.X, location.Y, 2);
        }

        public bool HasToolTip()
        {
            return String.IsNullOrEmpty(GetAttribute("title"));
        }

        public string GetTooltipText()
        {
            return GetAttribute("title");
        }
        
        public bool Focus()
        {
            try
            {
                webDriver.ExecuteScript("arguments[0].scrollIntoView(true);", this.CompletlyUnprotected());

                XConsole.WriteLine($"  Ok    - Focus[{Name}]");
                return true;
            }
            catch (Exception e)
            {
                XConsole.WriteLine($"*BROKE* - Focus[{Name}] - {e.Message} ");
                return false;
            }
            
        }

        public IWebDriverWrapper Driver()
        {
            return this.webDriver;
        }
    }
}
