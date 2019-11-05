using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic;
using System.Linq;
using PSFiddle.UIAutomation.POM.Attributes;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules
{
    [ParsingRuleDefinition]
    public class UIMultiplePOMParsingRule : BasicPOMParsingRule
    {
        public UIMultiplePOMParsingRule(POMParsingContext pOMParsingContext) : base(pOMParsingContext, "ui-multiple", typeof(UIMultiplePOMParsedElement))
        {
        }


        public override bool IsMatch(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            if (!base.IsMatch(htmlElement, currentParsedUI))
                return false;

            var alreadyExistingMultiplePOMs = currentParsedUI.Children().Where(b => b is UIMultiplePOMParsedElement);
            if (alreadyExistingMultiplePOMs.Count() == 0)
                return true;

            string element = htmlElement.Attributes[MainTag].Value;
            if (alreadyExistingMultiplePOMs.Where(b => b.UIElementName == element).Count() == 0)
                return true;

            return false;
        }

        
    }
}
