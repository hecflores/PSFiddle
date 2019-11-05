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
    public class ContainingDivWithClassesBadVersionPOMParsingRule : FormatedReferencePOMParsingRule
    {
        public ContainingDivWithClassesBadVersionPOMParsingRule(POMParsingContext pOMParsingContext) 
            : base(pOMParsingContext, 
                   "ui-element", 
                   typeof(UIElementPOMParsedElement), 
                   @"(?:ContainingDivWithClassesWithUIElement)\(.*?,.*?""(.*?)""",
                   ".//*[contains(text(),'ContainingDivWithClassesWithUIElement')]")
        {
        }

    }
}
