using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Constants;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements.Generic;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedElements
{
    public class UIComponentPOMParsedElement : BasicPOMParsedElement
    {
        public UIComponentPOMParsedElement(POMParsingContext context, HtmlNode htmlElement, POMParsedRawElement parent, POMParsingRule parsableDefinition, string uiElementName, string name, string selector) : base(context, htmlElement, parent, parsableDefinition, uiElementName, name, selector)
        {
            CreationModel.SimpleElementAttributeList = new Dictionary<string, string>()
            {
                { "Component", Component }
            };
        }

        public String Component => Regex.Replace(UIElementName, "[^A-Za-z0-9]", "");


        public override string PageObjectXML()
        {
            var name = Component;

            var page = $"<UIComponentObject Name=\"{name}\" UIElement=\"{UIElementName}\">\r\n";
            var innerContent = "";
            //#if (this.Children().Items().Count -gt 0)
            //#{
            var simpleElements = this.Children().SimpleElementsXML();
            if (!String.IsNullOrEmpty(simpleElements))
            {
                simpleElements = "    " + (Regex.Replace(simpleElements, "\r\n", "\r\n    "));
            }

            innerContent = "<UIElements>\r\n";
            var uiElement = this.UIElementXML();
            uiElement += this.Children().UIElementXML();
            innerContent += uiElement;
            innerContent += "\r\n";
            innerContent += "</UIElements>";

            innerContent += "<SimpleElements>";
            innerContent += simpleElements;
            innerContent += "\r\n</SimpleElements>";
            //# }
            if (!String.IsNullOrEmpty(innerContent))
            {
                innerContent = "    " + (Regex.Replace(innerContent, "\r\n", "\r\n    ")) + "\r\n";
            }
            page += innerContent;
            page += "</UIComponentObject>\r\n";
            return page;
        }

        public override string SimpleElementXML()
        {
            var component = this.Component;
            var name      = this.Name;
            

            var simpleElement = $"\r\n<SimpleElement Name=\"{name}\" Component=\"{component}\"/>";
            return simpleElement;
        }

        public override string UIElementXML()
        {
            var component = this.UIElementName;

            var description = component;
            description = Regex.Replace(description, "([A-Za-z])([A-Z])", "$1 $2");
            description = Regex.Replace(description, "(_)([A-Z])", " - $2");
            description = $"{description} (Root)";

            return $"<UIElement Name=\"{component}\" Description=\"{description}\">{Selector}</UIElement>";
        }


        

        public override string BuildInterface()
        {
            var extension = ": IPageBase";
            var methods = Children().BuildInterfaceMethods($"{{0}}{Environment.NewLine}");
            var interfaces = Children().BuildInterface($"{{0}}{Environment.NewLine}");
            var finalData = $@"
namespace MC.Track.TestSuite.Interfaces.Pages.Generated.Partials
{{
    using MC.Track.TestSuite.Interfaces.Pages.Generated.Shared;
    using MC.Track.TestSuite.Interfaces.Pages.Generated.Partials;
    using MC.Track.TestSuite.Interfaces.Pages.Generated;
    using MC.Track.TestSuite.Interfaces.Pages.Shared;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public partial interface I{this.Component} {extension}
    {{
{CommonCode.Spacing_CodeClassMethod}IGeneratedPages AllPages();
{CommonCode.Spacing_CodeClassMethod}I{Component} DoThings(Action<I{Component}> callback);
{methods}
    }}
}}
{interfaces}
";
            return finalData;


        }
        public override string BuildClass()
        {
            var extension = $": ScopedPageUI, I{Component}";
            var methods = Children().BuildClassMethods($"{{0}}{Environment.NewLine}");
            var classes = Children().BuildClass($"{{0}}{Environment.NewLine}");
            var finalData = $@"
namespace MC.Track.TestSuite.Toolkit.Pages.Generated.Partials
{{
    using MC.Track.TestSuite.Interfaces.Pages.Generated.Shared;
    using MC.Track.TestSuite.Model.PageModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MC.Track.TestSuite.Toolkit.Pages.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using MC.Track.TestSuite.Interfaces.Services;
    using MC.Track.TestSuite.Interfaces.Pages.Generated;
    using MC.Track.TestSuite.Interfaces.Pages.Shared;
    using MC.Track.TestSuite.Interfaces.Attributes;
    using MC.Track.TestSuite.Model.Enums;
    using MC.Track.TestSuite.Interfaces.Driver;
    using MC.Track.TestSuite.Toolkit.Pages.Generated.Partials;
    using MC.Track.TestSuite.Interfaces.Pages.Generated.Partials;
    

    public partial class {this.Component} {extension}
    {{
{CommonCode.Spacing_CodeClassMethod}public IGeneratedPages AllPages(){{
{CommonCode.Spacing_CodeInnerMethod}return resolver.Resolve<IGeneratedPages>();
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public I{Component} DoThings(Action<I{Component}> callback){{
{CommonCode.Spacing_CodeInnerMethod}WaitFor(() => callback.Invoke(this));
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public override void ValidatePage(){{
{CommonCode.Spacing_CodeInnerMethod}//Unkown action here...
{CommonCode.Spacing_CodeClassMethod}}}
{methods}
    }}
}}
{classes}
";
            return finalData;
        }
    }
}
