using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedElements
{
    public class UIEditablePOMParsedElement : SimpleElementPOMParsedElement
    {
        public UIEditablePOMParsedElement(POMParsingContext context, HtmlNode htmlElement, POMParsedRawElement parent, POMParsingRule parsableDefinition, string uiElementName, string name, string selector) : base(context, htmlElement, parent, parsableDefinition, uiElementName, name, selector)
        {
            CreationModel.CreateChildSimpleElements = true;
            CreationModel.CreateSimpleElement = true;
            CreationModel.CreateUIElement = true;
            CreationModel.SimpleElementAttributeList = new Dictionary<string, string>()
            {
                {"UIElement" , UIElementName },
                {"CanVerify","True" },
                {"CanEdit","True" }
            };
        }
    }
}
