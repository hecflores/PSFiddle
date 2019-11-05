using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class POMParsingRule
    {
        private readonly POMParsingContext _context;
        public POMParsingContext Context => _context;


        public POMParsingRule(POMParsingContext pOMParsingContext)
        {
            this._context = pOMParsingContext;
        }

        /// <summary>
        /// xes the path definition.
        /// </summary>
        /// <param name="htmlElement">The HTML element.</param>
        /// <param name="currentParsedUI">The current parsed UI.</param>
        /// <returns></returns>
        public abstract String XPathDefinition(HtmlNode htmlElement, POMParsedRawElement currentParsedUI);

        /// <summary>
        /// Gets the discovery x path.
        /// </summary>
        /// <value>
        /// The discovery x path.
        /// </value>
        public abstract String DiscoveryXPath { get; }

        /// <summary>
        /// Determines whether the specified HTML element is match.
        /// </summary>
        /// <param name="htmlElement">The HTML element.</param>
        /// <param name="currentParsedUI">The current parsed UI.</param>
        /// <returns>
        ///   <c>true</c> if the specified HTML element is match; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsMatch(HtmlNode htmlElement, POMParsedRawElement currentParsedUI);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlElement"></param>
        /// <param name="currentParsedUI"></param>
        /// <returns></returns>
        public virtual bool IsMatchPreParse(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            return false;
        }
        

        /// <summary>
        /// Determines whether the specified HTML element is match.
        /// </summary>
        /// <param name="htmlElement">The HTML element.</param>
        /// <param name="currentParsedUI">The current parsed UI.</param>
        /// <returns>
        ///   <c>true</c> if the specified HTML element is match; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsMatchAlone(HtmlNode htmlElement, POMParsedRawElement currentParsedUI, POMParsedRawElement alreadyMatchedParsedUI)
        {
            return false;
        }
        /// <summary>
        /// Determines whether [is match after match] [the specified HTML element].
        /// </summary>
        /// <param name="htmlElement">The HTML element.</param>
        /// <param name="currentParsedUI">The current parsed UI.</param>
        /// <returns>
        ///   <c>true</c> if [is match after match] [the specified HTML element]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsMatchAfterMatch(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            return false;
        }

        /// <summary>
        /// Adds the new parsed element.
        /// </summary>
        /// <param name="htmlElement">The HTML element.</param>
        /// <param name="currentParsedUI">The current parsed UI.</param>
        /// <param name="newParsedElement">The new parsed element.</param>
        public virtual POMParsedRawElement AddNewParsedElement(HtmlNode htmlElement, POMParsedRawElement currentParsedUI, POMParsedRawElement matchedParsedUI, POMParsedRawElement newParsedElement)
        {
            if (currentParsedUI != null)
                currentParsedUI.Children().Add(newParsedElement);

            return newParsedElement;
        }


        /// <summary>
        /// Creates the parsed UI.
        /// </summary>
        /// <param name="htmlElement">The HTML element.</param>
        /// <param name="currentParsedUI">The current parsed UI.</param>
        /// <returns></returns>
        public abstract POMParsedRawElement CreateParsedUI(HtmlNode htmlElement, POMParsedRawElement currentParsedUI);


        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public virtual int Priority() => 0; 

      

        /// <summary>
        /// Posts the parse.
        /// </summary>
        /// <param name="htmlElement">The HTML element.</param>
        /// <param name="currentParsedUI">The current parsed UI.</param>
        /// <param name="newParsedUI">The new parsed UI.</param>
        /// <returns></returns>
        public virtual POMParsedRawElement PostParse(HtmlNode htmlElement, POMParsedRawElement currentParsedUI, POMParsedRawElement newParsedUI)
        {
            return newParsedUI;
        }

        /// <summary>
        /// Parses the specified HTML element.
        /// </summary>
        /// <param name="htmlElement">The HTML element.</param>
        /// <param name="currentParsedUI">The current parsed UI.</param>
        /// <returns></returns>
        public virtual POMParsedRawElement Parse(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            if (!this.IsMatch(htmlElement, currentParsedUI))
            {
                return null;
            }
            return this.CreateParsedUI(htmlElement, currentParsedUI);
        }
        /// <summary>
        /// Parses the specified HTML element.
        /// </summary>
        /// <param name="htmlElement">The HTML element.</param>
        /// <param name="currentParsedUI">The current parsed UI.</param>
        /// <returns></returns>
        public virtual POMParsedRawElement PreParse(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            if (!this.IsMatchPreParse(htmlElement, currentParsedUI))
            {
                return null;
            }
            return this.CreateParsedUI(htmlElement, currentParsedUI);
        }
        /// <summary>
        /// Parses the specified HTML element.
        /// </summary>
        /// <param name="htmlElement">The HTML element.</param>
        /// <param name="currentParsedUI">The current parsed UI.</param>
        /// <returns></returns>
        public virtual POMParsedRawElement ParseAlone(HtmlNode htmlElement, POMParsedRawElement currentParsedUI, POMParsedRawElement alreadyMatchedParsedUI)
        {
            if (!this.IsMatchAlone(htmlElement, currentParsedUI, alreadyMatchedParsedUI))
            {
                return null;
            }
            return this.CreateParsedUI(htmlElement, currentParsedUI);
        }
        /// <summary>
        /// Parses the after match.
        /// </summary>
        /// <param name="htmlElement">The HTML element.</param>
        /// <param name="currentParsedUI">The current parsed UI.</param>
        /// <returns></returns>
        public virtual POMParsedRawElement ParseAfterMatch(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            if (!this.IsMatchAfterMatch(htmlElement, currentParsedUI))
            {
                return null;
            }
            return this.CreateParsedUI(htmlElement, currentParsedUI);
        }

    }
}
