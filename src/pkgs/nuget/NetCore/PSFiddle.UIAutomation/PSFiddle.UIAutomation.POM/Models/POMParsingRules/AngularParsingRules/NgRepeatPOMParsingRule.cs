using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Attributes;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules.AngularParsingRules
{
    [ParsingRuleDefinition]
    public class NgRepeatPOMParsingRule : UIMultiplePOMParsingRule
    {
        public NgRepeatPOMParsingRule(POMParsingContext pOMParsingContext) : base(pOMParsingContext)
        {
            MainTag = "ui-element";
        }
        public override int Priority() => 1;
        public override string DiscoveryXPath => $".//*[@{MainTag} and @ng-repeat]";

        public override bool IsMatch(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            if (!htmlElement.Attributes.Contains("ng-repeat"))
                return false;

            return base.IsMatch(htmlElement, currentParsedUI);

        }
    }
}
