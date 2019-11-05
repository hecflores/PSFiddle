using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Attributes;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules.CustomRules
{
    [ParsingRuleDefinition]
    public class DescriptionListWithClassesAndChanges : FormatedReferencePOMParsingRule
    {
        public DescriptionListWithClassesAndChanges(POMParsingContext pOMParsingContext) :
            base(pOMParsingContext,
                "ui-readable",
                typeof(UIReadablePOMParsedElement),
                 @"(?:DescriptionListWithClassesAndChanges)\([^"",]*?""(.*?)""",
                ".//*[contains(text(),'DescriptionListWithClassesAndChanges')]")
        {
        }

        public override POMParsedRawElement CreateParsedUI(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            var clonedHtmlElement = htmlElement.Clone();
            var nextDescoveries = clonedHtmlElement.SelectNodes(Context.ParsingRules.AnyRuleSelector);

            if (nextDescoveries != null)
            {
                foreach (var delete in nextDescoveries)
                {
                    delete.Remove();
                }
            }

            var text = clonedHtmlElement.InnerText;

            var matches = Regex.Matches(text, _Format);
            foreach (Match match in matches)
            {
                var uiElement = match.Groups[1].Value;
                var name = Regex.Replace(uiElement, @"^.*?([A-Za-z0-9]+)$", "$1");

                var newElement = new UIReadablePOMParsedElement(Context, htmlElement, currentParsedUI, this, uiElement, name, $@"//*[@ui-readable='{uiElement}']");
                var changesComponent = new UIDirectivePOMParsedElement(Context, htmlElement, newElement, this, "ChangesOldAndNewValue", "Changes",$@"//*[@ui-component='{"ChangesOldAndNewValue"}']");
                newElement.Children().Add(changesComponent);
                currentParsedUI.Children().Add(newElement);
            }
            return null;
        }
    }
}
