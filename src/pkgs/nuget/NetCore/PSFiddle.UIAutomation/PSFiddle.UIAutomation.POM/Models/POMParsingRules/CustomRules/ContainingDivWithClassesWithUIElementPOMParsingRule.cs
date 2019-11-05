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
    public class ContainingDivWithClassesWithUIElement : FormatedReferencePOMParsingRule
    {
        public ContainingDivWithClassesWithUIElement(POMParsingContext pOMParsingContext) : 
            base(pOMParsingContext, 
                "ui-element", 
                typeof(UIElementPOMParsedElement),
                @"(?:ContainingDivWithClasses|ContainingDivWithClassesWithUIElement)\([^"",]*?""(.*?)""",
                ".//*[contains(text(),'ContainingDivWithClasses') or contains(text(),'ContainingDivWithClassesWithUIElement')]")
        {
        }

    }
}
