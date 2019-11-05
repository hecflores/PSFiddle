using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules.AngularParsingRules
{
    [ParsingRuleDefinition]
    public class NgIncludeTagVersionPOMParsingRule : NgIncludePOMParsingRule
    {
        public NgIncludeTagVersionPOMParsingRule(POMParsingContext pOMParsingContext) : base(pOMParsingContext)
        {
        }


        public override string DiscoveryXPath => ".//ng-include[@src]";
        
        public override bool IsMatchAlone(HtmlNode htmlElement, POMParsedRawElement currentParsedUI, POMParsedRawElement alreadyMatchedParsedUI)
        {
            if (htmlElement.Attributes == null)
                return false;

            if (! (htmlElement.Name == "ng-include" && htmlElement.Attributes.Contains("src")))
                return false;

            if (String.IsNullOrEmpty(htmlElement.Attributes["src"].Value))
                return false;

            var relativeHtmlFile = htmlElement.Attributes["src"].Value.Replace("'", "");
            var htmlFilePath = currentParsedUI.ResolvePath(relativeHtmlFile);

            if (htmlFilePath == null)
                return false;

            return true;
        }
    }
}
