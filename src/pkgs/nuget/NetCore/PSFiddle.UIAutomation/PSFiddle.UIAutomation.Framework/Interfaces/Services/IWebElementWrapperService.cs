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

namespace MC.Track.TestSuite.Interfaces.Services
{
    /// <summary>
    /// This service is meant to be a proxy to the web element class. Its a way to add Aspect Oriented Programming to this 3rd part library 
    /// </summary>
    public interface IWebElementWrapperService
    {
        /// <summary>
        /// Get the tag name from the element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        string TagName(IWebElement element);

        /// <summary>
        /// Get text from the element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        string Text(IWebElement element);

        /// <summary>
        /// See if element is enabled
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool Enabled(IWebElement element);

        /// <summary>
        /// See if element is selected
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool Selected(IWebElement element);

        /// <summary>
        /// Get where the element is located
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        Point Location(IWebElement element);

        /// <summary>
        /// Get the size of the element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        Size Size(IWebElement element);

        /// <summary>
        /// Get rather or not the element is displayed
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool Displayed(IWebElement element);

        /// <summary>
        /// Clear the element
        /// </summary>
        /// <param name="element"></param>
        void Clear(IWebElement element);

        /// <summary>
        /// Click the element
        /// </summary>
        /// <param name="element"></param>
        void Click(IWebElement element);

        /// <summary>
        /// Find an element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        IWebElement FindElement(IWebElement element, By by);

        /// <summary>
        /// Find all elements
        /// </summary>
        /// <param name="element"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        ReadOnlyCollection<IWebElement> FindElements(IWebElement element, By by);

        /// <summary>
        /// Get attributes
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        string GetAttribute(IWebElement element, string attributeName);

        /// <summary>
        /// Get Css Value
        /// </summary>
        /// <param name="element"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        string GetCssValue(IWebElement element, string propertyName);

        /// <summary>
        /// Get property
        /// </summary>
        /// <param name="element"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        string GetProperty(IWebElement element, string propertyName);

        /// <summary>
        /// Send some keys
        /// </summary>
        /// <param name="element"></param>
        /// <param name="text"></param>
        void SendKeys(IWebElement element, string text);

        /// <summary>
        /// Submit this element
        /// </summary>
        /// <param name="element"></param>
        void Submit(IWebElement element);

        /// <summary>
        /// Locations the on screen once scrolled into view.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        Point LocationOnScreenOnceScrolledIntoView(IWebElement element);

        /// <summary>
        /// Coordinateses the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        ICoordinates Coordinates(IWebElement element);
    }
}
