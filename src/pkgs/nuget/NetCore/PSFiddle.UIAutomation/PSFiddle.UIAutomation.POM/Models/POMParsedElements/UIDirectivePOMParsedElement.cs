using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedElements
{
    public class UIDirectivePOMParsedElement : SimpleElementPOMParsedElement
    {
        public UIDirectivePOMParsedElement(POMParsingContext context, HtmlNode htmlElement, POMParsedRawElement parent, POMParsingRule parsableDefinition, string uiElementName, string name, string selector) : base(context, htmlElement, parent, parsableDefinition, uiElementName, name, selector)
        {
            CreationModel.CreateUIElement = false;
            CreationModel.CreateChildSimpleElements = false;
            CreationModel.SimpleElementAttributeList = new Dictionary<string, string>()
            {
                { "Component", UIElementName }
            };
        }
    }
}
