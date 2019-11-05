using PSFiddle.UIAutomation.POM.Attributes;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules
{
    [ParsingRuleDefinition]
    public class UIClickablePOMParsingRule : BasicPOMParsingRule
    {
        public UIClickablePOMParsingRule(POMParsingContext pOMParsingContext) : base(pOMParsingContext, "ui-clickable", typeof(UIClickablePOMParsedElement))
        {
        }
    }
}
