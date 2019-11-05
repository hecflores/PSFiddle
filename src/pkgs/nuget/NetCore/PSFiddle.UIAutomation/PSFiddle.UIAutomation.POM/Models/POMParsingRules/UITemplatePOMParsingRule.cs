using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Attributes;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules
{
    [ParsingRuleDefinition]
    public class UITemplatePOMParsingRule : BasicPOMParsingRule
    {
        public UITemplatePOMParsingRule(POMParsingContext pOMParsingContext) : base(pOMParsingContext, "ui-template", typeof(UIComponentPOMParsedElement))
        {
        }
        public override string XPathDefinition(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            string element = htmlElement.Attributes[MainTag].Value;
            return $"//*[@ui-template='{element}']";
        }
        public override string ConvertUIElementNameToName(string element)
        {
            return Regex.Replace(element, "^.*?([A-Za-z0-9]+)$", "$1");
        }
        public override string GetElementName(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            var elementName = base.GetElementName(htmlElement, currentParsedUI);
            return elementName;
        }

        public override void OnAfterNewElement(POMParsedRawElement currentParsedUI, POMParsedRawElement parsedRawElement)
        {
            //if(! (currentParsedUI is UIRootPOMParsedElement))
            //    currentParsedUI.Root.Children().Add(parsedRawElement);
        }
    }
}
