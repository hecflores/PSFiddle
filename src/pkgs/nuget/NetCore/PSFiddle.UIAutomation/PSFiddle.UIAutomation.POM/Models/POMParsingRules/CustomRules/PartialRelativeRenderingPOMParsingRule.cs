using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Attributes;
using PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules.CustomRules
{
    [ParsingRuleDefinition]
    public class PartialRelativeRenderingPOMParsingRule : BasicPOMParsingRule
    {
        private string _Format = @"\@Html.Partial\(""([^~].*?)""";

        public PartialRelativeRenderingPOMParsingRule(POMParsingContext pOMParsingContext) : base(pOMParsingContext, "ui-readable", typeof(UIReadablePOMParsedElement))
        {
        }

        public override string DiscoveryXPath => ".//*[contains(text(),'Partial')]";

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
                var referencedFile = $"{match.Groups[1].Value}.cshtml";
                var fullPath = currentParsedUI.ResolvePath(referencedFile);
                if (fullPath == null)
                    continue;

                var root = Context.ParseHtmlFile(fullPath);

                if(root.Children().Count() == 0)
                {
                    Context.WriteLine($"{fullPath}{Environment.NewLine}   No elements found at all", ConsoleColor.Yellow);
                    continue;
                }

                if(root.Children().Where(b=>b is UIComponentPOMParsedElement).Count() == 0)
                {
                    Context.WriteLine($"{fullPath}{Environment.NewLine}   No ui-page or ui-component foundelements found at all", ConsoleColor.Yellow);
                    continue;
                }

                var parsedUIComponents = root.Children().Where(b => b is UIComponentPOMParsedElement);
                foreach(UIComponentPOMParsedElement component in parsedUIComponents)
                {
                    var name = component.Component;
                    var defaultReferenceName = component.Name;

                    if (component.HtmlElement.Attributes.Contains("ui-name"))
                        defaultReferenceName = component.HtmlElement.Attributes["ui-name"].Value;

                    if(currentParsedUI.Children().Where(b=>b.Name == defaultReferenceName).Count() != 0)
                    {
                        Context.WriteLine($"{fullPath}{Environment.NewLine}   Showing duplicate references", ConsoleColor.Yellow);
                        continue;
                    }
                    Context.WriteLine("Adding Directive", ConsoleColor.Magenta);
                    var directive = new UIDirectivePOMParsedElement(Context, htmlElement, currentParsedUI, this, name, defaultReferenceName, component.Selector);
                    currentParsedUI.Children().Add(directive);

                }                
            }
            return null;
        }

        public override bool IsMatch(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
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
            if (clonedHtmlElement.InnerText == null)
                return false;

            if (String.IsNullOrEmpty(text))
                return false;

            if (!Regex.Match(text, _Format).Success)
                return false;

            return true;
        }
        
        public override bool IsMatchAfterMatch(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            return IsMatch(htmlElement, currentParsedUI);
        }
        public override void Init()
        {
        }
    }
}
