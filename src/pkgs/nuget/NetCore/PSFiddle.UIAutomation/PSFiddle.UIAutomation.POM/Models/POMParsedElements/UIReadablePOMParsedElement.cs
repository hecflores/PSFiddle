using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedElements
{
    public class UIReadablePOMParsedElement : SimpleElementPOMParsedElement
    {
        public UIReadablePOMParsedElement(POMParsingContext context, HtmlNode htmlElement, POMParsedRawElement parent, POMParsingRule parsableDefinition, string uiElementName, string name, string selector) : base(context, htmlElement, parent, parsableDefinition, uiElementName, name, selector)
        {
            CreationModel.CreateUIElement = true;
            CreationModel.CreateSimpleElement = true;
            CreationModel.CreateChildSimpleElements = true;
            CreationModel.SimpleElementAttributeList = new Dictionary<string, string>()
            {
                {"UIElement",UIElementName },
                {"CanVerify","True" },
                {"CanRead","True" }
            };
        }
    }
}
