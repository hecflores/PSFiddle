using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedElements
{
    public class UIMultiplePOMParsedElement : SimpleElementPOMParsedElement
    {
        public UIMultiplePOMParsedElement(POMParsingContext context, HtmlNode htmlElement, POMParsedRawElement parent, POMParsingRule parsableDefinition, string element, string name, string selector)
            :base(context, htmlElement, parent, parsableDefinition, element, name, selector)
        {
            CreationModel.CreateChildSimpleElements = true;
            CreationModel.CreateSimpleElement = true;
            CreationModel.CreateUIElement = true;
            CreationModel.SimpleElementAttributeList = new Dictionary<string, string>()
            {
                {"UIElement" , UIElementName },
                {"CanVerify","True" },
                {"CanEdit","True" },
                {"CanRead","True" },
                {"CanClick","True" },
                {"Multiple","True" }
            };
        }
    }
}
