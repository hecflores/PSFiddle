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
    public class UIPagePOMParsingRule : BasicPOMParsingRule
    {
        public UIPagePOMParsingRule(POMParsingContext pOMParsingContext) : base(pOMParsingContext, "ui-page", typeof(UIPagePOMParsedElement))
        {
        }

        public override string ConvertUIElementNameToName(string element)
        {
            return Regex.Replace(element, "[^A-Za-z]", "");
        }
    }
}
