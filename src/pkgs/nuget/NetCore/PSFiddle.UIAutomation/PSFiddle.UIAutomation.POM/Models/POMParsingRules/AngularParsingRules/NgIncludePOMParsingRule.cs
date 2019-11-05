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
    public class NgIncludePOMParsingRule : UIDirectivePOMParsingRule
    {
        public NgIncludePOMParsingRule(POMParsingContext pOMParsingContext) : base(pOMParsingContext)
        {
        }


        public override string DiscoveryXPath => ".//*[@ng-include and @src]";
        public override POMParsedRawElement CreateParsedUI(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            var relativeHtmlFile = htmlElement.Attributes["src"].Value.Replace("'", "");
            var htmlFilePath = currentParsedUI.ResolvePath(relativeHtmlFile);

            if (htmlFilePath == null)
                throw new Exception($"Unable to resolve file {relativeHtmlFile}");

            var root = Context.ParseHtmlFile(htmlFilePath);

            if (root.Children().Count() == 0)
            {
                Context.WriteLine($"{htmlFilePath}{Environment.NewLine}   No elements found at all", ConsoleColor.Yellow);
                return null;
            }

            if (root.Children().OfType<UIComponentPOMParsedElement>().Count() == 0)
            {
                Context.WriteLine($"{htmlFilePath}{Environment.NewLine}   No ui-component found", ConsoleColor.Yellow);
                return null;
            }

            var components = root.Children().OfType<UIComponentPOMParsedElement>();
            foreach (var component in components)
            {
                var name = component.Component;
                var defaultReferenceName = component.Name;
                if (currentParsedUI.Children().Where(b => b.Name == defaultReferenceName).Count() > 0)
                {
                    Context.WriteLine($"{htmlFilePath}{Environment.NewLine}   Showing duplicate references", ConsoleColor.Yellow);
                    return null;
                }

                var directive = new UIDirectivePOMParsedElement(Context, htmlElement, currentParsedUI, this, name, defaultReferenceName, component.Selector);
                currentParsedUI.Children().Add(directive);
            }


            return null;
        }
        public override bool IsMatch(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            return false;
        }
        public override bool IsMatchAlone(HtmlNode htmlElement, POMParsedRawElement currentParsedUI, POMParsedRawElement alreadyMatchedParsedUI)
        {
            if (htmlElement.Attributes == null)
                return false;

            if (! (htmlElement.Attributes.Contains("ng-include") && htmlElement.Attributes.Contains("src")))
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
