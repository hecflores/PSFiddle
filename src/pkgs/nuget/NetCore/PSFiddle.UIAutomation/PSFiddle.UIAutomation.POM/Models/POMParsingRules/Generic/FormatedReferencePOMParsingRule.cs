using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic
{
    public class FormatedReferencePOMParsingRule : BasicPOMParsingRule
    {
        protected string _Format;
        private string _Discovery;
        public FormatedReferencePOMParsingRule(POMParsingContext pOMParsingContext, String SelectorType, Type CreationType, String Format, String Discovery) : base(pOMParsingContext, SelectorType, CreationType)
        {
            _Format = Format;
            _Discovery = Discovery;
        }

        public override string DiscoveryXPath => _Discovery;

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

                var constructor = CreationPOMParsedElementType.GetConstructor(new Type[] { typeof(POMParsingContext), typeof(HtmlNode), typeof(POMParsedRawElement), typeof(POMParsingRule), typeof(string), typeof(string), typeof(string) });
                var parsedRawElement = constructor.Invoke(new Object[] { this.Context, htmlElement, currentParsedUI, this, uiElement, name, $"//*[@{MainTag}='{uiElement}']" }) as POMParsedRawElement;
                currentParsedUI.Children().Add(parsedRawElement);
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
        public override int Priority()
        {
            return -2;
        }
        public override void Init()
        {
        }
    }
}
