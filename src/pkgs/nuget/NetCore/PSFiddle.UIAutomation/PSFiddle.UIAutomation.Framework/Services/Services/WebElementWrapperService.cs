using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services
{
    [AthenaRegister(typeof(IWebElementWrapperService),Model.Enums.AthenaRegistrationType.Singleton)]
    public class WebElementWrapperService : IWebElementWrapperService
    {
        private readonly IResolver resolver;

        public WebElementWrapperService(IResolver resolver)
        {
            this.resolver = resolver;
        }
        public string TagName(IWebElement element)
        {
            return element.TagName;
        }

        public string Text(IWebElement element)
        {
            return element.Text;
        }

        public bool Enabled(IWebElement element)
        {
            return element.Enabled;
        }

        public bool Selected(IWebElement element)
        {
            return element.Selected;
        }

        public Point Location(IWebElement element)
        {
            return element.Location;
        }

        public Size Size(IWebElement element)
        {
            return element.Size;
        }

        public bool Displayed(IWebElement element)
        {
            return element.Displayed;
        }

        public void Clear(IWebElement element)
        {
            element.Clear();
        }

        public void Click(IWebElement element)
        {
            element.Click();
        }

        public IWebElement FindElement(IWebElement element, By by)
        {
            return element.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(IWebElement element, By by)
        {
            return element.FindElements(by);
        }

        public string GetAttribute(IWebElement element, string attributeName)
        {
            return element.GetAttribute(attributeName);
        }

        public string GetCssValue(IWebElement element, string propertyName)
        {
            return element.GetCssValue(propertyName);
        }

        public string GetProperty(IWebElement element, string propertyName)
        {
            return element.GetProperty(propertyName);
        }

        public void SendKeys(IWebElement element, string text)
        {
            element.SendKeys(text);
        }
        
        public void Submit(IWebElement element)
        {
            element.Submit();
        }

        public Point LocationOnScreenOnceScrolledIntoView(IWebElement element)
        {
            if(!(element is ILocatable))
            {
                throw new Exception("The IWebElement object must implement or wrap an element that implements ILocatable.");
            }

            return (element as ILocatable).LocationOnScreenOnceScrolledIntoView;
        }

        public ICoordinates Coordinates(IWebElement element)
        {
            if (!(element is ILocatable))
            {
                throw new Exception("The IWebElement object must implement or wrap an element that implements ILocatable.");
            }

            return (element as ILocatable).Coordinates;
        }
    }
}
