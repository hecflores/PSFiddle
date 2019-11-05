using PSFiddle.UIAutomation.POM.Attributes;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules
{
    [ParsingRuleDefinition]
    public class UIReadablePOMParsingRule : BasicPOMParsingRule
    {
        public UIReadablePOMParsingRule(POMParsingContext pOMParsingContext) : base(pOMParsingContext, "ui-readable", typeof(UIReadablePOMParsedElement))
        {
        }
    }
}
