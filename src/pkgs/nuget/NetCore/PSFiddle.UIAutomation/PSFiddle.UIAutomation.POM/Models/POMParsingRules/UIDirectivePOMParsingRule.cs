using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Attributes;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules
{
    [ParsingRuleDefinition]
    public class UIDirectivePOMParsingRule : BasicPOMParsingRule
    {
        public UIDirectivePOMParsingRule(POMParsingContext pOMParsingContext) : base(pOMParsingContext, "ui-directive", typeof(UIDirectivePOMParsedElement))
        {
        }
        public override string XPathDefinition(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            string element = htmlElement.Attributes[MainTag].Value;
            return $"//*[@ui-component='{element}']";
        }
    }
}
