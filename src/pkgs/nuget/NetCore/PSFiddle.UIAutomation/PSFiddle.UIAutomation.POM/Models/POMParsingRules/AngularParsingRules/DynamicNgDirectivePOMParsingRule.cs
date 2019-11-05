using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules.AngularParsingRules
{
    // We dont want to automaticly create a default rule since this will be added dynamicly or when needed.
    public class DynamicNgDirectivePOMParsingRule : UIDirectivePOMParsingRule
    {
        private readonly string directiveName;
        private readonly string componentName;
        private readonly string directiveReferenceName;

        public DynamicNgDirectivePOMParsingRule(POMParsingContext pOMParsingContext, String directiveName, String componentName) : base(pOMParsingContext)
        {
            this.directiveName = directiveName;
            this.componentName = componentName;
            this.directiveReferenceName = Regex.Replace(this.directiveName, "([A-Z])", "-$1").ToLower();
        }
        
        public override string DiscoveryXPath => $".//{DirectiveReferenceName} | .//*[@{DirectiveReferenceName}]";

        public string DirectiveReferenceName => directiveReferenceName;

        public string ComponentName => componentName;

        public string DirectiveName => directiveName;

        public override string XPathDefinition(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            return $"//*[@ui-component='{ComponentName}']";
        }
        public override POMParsedRawElement AddNewParsedElement(HtmlNode htmlElement, POMParsedRawElement currentParsedUI, POMParsedRawElement matchedParsedUI, POMParsedRawElement newParsedElement)
        {
            if(matchedParsedUI != null)
            {
                currentParsedUI.Children().Insert(currentParsedUI.Children().Count() - 1, newParsedElement);
                return matchedParsedUI;
            }
            return base.AddNewParsedElement(htmlElement, currentParsedUI, matchedParsedUI, newParsedElement);
        }
        public override bool IsMatchAlone(HtmlNode htmlElement, POMParsedRawElement currentParsedUI, POMParsedRawElement alreadyMatchedParsedUI)
        {
            if (currentParsedUI.Children().OfType<UIDirectivePOMParsedElement>().Where(b=>b.Name == ComponentName).Count() != 0)
                return false;

            if (htmlElement.Name == null)
                return false;

            if (htmlElement.Attributes == null)
                return false;

            if (htmlElement.Name.Equals(DirectiveReferenceName, StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (htmlElement.Attributes.Contains(DirectiveReferenceName))
                return true;

            return false;
        }
        public override bool IsMatch(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            return false;
        }
        public override string GetElementName(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            return componentName;
        }

        public override string ConvertUIElementNameToName(string element, HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            if (htmlElement.Attributes.Contains("ui-name"))
                return htmlElement.Attributes["ui-name"].Value;

            return base.ConvertUIElementNameToName(element, htmlElement, currentParsedUI);
        }
    }
}
