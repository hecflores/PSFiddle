using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedElements.Generic
{
    public class SimpleElementPOMParsedElement : BasicPOMParsedElement
    {
        public SimpleElementPOMParsedElement(POMParsingContext context, HtmlNode htmlElement, POMParsedRawElement parent, POMParsingRule parsableDefinition, string uiElementName, string name, string selector) : base(context, htmlElement, parent, parsableDefinition, uiElementName, name, selector)
        {
        }
    }
}
