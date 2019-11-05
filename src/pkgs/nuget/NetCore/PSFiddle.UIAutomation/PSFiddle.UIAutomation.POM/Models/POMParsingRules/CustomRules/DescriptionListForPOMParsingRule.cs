using PSFiddle.UIAutomation.POM.Attributes;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules.CustomRules
{
    [ParsingRuleDefinition]
    public class DescriptionListForPOMParsingRule : FormatedReferencePOMParsingRule
    {
        public DescriptionListForPOMParsingRule(POMParsingContext pOMParsingContext) :
            base(pOMParsingContext,
                "ui-readable",
                typeof(UIReadablePOMParsedElement),
                 @"(?:DescriptionListFor)\(.*?""(.*?)""",
                ".//*[contains(text(),'DescriptionListFor')]")
        {
        }
    }
}
