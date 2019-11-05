using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Attributes;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules
{
    [ParsingRuleDefinition]
    public class UILayoutPOMParsingRule : BasicPOMParsingRule
    {
        public UILayoutPOMParsingRule(POMParsingContext pOMParsingContext) : base(pOMParsingContext, "ui-layout", typeof(UILayoutReferencePOMParsedElement))
        {
        }

        public override bool IsMatch(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            return false;
        }
        public override bool IsMatchPreParse(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            return base.IsMatch(htmlElement, currentParsedUI);
        }

    }
}
