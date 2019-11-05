using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Linq;
using PSFiddle.UIAutomation.POM.Constants;
using System.Linq;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedElements.Generic
{
    public static class CodeDOMExtensions
    {
        public static String GenerateCode(this CodeTypeMember method)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            options.IndentString = CommonCode.Spacing_CodeClassMethod;
            using (StringWriter sourceWriter = new StringWriter())
            {
                provider.GenerateCodeFromMember(
                    method, sourceWriter, options);

                return sourceWriter.GetStringBuilder().ToString();
            }

            
        }
    }
    public class BasicPOMParsedElementCreationModel
    {
        public bool CreateSimpleElement { get; set; } = true;
        public bool CreateUIElement { get; set; } = true;
        public bool CreateChildSimpleElements { get; set; } = true;
        public Dictionary<String, String> SimpleElementAttributeList { get; set; } = new Dictionary<string, string>();

    }
    public class BasicPOMParsedElement : POMParsedRawElement
    {
        private readonly BasicPOMParsedElementCreationModel creationModel;

        protected BasicPOMParsedElementCreationModel CreationModel => creationModel;

        public BasicPOMParsedElement(POMParsingContext context, HtmlNode htmlElement, POMParsedRawElement parent, POMParsingRule parsableDefinition, string uiElementName, string name, string selector) : this(context, htmlElement, parent, parsableDefinition, uiElementName, name, selector, new BasicPOMParsedElementCreationModel())
        {
        }
        public BasicPOMParsedElement(POMParsingContext context, HtmlNode htmlElement, POMParsedRawElement parent, POMParsingRule parsableDefinition, string uiElementName, string name, string selector, BasicPOMParsedElementCreationModel basicPOMParsedElementCreationModel) : base(context, htmlElement, parent, parsableDefinition, uiElementName, name, selector)
        {
            this.creationModel = basicPOMParsedElementCreationModel;

            CreationModel.SimpleElementAttributeList.Add("UIElement", UIElementName);
        }

        public override string UIElementXML()
        {
            if (!creationModel.CreateUIElement)
                return String.Empty;

            var description = UIElementName;
            description = Regex.Replace(description, "([A-Za-z])([A-Z])", "$1 $2");
            description = Regex.Replace(description, "(_)([A-Z])", " - $2");

            return $"<UIElement Name=\"{UIElementName}\" Description=\"{description}\">{Selector}</UIElement>";
        }


        public override string SimpleElementXML()
        {
            if (!creationModel.CreateSimpleElement)
                return String.Empty;

            
            var uiElements = this.Children().UIElementXML();
            uiElements += this.UIElementXML();

            var simpleElementAttributes = String.Join(" ", this.creationModel.SimpleElementAttributeList.Select(b => $"{b.Key}=\"{b.Value}\""));
            var simpleElement = $"<SimpleElement Name=\"{Name}\"  {simpleElementAttributes}  />";

            if (creationModel.CreateChildSimpleElements)
            {
                var simpleElements = this.Children().SimpleElementsXML();
                if (!String.IsNullOrEmpty(simpleElements))
                {
                    simpleElement = $"<SimpleElement Name=\"{Name}\"  {simpleElementAttributes} > "
                        + "<UIElements>" + (uiElements) + "</UIElements>"
                        + (simpleElements)
                        + "</SimpleElement>";
                }
            }
            
            return simpleElement;
        }

        public override string PageObjectXML()
        {
            return String.Empty;
        }
        private string BuildChartInterface()
        {


            var Name = $"{this.NameWithHistory}Chart";
            var MainInterfaceName = $"I{Name}";
            var methods = "";
            methods += $@"
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} ClickCategory(String chartItem);
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} VerifyIsLoaded();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} VerifyCategoryExists(String chartItem);
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} VerifyCategoryDoesNotExists(String chartItem);";

            var extension = $": IPageBase";
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

    public  partial interface I{Name} {extension}
    {{
{CommonCode.Spacing_CodeClassMethod}IGeneratedPages AllPages();
{CommonCode.Spacing_CodeClassMethod}I{Name} DoThings(Action<I{Name}> callback);
{methods}
    }}
}}
";
            return finalData;
        }
        public override string BuildInterface()
        {
            if (CreationModel.SimpleElementAttributeList.ContainsKey("IsChart"))
                return BuildChartInterface();

            if (Children().Count() == 0)
                return "";

            var Name = this.NameWithHistory;

            var extension = ": IPageBase";
            var methods = Children().BuildInterfaceMethods($"{{0}}{Environment.NewLine}");
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

    public  partial interface I{Name} {extension}
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
        private CodeMemberMethod CreateMethod(String Name, Action<CodeMemberMethod> methodBuilder)
        {
            var method = new CodeMemberMethod()
            {
                Name = Name
            };
            methodBuilder(method);
            return method;
        }

        private string BuildChartClass()
        {
            var ClassName = $"{this.NameWithHistory}Chart";
            var extension = $": ScopedPageUI, I{ClassName}";
            var MainInterfaceName = $"I{ClassName}";
            var methods = $@"
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} ClickCategory(String chartItem){{
{CommonCode.Spacing_CodeInnerMethod}ClickChartItem(chartItem);
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} VerifyIsLoaded(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyChartIsLoaded();
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} VerifyCategoryExists(String chartItem){{
{CommonCode.Spacing_CodeInnerMethod}VerifyChartCategoryExists(chartItem);
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} VerifyCategoryDoesNotExists(String chartItem){{
{CommonCode.Spacing_CodeInnerMethod}VerifyChartCategoryDoesNotExists(chartItem);
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}";
            
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
    

    public partial class {ClassName} {extension}
    {{
{CommonCode.Spacing_CodeClassMethod}public IGeneratedPages AllPages(){{
{CommonCode.Spacing_CodeInnerMethod}return resolver.Resolve<IGeneratedPages>();
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} DoThings(Action<{MainInterfaceName}> callback){{
{CommonCode.Spacing_CodeInnerMethod}WaitFor(() => callback.Invoke(this));
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public override void ValidatePage(){{
{CommonCode.Spacing_CodeInnerMethod}//Unkown action here...
{CommonCode.Spacing_CodeClassMethod}}}
{methods}
    }}
}}
";
            return finalData;
        }
        public override string BuildClass()
        {
            if (CreationModel.SimpleElementAttributeList.ContainsKey("IsChart"))
                return BuildChartClass();

            if (Children().Count() == 0)
                return "";

            var Name = this.NameWithHistory;

            var extension = $": ScopedPageUI, I{Name}";
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
    

    public partial class {Name} {extension}
    {{
{CommonCode.Spacing_CodeClassMethod}public IGeneratedPages AllPages(){{
{CommonCode.Spacing_CodeInnerMethod}return resolver.Resolve<IGeneratedPages>();
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public I{Name} DoThings(Action<I{Name}> callback){{
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
        public override string BuildInterfaceMethods()
        {
            var FullName = String.Join("", AncencestorsIncludeSelf().Reverse<POMParsedRawElement>().Select(b => b.Name));

            var code = "";
            var MainInterfaceName = $"I{this.Parent.NameWithHistory}";
            if (this.Parent is UIComponentPOMParsedElement component)
                MainInterfaceName = $"I{component.Component}";
            if (this.Parent is UIPagePOMParsedElement page)
                MainInterfaceName = $"I{page.Name}";

            var names = new HashSet<String>();
            names.Add(FullName);
            names.Add(Name);

            if (CreationModel.SimpleElementAttributeList.ContainsKey("Component"))
            {
                var ClassName = CreationModel.SimpleElementAttributeList["Component"];
                var InterfaceName = $"I{ClassName}";

                if(this is UIComponentPOMParsedElement)
                    names.Add(ClassName);

                foreach (var _name in names)
                {
                    code += $@"
{CommonCode.Spacing_CodeInterfaceMethod}[Obsolete(""Get{_name}Component() is deprecated, please use {Name}Component() instead."")]
{CommonCode.Spacing_CodeInterfaceMethod}{InterfaceName} Get{_name}Component();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{_name}Component();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{_name}IsNotSeen();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{_name}ComponentIsNotSeen();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{_name}ComponentIsMissing();";
                }

                code += $@"
{CommonCode.Spacing_CodeInterfaceMethod}{InterfaceName} {Name}Component();";

            }
            if (CreationModel.SimpleElementAttributeList.ContainsKey("IsChart"))
            {
                var ClassName = $"{NameWithHistory}Chart";
                var InterfaceName = $"I{ClassName}";

                if (this is UIComponentPOMParsedElement)
                    names.Add(ClassName);

                foreach (var _name in names)
                {
                    code += $@"
{CommonCode.Spacing_CodeInterfaceMethod}[Obsolete(""Get{_name}Component() is deprecated, please use {Name}Component() instead."")]
{CommonCode.Spacing_CodeInterfaceMethod}{InterfaceName} Get{_name}Component();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{_name}Component();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{_name}ComponentIsMissing();";
                }

                code += $@"
{CommonCode.Spacing_CodeInterfaceMethod}{InterfaceName} {Name}Chart();";

            }
            
            if (CreationModel.SimpleElementAttributeList.ContainsKey("CanRead"))
            {
                var ClassName = Name;
                var InterfaceName = $"I{Name}";

                code += $@"
{CommonCode.Spacing_CodeInterfaceMethod}String {Name} {{get;}}";
            }
            if (CreationModel.SimpleElementAttributeList.ContainsKey("CanEdit"))
            {
                var ClassName = Name;
                var InterfaceName = $"I{Name}";

                code += $@"
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Set{Name}(String {Name});";
            }
            if (CreationModel.SimpleElementAttributeList.ContainsKey("CanClick"))
            {
                var ClassName = Name;
                var InterfaceName = $"I{Name}";

                code += $@"
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Click{Name}();";
            }
            if (CreationModel.SimpleElementAttributeList.ContainsKey("CanVerify"))
            {
                var ClassName = Name;
                var InterfaceName = $"I{Name}";
                names.Add(ClassName);

                foreach (var _name in names)
                {
                    code += $@"
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{_name}IsNotEnabled();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{_name}IsNotSeen();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{_name}IsSelected();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{_name}IsNotSelected();";

                }

                code += $@"
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{Name}();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{Name}(String verifyText);";


            }
            // Get Component Methods
            if(Children().Count() > 0 && !(this is UIMultiplePOMParsedElement || this is UIComponentPOMParsedElement || this is UIPagePOMParsedElement || this is UIDirectivePOMParsedElement))
            {
                var Name = this.Name;
                var ClassName = this.NameWithHistory;
                var InterfaceName = $"I{ClassName}";


                code += $@"
{CommonCode.Spacing_CodeInterfaceMethod}{InterfaceName} {Name}Component();
{CommonCode.Spacing_CodeInterfaceMethod}{InterfaceName} Get{FullName}Component();
{CommonCode.Spacing_CodeInterfaceMethod}{InterfaceName} Get{Name}Component();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{Name}Component();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{FullName}Component();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{Name}ComponentIsMissing();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{FullName}ComponentIsMissing();
{CommonCode.Spacing_CodeInterfaceMethod}{MainInterfaceName} Verify{Name}ComponentIsNotSeen();";
            }


            // Get Component Methods
            if (Children().Count() > 0 && (this is UIMultiplePOMParsedElement ))
            {
                var Name = this.Name;
                var ClassName = this.NameWithHistory;
                var InterfaceName = $"I{ClassName}";

                code += $@"
{CommonCode.Spacing_CodeInnerMethod}List<{InterfaceName}> GetAll{Name}Components();
{CommonCode.Spacing_CodeInnerMethod}List<{InterfaceName}> GetAll{FullName}Components();
{CommonCode.Spacing_CodeInnerMethod}List<{InterfaceName}> {Name}Component();
    ";

                var QuerableComponents = this.ChildrenAndSelfDeep(b=>b.Children().OfType<UIQuerablePOMParsedElement>().Count() > 0, b=> ! (b is UIQuerablePOMParsedElement));
                foreach (var queuableLabelComponent in QuerableComponents)
                {
                    var traversePages = this.TraverseForwardTargetPage(queuableLabelComponent);

                    // Set up prefix code - TODO move to a reusable metod...
                    // Example: item.GetSomeComponent().SomeOtherComponent()
                    var prefixCode = "";
                    foreach (var syncs in traversePages)
                    {
                        if (String.IsNullOrEmpty(prefixCode))
                        {
                            prefixCode += "item";
                            continue;
                        }

                        prefixCode += $".Get{syncs.Name}Component()";
                        if (syncs.Name == queuableLabelComponent.Name)
                            break;
                    }

                    var queueableLabels = queuableLabelComponent.Children().OfType<UIQuerablePOMParsedElement>();
                    foreach(var label in queueableLabels)
                    {
                        code += $@"
{CommonCode.Spacing_CodeInterfaceMethod}{InterfaceName} Get{Name}ComponentBasedOn{label.Name}(String {label.Name});
{CommonCode.Spacing_CodeInterfaceMethod}{InterfaceName} Get{Name}BasedOn{label.Name}(String {label.Name});
{CommonCode.Spacing_CodeInterfaceMethod}{InterfaceName} Get{FullName}ComponentBasedOn{label.Name}(String {label.Name});
{CommonCode.Spacing_CodeInterfaceMethod}{InterfaceName} Get{FullName}BasedOn{label.Name}(String {label.Name});";

                    }

                }

            }

            return code;
        }

        public override string BuildClassMethods()
        {
            var FullName = String.Join("", AncencestorsIncludeSelf().Reverse<POMParsedRawElement>().Select(b => b.Name));

            var code = "";
            var MainInterfaceName = $"I{this.Parent.NameWithHistory}";
            if (this.Parent is UIComponentPOMParsedElement component)
                MainInterfaceName = $"I{component.Component}";
            if (this.Parent is UIPagePOMParsedElement page)
                MainInterfaceName = $"I{page.Name}";

            var names = new HashSet<String>();
            names.Add(FullName);
            names.Add(Name);

            if (CreationModel.SimpleElementAttributeList.ContainsKey("Component"))
            {
                var ClassName = CreationModel.SimpleElementAttributeList["Component"];
                var InterfaceName = $"I{ClassName}";
                
                if (this is UIComponentPOMParsedElement)
                    names.Add(ClassName);

                foreach (var _name in names)
                {
                    code += $@"
{CommonCode.Spacing_CodeClassMethod}public {InterfaceName} Get{_name}Component(){{
{CommonCode.Spacing_CodeInnerMethod}return PartialPage<{ClassName}>(GetElement({Locator.MultiLinedStringLiteral()}));
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{_name}Component(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElement({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{_name}ComponentIsNotSeen(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElementIsHidden({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{_name}IsNotSeen(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElementIsHidden({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{_name}ComponentIsMissing(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElementNotExists({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}";
                }
                code += $@"
{CommonCode.Spacing_CodeClassMethod}public {InterfaceName} {Name}Component(){{
{CommonCode.Spacing_CodeInnerMethod}return PartialPage<{ClassName}>(GetElement({this.Locator.MultiLinedStringLiteral()}));
{CommonCode.Spacing_CodeClassMethod}}}";



            }
            if (CreationModel.SimpleElementAttributeList.ContainsKey("IsChart"))
            {
                var ClassName = $"{NameWithHistory}Chart";
                var InterfaceName = $"I{ClassName}";

                if (this is UIComponentPOMParsedElement)
                    names.Add(ClassName);

                foreach (var _name in names)
                {
                    code += $@"
{CommonCode.Spacing_CodeClassMethod}public {InterfaceName} Get{_name}Component(){{
{CommonCode.Spacing_CodeInnerMethod}return PartialPage<{ClassName}>(GetElement({Locator.MultiLinedStringLiteral()}));
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{_name}Component(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElement({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{_name}ComponentIsMissing(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElementNotExists({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}";
                }
                code += $@"
{CommonCode.Spacing_CodeClassMethod}public {InterfaceName} {Name}Chart(){{
{CommonCode.Spacing_CodeInnerMethod}return PartialPage<{ClassName}>(GetElement({this.Locator.MultiLinedStringLiteral()}));
{CommonCode.Spacing_CodeClassMethod}}}";



            }
            if (CreationModel.SimpleElementAttributeList.ContainsKey("CanRead"))
            {
                var ClassName = Name;
                var InterfaceName = $"I{ClassName}";

                code += $@"
{CommonCode.Spacing_CodeClassMethod}public String {Name} {{get => Text({this.Locator.MultiLinedStringLiteral()}); }}";
            }
            if (CreationModel.SimpleElementAttributeList.ContainsKey("CanEdit"))
            {
                var ClassName = Name;
                var InterfaceName = $"I{ClassName}";

                code += $@"
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Set{Name}(String {Name}){{
{CommonCode.Spacing_CodeInnerMethod}TypeIn({Name},{this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}";
            }
            if (CreationModel.SimpleElementAttributeList.ContainsKey("CanClick"))
            {
                var ClassName = Name;
                var InterfaceName = $"I{ClassName}";

                code += $@"
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Click{Name}(){{
{CommonCode.Spacing_CodeInnerMethod}ClickElement({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}";
            }
            if (CreationModel.SimpleElementAttributeList.ContainsKey("CanVerify"))
            {
                var ClassName = Name;
                var InterfaceName = $"I{ClassName}";
                names.Add(ClassName);

                foreach (var _name in names)
                {
                    code += $@"
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{_name}IsNotSeen(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElementIsHidden({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{_name}IsNotEnabled(){{
{CommonCode.Spacing_CodeInnerMethod}GetElement({this.Locator.MultiLinedStringLiteral()}).VerifyElementIsNotEnabled();
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{_name}IsSelected(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyIsSelected({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{_name}IsNotSelected(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyIsNotSelected({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}";
                }
                code += $@"
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{Name}(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElement({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{Name}(String verifyText){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElementText({this.Locator.MultiLinedStringLiteral()}, verifyText);
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}";


            }

            // Get Component Methods
            if (Children().Count() > 0 && !(this is UIMultiplePOMParsedElement || this is UIComponentPOMParsedElement || this is UIPagePOMParsedElement || this is UIDirectivePOMParsedElement))
            {
                
                var Name = this.Name;
                
                var ClassName = this.NameWithHistory;
                var InterfaceName = $"I{ClassName}";

                code += $@"
{CommonCode.Spacing_CodeClassMethod}public {InterfaceName} {Name}Component(){{
{CommonCode.Spacing_CodeInnerMethod}return PartialPage<{ClassName}>(GetElement({this.Locator.MultiLinedStringLiteral()}));
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {InterfaceName} Get{FullName}Component(){{
{CommonCode.Spacing_CodeInnerMethod}return PartialPage<{ClassName}>(GetElement({this.Locator.MultiLinedStringLiteral()}));
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {InterfaceName} Get{Name}Component(){{
{CommonCode.Spacing_CodeInnerMethod}return PartialPage<{ClassName}>(GetElement({this.Locator.MultiLinedStringLiteral()}));
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{Name}Component(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElement({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{FullName}Component(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElement({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{Name}ComponentIsMissing(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElementNotExists({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{FullName}ComponentIsMissing(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElementNotExists({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {MainInterfaceName} Verify{Name}ComponentIsNotSeen(){{
{CommonCode.Spacing_CodeInnerMethod}VerifyElementIsHidden({this.Locator.MultiLinedStringLiteral()});
{CommonCode.Spacing_CodeInnerMethod}return this;
{CommonCode.Spacing_CodeClassMethod}}}";
            }


            // Get Component Methods
            if (Children().Count() > 0 && (this is UIMultiplePOMParsedElement))
            {
                var Name = this.Name;
                var ClassName = this.NameWithHistory;
                var InterfaceName = $"I{ClassName}";

                code += $@"
{CommonCode.Spacing_CodeClassMethod}public List<{InterfaceName}> GetAll{Name}Components(){{
{CommonCode.Spacing_CodeInnerMethod}return GetElements({this.Locator.MultiLinedStringLiteral()})
{CommonCode.Spacing_CodeInnerMethod}        .Select(b=>PartialPage<{ClassName}>(b))
{CommonCode.Spacing_CodeInnerMethod}        .Cast<{InterfaceName}>()
{CommonCode.Spacing_CodeInnerMethod}        .ToList();
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public List<{InterfaceName}> GetAll{FullName}Components(){{
{CommonCode.Spacing_CodeInnerMethod}return GetElements({this.Locator.MultiLinedStringLiteral()})
{CommonCode.Spacing_CodeInnerMethod}        .Select(b=>PartialPage<{ClassName}>(b))
{CommonCode.Spacing_CodeInnerMethod}        .Cast<{InterfaceName}>()
{CommonCode.Spacing_CodeInnerMethod}        .ToList();
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public List<{InterfaceName}> {Name}Component(){{
{CommonCode.Spacing_CodeInnerMethod}return GetElements({this.Locator.MultiLinedStringLiteral()})
{CommonCode.Spacing_CodeInnerMethod}        .Select(b=>PartialPage<{ClassName}>(b))
{CommonCode.Spacing_CodeInnerMethod}        .Cast<{InterfaceName}>()
{CommonCode.Spacing_CodeInnerMethod}        .ToList();
{CommonCode.Spacing_CodeClassMethod}}}";

                var QuerableComponents = this.ChildrenAndSelfDeep(b => b.Children().OfType<UIQuerablePOMParsedElement>().Count() > 0, b => !(b is UIQuerablePOMParsedElement));
                foreach (var queuableLabelComponent in QuerableComponents)
                {
                    var traversePages = this.TraverseForwardTargetPage(queuableLabelComponent);

                    // Set up prefix code - TODO move to a reusable metod...
                    // Example: item.GetSomeComponent().SomeOtherComponent()
                    var prefixCode = "item";
                    foreach (var syncs in traversePages)
                    {
                        prefixCode += $".Get{syncs.Name}Component()";
                        if (syncs.Name == queuableLabelComponent.Name)
                            break;
                    }

                    var queueableLabels = queuableLabelComponent.Children().OfType<UIQuerablePOMParsedElement>();
                    foreach (var label in queueableLabels)
                    {
                        var ListCode = $"this.GetAll{Name}Components()";
                        var ListItemVarName = "item";
                        var ConditionalCode = $"{prefixCode}.{label.Name}.Trim() == {label.Name}.Trim()";
                        var MessageOnError = $"No Items found where {label.Name} is equal to {{{label.Name}}}";

                        
                        code += $@"
{CommonCode.Spacing_CodeClassMethod}public {InterfaceName} Get{Name}ComponentBasedOn{label.Name}(String {label.Name}){{
{CommonCode.Spacing_CodeInnerMethod}return WaitFor(()=>{{
{CommonCode.Spacing_CodeInnerMethod}    var foundItem = {ListCode}.Where({ListItemVarName} => {ConditionalCode}).FirstOrDefault();
{CommonCode.Spacing_CodeInnerMethod}    Assert.IsNotNull(foundItem, $""{MessageOnError}"");
{CommonCode.Spacing_CodeInnerMethod}    return foundItem;
{CommonCode.Spacing_CodeInnerMethod}}});
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {InterfaceName} Get{Name}BasedOn{label.Name}(String {label.Name}){{
{CommonCode.Spacing_CodeInnerMethod}return WaitFor(()=>{{
{CommonCode.Spacing_CodeInnerMethod}    var foundItem = {ListCode}.Where({ListItemVarName} => {ConditionalCode}).FirstOrDefault();
{CommonCode.Spacing_CodeInnerMethod}    Assert.IsNotNull(foundItem, $""{MessageOnError}"");
{CommonCode.Spacing_CodeInnerMethod}    return foundItem;
{CommonCode.Spacing_CodeInnerMethod}}});
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {InterfaceName} Get{FullName}ComponentBasedOn{label.Name}(String {label.Name}){{
{CommonCode.Spacing_CodeInnerMethod}return WaitFor(()=>{{
{CommonCode.Spacing_CodeInnerMethod}    var foundItem = {ListCode}.Where({ListItemVarName} => {ConditionalCode}).FirstOrDefault();
{CommonCode.Spacing_CodeInnerMethod}    Assert.IsNotNull(foundItem, $""{MessageOnError}"");
{CommonCode.Spacing_CodeInnerMethod}    return foundItem;
{CommonCode.Spacing_CodeInnerMethod}}});
{CommonCode.Spacing_CodeClassMethod}}}
{CommonCode.Spacing_CodeClassMethod}public {InterfaceName} Get{FullName}BasedOn{label.Name}(String {label.Name}){{
{CommonCode.Spacing_CodeInnerMethod}return WaitFor(()=>{{
{CommonCode.Spacing_CodeInnerMethod}    var foundItem = {ListCode}.Where({ListItemVarName} => {ConditionalCode}).FirstOrDefault();
{CommonCode.Spacing_CodeInnerMethod}    Assert.IsNotNull(foundItem, $""{MessageOnError}"");
{CommonCode.Spacing_CodeInnerMethod}    return foundItem;
{CommonCode.Spacing_CodeInnerMethod}}});
{CommonCode.Spacing_CodeClassMethod}}}
    ";
                    }

                }

            }

            return code;
        }
    }
}
