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
    public class UIChartPOMParsingRule : BasicPOMParsingRule
    {
        public UIChartPOMParsingRule(POMParsingContext pOMParsingContext) : base(pOMParsingContext, "ui-chart", typeof(UIChartPOMParsedElement))
        {
        }
    }
}
