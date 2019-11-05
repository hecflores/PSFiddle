using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Attributes;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules
{
    [ParsingRuleDefinition]
    public class UIElementPOMParsingRule : POMParsingRule
    {
        public UIElementPOMParsingRule(POMParsingContext pOMParsingContext) : base(pOMParsingContext)
        {
        }

        public override string DiscoveryXPath => ".//*[@ui-element]";

        public override string XPathDefinition(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            string element = htmlElement.Attributes["ui-element"].Value;
            return $"//*[@ui-element='{element}']";
        }

        public override POMParsedRawElement CreateParsedUI(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            string element = htmlElement.Attributes["ui-element"].Value;
            var name = Regex.Replace(element, "^.*?([A-Za-z0-9]+)$", "$1");

            if (currentParsedUI is UIMultiplePOMParsedElement)
            {
                return new UIQuerablePOMParsedElement(this.Context, htmlElement, currentParsedUI, this, element, name, XPathDefinition(htmlElement, currentParsedUI));
            }

            return new UIElementPOMParsedElement(this.Context, htmlElement, currentParsedUI, this, element, name, XPathDefinition(htmlElement, currentParsedUI));
        }

        public override bool IsMatch(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            if (!htmlElement.HasAttributes)
            {
                return false;
            }
            return htmlElement.Attributes.Contains("ui-element");
        }

        public override bool IsMatchAlone(HtmlNode htmlElement, POMParsedRawElement currentParsedUI, POMParsedRawElement alreadyMatchedParsedUI)
        {
            if (!htmlElement.HasAttributes)
            {
                return false;
            }

            var canDoAfterTheFact = false;

            // To Account for already existing bug in old parsing system. File, LinkingSelectionScreen
            // TODO - Finalize these fixes
            canDoAfterTheFact |= htmlElement.Attributes.Contains("ui-element") && htmlElement.Attributes.Contains("ui-directive");

            return canDoAfterTheFact;
        }


    }
}
