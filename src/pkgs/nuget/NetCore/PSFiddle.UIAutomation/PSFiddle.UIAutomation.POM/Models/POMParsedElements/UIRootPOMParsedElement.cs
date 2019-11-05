using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedElements
{
    public class UIRootPOMParsedElement : POMParsedRawElement
    {
        private readonly POMParsedSource source;

        public UIRootPOMParsedElement(POMParsingContext context, POMParsedSource source) : base(context, null, null, null, null, null, null)
        {
            this.source = source;
        }

        public POMParsedSource Source => source;

        public override string PageObjectXML()
        {
            string uiElements = "<UIPageObjects>\r\n";

            string uiElement = this.Children().PageObjectsXML();
            uiElement = Regex.Replace(uiElement, "\r\n", "\r\n        ");
            uiElements += uiElement;
            uiElements += "\r\n";
            uiElements += "</UIPageObjects>";

            return uiElements;
        }
        public override string BuildInterfaceFile()
        {
            var code = @"";

            code += this.Children().BuildInterface();

            var codeLines = "";
            var pages = this.Children().OfType<UIPagePOMParsedElement>();
            foreach (var page in pages)
                codeLines += $"        I{page.Name} {page.Name} {{get;}}{Environment.NewLine}";

            var finalData = $@"

namespace MC.Track.TestSuite.Interfaces.Pages.Generated.Shared
{{
    using MC.Track.TestSuite.Interfaces.Pages.Generated.Partials;
    using MC.Track.TestSuite.Interfaces.Pages.Generated;
    using MC.Track.TestSuite.Interfaces.Pages.Shared;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public partial interface IGeneratedPages
    {{
{codeLines}
    }}
}}
";
            if(pages.Count() > 0)
                code += finalData;

            code = $@"
{code}";

            return code;
        }
        
        public override string BuildClassFile()
        {
            var code = @"
";

            code += this.Children().BuildClass();

            var codeLines = "";
            var pages = this.Children().OfType<UIPagePOMParsedElement>();
            foreach (var page in pages)
                codeLines += $"        public I{page.Name} {page.Name} => CreatePage<I{page.Name}>(); {Environment.NewLine}";

            var finalData = $@"

namespace MC.Track.TestSuite.Interfaces.Pages.Generated.Shared
{{
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
    

    public partial class GeneratedPages : IGeneratedPages
    {{
{codeLines}
    }}
}}
";
            if (pages.Count() > 0)
                code += finalData;

            code = $@"
{code}";

            return code;
        }
        public override string SimpleElementXML()
        {
            return String.Empty;
        }

        public override string UIElementXML()
        {
            return String.Empty;
        }

        public void CreateAutomationMasterFile()
        {
            var HtmlFile = Root.Source.Identifier();

            var root = this;
            if (root.Children().Count() > 0)
            {
                if (root.Children().Where(b => b is UIPagePOMParsedElement || b is UIComponentPOMParsedElement).Count() == 0)
                {
                    Context.WriteLine($"{HtmlFile}{Environment.NewLine}   No ui-page or ui-component found", ConsoleColor.Red);
                    return;
                }

                var automationMasterFile = Path.ChangeExtension(HtmlFile, ".automationmaster.generated");
                Context.WriteLine($"Generating {automationMasterFile}", ConsoleColor.White);
                var automationMasterContent = $@"<?xml version=""1.0""?>
    <!--  This File has been updated by the CreateAutomationMasterFile script -->
<UIAutomation>
    {Regex.Replace(root.UIElementXML(), $"{Environment.NewLine}", $"{Environment.NewLine}    ")}
    {Regex.Replace(root.PageObjectXML(), $"{Environment.NewLine}", $"{Environment.NewLine}    ")}
</UIAutomation>
";
                File.WriteAllText(automationMasterFile, automationMasterContent);
                var proj = XDocument.Load(automationMasterFile, LoadOptions.None);
                proj.Save(automationMasterFile);

                automationMasterContent = File.ReadAllText(automationMasterFile);
                automationMasterContent = automationMasterContent.Replace(@"utf-8", @"utf-16");
                automationMasterContent = Regex.Replace(automationMasterContent,@"([^ ])( *)\<SimpleElements\>\<\/SimpleElements\>", $@"$1$2<SimpleElements>{Environment.NewLine}$2</SimpleElements>");

                File.WriteAllText(automationMasterFile, automationMasterContent);
            }
        }
        public static void ClearCodeFile(String interfaceCodeFile, String classCodeFile)
        {
            File.WriteAllText(interfaceCodeFile, $"// Generating code {DateTime.Now}");
            File.WriteAllText(classCodeFile, $"// Generating code {DateTime.Now}");

            File.WriteAllText(classCodeFile, $@"

namespace MC.Track.TestSuite.Interfaces.Pages.Generated.Shared
{{
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
    

    [AthenaRegister(typeof(IGeneratedPages), AthenaRegistrationType.Singleton)]
    public partial class GeneratedPages
    {{
        private readonly IResolver resolver;
        public T CreatePage<T>() where T: class, IPageBase
        {{
            var page = resolver.Resolve<T>();
            page.Setup(resolver);
            return page;
        }}
        public GeneratedPages(IResolver resolver)
        {{
            this.resolver = resolver;
        }}
    }}
}}");


        }
        public void CreateCodeFile(out String interfaceCode, out String classCode)
        {
            var HtmlFile = Root.Source.Identifier();

            var root = this;
            interfaceCode = root.BuildInterfaceFile();
            classCode = root.BuildClassFile();

        }
    }
}
