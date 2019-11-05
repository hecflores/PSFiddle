using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PSFiddle.UIAutomation.POM.Models.POMParsingRules.Generic
{
    public class BasicPOMParsingRule : POMParsingRule
    {
        protected string MainTag;
        protected Type CreationPOMParsedElementType;

        public BasicPOMParsingRule(POMParsingContext pOMParsingContext, String mainTag, Type pomParsedRawElementType) : base(pOMParsingContext)
        {
            this.MainTag = mainTag;
            this.CreationPOMParsedElementType = pomParsedRawElementType;
        }

        public override string DiscoveryXPath => $".//*[@{MainTag}]";

        public override string XPathDefinition(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            string element = htmlElement.Attributes[MainTag].Value;
            return $"//*[@{MainTag}='{element}']";
        }
        public virtual String ConvertUIElementNameToName(String element, HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            return ConvertUIElementNameToName(element);
        }
        public virtual String ConvertUIElementNameToName(String element)
        {
            return Regex.Replace(element, "^.*?([A-Za-z0-9]+)$", "$1");
        }
        public virtual String GetElementName(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            return htmlElement.Attributes[MainTag].Value;
        }
        public virtual void OnAfterNewElement(POMParsedRawElement currentParsedUI, POMParsedRawElement parsedRawElement)
        {
            return;
        }
        public override POMParsedRawElement CreateParsedUI(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            var element = GetElementName(htmlElement, currentParsedUI);
            var name = ConvertUIElementNameToName(element, htmlElement, currentParsedUI);

            Context.WriteLine($"Creating Parsed Type '{CreationPOMParsedElementType}'", ConsoleColor.Cyan);
            var parsedRawElement = Activator.CreateInstance(CreationPOMParsedElementType, this.Context, htmlElement, currentParsedUI, this, element, name, XPathDefinition(htmlElement, currentParsedUI)) as POMParsedRawElement;

            OnAfterNewElement(currentParsedUI, parsedRawElement);

            return parsedRawElement;
        }

        public override bool IsMatch(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            if (!htmlElement.HasAttributes)
            {
                return false;
            }
            return htmlElement.Attributes.Contains(MainTag);
        }

        public override bool IsMatchAlone(HtmlNode htmlElement, POMParsedRawElement currentParsedUI, POMParsedRawElement alreadyMatchedParsedUI)
        {
            return false;
        }
    }
}
