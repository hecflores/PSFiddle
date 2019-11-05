using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Constants;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements.Generic;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedElements
{
    public class UIPagePOMParsedElement : BasicPOMParsedElement
    {
        public UIPagePOMParsedElement(POMParsingContext context, HtmlNode htmlElement, POMParsedRawElement parent, POMParsingRule parsableDefinition, string uiElementName, string name, string selector) : base(context, htmlElement, parent, parsableDefinition, uiElementName, name, selector)
        {
        }

        
        public String Page => Name;
        
        public override string PageObjectXML()
        {

            var page = $"<UIPageObject Name=\"{Name}\" UIElement=\"{Page}\">\r\n";
            var innerContent = "";
            //# if(this.Children().Items().Count -gt 0)
            //# {
            var simpleElements = this.Children().SimpleElementsXML();
            if (!String.IsNullOrEmpty(simpleElements))
            {
                simpleElements = "        " + Regex.Replace(simpleElements, "\r\n", "\r\n    ");
            }
            innerContent = "<UIElements>\r\n";
            var uiElement = this.Children().UIElementXML();
            uiElement = "        " + (Regex.Replace(uiElement, "\r\n", "\r\n        "));
            innerContent += uiElement;
            innerContent += "\r\n";
            innerContent += "</UIElements>";

            innerContent += "<SimpleElements>\r\n";
            innerContent += simpleElements;
            innerContent += "\r\n</SimpleElements>\r\n";

            if (!String.IsNullOrEmpty(innerContent))
            {
                innerContent = "    " + (Regex.Replace(innerContent, "\r\n", "\r\n    "));
            }

            page += innerContent;
            page += "</UIPageObject>\r\n";
            return page;
        }

        public override string SimpleElementXML()
        {
            return String.Empty;
        }

        public override string UIElementXML()
        {
            var page = this.Page;

            var description = page;
            description = Regex.Replace(description, "([A-Za-z])([A-Z])", "$1 $2");
            description = Regex.Replace(description, "(_)([A-Z])", " - $2");
            description = $"{description} (Root)";

            return $"<UIElement Name=\"{page}\" Description=\"{description}\">//*@ui-page='" + (page) + "'</UIElement>";
        }

        public override string BuildInterface()
        {
            var extension = ": IPageBase";
            var methods    = Children().BuildInterfaceMethods($"{{0}}{Environment.NewLine}");
            var interfaces = Children().BuildInterface($"{{0}}{Environment.NewLine}");
            var finalData = $@"
namespace MC.Track.TestSuite.Interfaces.Pages.Generated
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

    public partial interface I{this.Name} {extension}
    {{
{CommonCode.Spacing_CodeClassMethod}IGeneratedPages AllPages();
{CommonCode.Spacing_CodeClassMethod}I{Name} DoThings(Action<I{Name}> callback);
{methods}
    }}
}}
{interfaces}
";
            return finalData;
        

        }
        public override string BuildClass()
        {
            var extension = $": RawPage, I{Name}";
            var methods = Children().BuildClassMethods($"{{0}}{Environment.NewLine}");
            var classes = Children().BuildClass($"{{0}}{Environment.NewLine}");
            var finalData = $@"
namespace MC.Track.TestSuite.Toolkit.Generated.Pages
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
    

    [AthenaRegister(typeof(I{this.Name}), AthenaRegistrationType.Singleton)]
    public partial class {this.Name} {extension}
    {{
{CommonCode.Spacing_CodeClassMethod}public IGeneratedPages AllPages(){{
{CommonCode.Spacing_CodeInnerMethod}return resolver.Resolve<IGeneratedPages>();
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public I{Name} DoThings(Action<I{Name}> callback){{
{CommonCode.Spacing_CodeInnerMethod}WaitFor(() => callback.Invoke(this));
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public override void ValidatePage(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElement({this.Locator.MultiLinedStringLiteral()});
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
