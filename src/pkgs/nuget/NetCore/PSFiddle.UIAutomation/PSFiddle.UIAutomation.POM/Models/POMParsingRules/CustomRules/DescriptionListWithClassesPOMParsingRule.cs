using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Attributes;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules.CustomRules
{
    [ParsingRuleDefinition]
    public class DescriptionListWithClassesPOMParsingRule : FormatedReferencePOMParsingRule
    {
        public DescriptionListWithClassesPOMParsingRule(POMParsingContext pOMParsingContext) 
            : base(pOMParsingContext, 
                  "ui-readable", 
                  typeof(UIReadablePOMParsedElement),
                   @"(?:DescriptionListWithClassesAndIcons|DescriptionListWithClasses)\([^"",]*?""(.*?)""",
                   ".//*[contains(text(),'DescriptionListWithClassesAndIcons') or contains(text(),'DescriptionListWithClasses')]")
        {
        }

    }
}
