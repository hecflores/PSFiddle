using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements.Generic;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedElements
{
    public class UIChartPOMParsedElement : SimpleElementPOMParsedElement
    {
        public UIChartPOMParsedElement(POMParsingContext context, HtmlNode htmlElement, POMParsedRawElement parent, POMParsingRule parsableDefinition, string uiElementName, string name, string selector) : base(context, htmlElement, parent, parsableDefinition, uiElementName, name, selector)
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
                {"IsChart","True" }
            };
        }

        public override string PageObjectXML()
        {
            return String.Empty;
        }

        public override string SimpleElementXML()
        {
            var simpleElements = this.Children().SimpleElementsXML();
            var uiElements = this.Children().UIElementXML();
            uiElements += this.UIElementXML();
            string simpleElement;
            if (!String.IsNullOrEmpty(simpleElements))
            {
                simpleElement = $"<SimpleElement Name=\"{Name}\" UIElement=\"{UIElementName}\" CanVerify= \"True\" CanRead=\"True\" CanEdit=\"True\" CanClick=\"True\" IsChart=\"True\"  >"
                    + "<UIElements>" + (uiElements) + "</UIElements>"
                    + (simpleElements)
                    + "</SimpleElement>";
            }
            else
            {
                simpleElement = $"<SimpleElement Name=\"{Name}\" UIElement=\"{UIElementName}\" CanVerify=\"True\" CanRead=\"True\" CanEdit=\"True\" CanClick=\"True\" IsChart=\"True\"  />";
            }
            return simpleElement;
        }

        public override string UIElementXML()
        {
            var description = UIElementName;
            description = Regex.Replace(description, "([A-Za-z])([A-Z])", "$1 $2");
            description = Regex.Replace(description, "(_)([A-Z])", " - $2");

            return $"<UIElement Name=\"{UIElementName}\" Description=\"{description}\">{Selector}</UIElement>";
        }
    }
}
