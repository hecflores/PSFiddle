using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Constants;
using PSFiddle.UIAutomation.POM.Models.Collections;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace PSFiddle.UIAutomation.POM.Models
{
    public abstract class POMParsedRawElement : TreeNodes<POMParsedRawElement, POMParsedElementCollection>
    {
        private readonly POMParsingContext context;
        private readonly HtmlNode htmlElement;
        private readonly POMParsedRawElement parent;
        private readonly UIRootPOMParsedElement root;
        private readonly POMParsingRule parsingRule;
        private readonly string uiElementName;
        private readonly string name;
        private readonly string selector;
        private readonly string description;


        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public POMParsingContext Context => context;

        /// <summary>
        /// Gets the HTML element.
        /// </summary>
        /// <value>
        /// The HTML element.
        /// </value>
        public HtmlNode HtmlElement => htmlElement;

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public POMParsedRawElement Parent => parent;

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        public UIRootPOMParsedElement Root => root;
        /// <summary>
        /// Gets the parsing rule.
        /// </summary>
        /// <value>
        /// The parsing rule.
        /// </value>
        public POMParsingRule ParsingRule => parsingRule;

        /// <summary>
        /// Gets the name of the UI element.
        /// </summary>
        /// <value>
        /// The name of the UI element.
        /// </value>
        public string UIElementName => uiElementName;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name => name;

        /// <summary>
        /// Gets the name with history.
        /// </summary>
        /// <value>
        /// The name with history.
        /// </value>
        public String NameWithHistory => String.Join("__", AncencestorsIncludeSelf().Reverse<POMParsedRawElement>().Select(b => b.Name));

        /// <summary>
        /// Ancencestorses the include self.
        /// </summary>
        /// <returns></returns>
        public List<POMParsedRawElement> AncencestorsIncludeSelf()
        {
            if (this.Parent == null)
                return new List<POMParsedRawElement>() { this };

            var ancestors = new List<POMParsedRawElement>();
            ancestors.Add(this);
            ancestors.AddRange(this.Parent.AncencestorsIncludeSelf());
            return ancestors;

        }

        /// <summary>
        /// Gets the selector.
        /// </summary>
        /// <value>
        /// The selector.
        /// </value>
        public string Selector => selector;

        public string Description => description;

        /// <summary>
        /// Gets the locator.
        /// </summary>
        /// <value>
        /// The locator.
        /// </value>
        public String Locator => $"UIElement |{Description}|==|{Selector}|";

        /// <summary>
        /// Initializes a new instance of the <see cref="POMParsedRawElement"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="htmlElement">The HTML element.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="parsableDefinition">The parsable definition.</param>
        /// <param name="uiElementName">Name of the UI element.</param>
        /// <param name="name">The name.</param>
        /// <param name="selector">The selector.</param>
        public POMParsedRawElement(POMParsingContext context, HtmlNode htmlElement, POMParsedRawElement parent, POMParsingRule parsableDefinition, string uiElementName, string name, string selector)
        {
            this.context = context;
            this.htmlElement = htmlElement;
            this.parent = parent;
            this.parsingRule = parsableDefinition;
            this.uiElementName = uiElementName;
            this.name = name;
            this.selector = selector;

            if(UIElementName != null)
                this.description = Regex.Replace(Regex.Replace(UIElementName, "([A-Za-z])([A-Z])", "$1 $2"), "(_)([A-Z])", " - $2");

            this.children = new POMParsedElementCollection(context);

            if (parent == null && this is UIRootPOMParsedElement)
                root = this as UIRootPOMParsedElement;
            else if (parent.Parent == null && parent is UIRootPOMParsedElement)
                root = parent as UIRootPOMParsedElement;
            else
                root = parent.Root;

            // Override the selector
            if (htmlElement != null)
                if (htmlElement.Attributes.Contains("ui-xpath-selector"))
                    this.selector = htmlElement.Attributes["ui-xpath-selector"].Value;
        }

        /// <summary>
        /// Traverses the forward target page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public List<POMParsedRawElement> TraverseForwardTargetPage(POMParsedRawElement page)
        {
            var children = this.Children();

            if (children.Count() == 0)
                return new List<POMParsedRawElement>();

            var found = children.Where(b => b.NameWithHistory == page.NameWithHistory);
            if (found.Count() != 0)
            {
                return new List<POMParsedRawElement>()
                {
                    this,
                    page
                };
            }

            foreach(var child in children)
            {
                var foundChildren = child.TraverseForwardTargetPage(page);
                if(foundChildren.Count() > 0)
                {
                    var finalCollection = new List<POMParsedRawElement>();
                    finalCollection.Add(this);
                    finalCollection.AddRange(foundChildren);
                    return finalCollection;
                }
            }

            return new List<POMParsedRawElement>();

        }
        /// <summary>
        /// UIs the element XML.
        /// </summary>
        /// <returns></returns>
        public abstract String UIElementXML();

        /// <summary>
        /// Simples the element XML.
        /// </summary>
        /// <returns></returns>
        public abstract String SimpleElementXML();

        /// <summary>
        /// Pages the object XML.
        /// </summary>
        /// <returns></returns>
        public abstract String PageObjectXML();

        private String _ResolvePath(String _root, String _findingPath)
        {
            if (File.Exists(_root))
                return _ResolvePath((new FileInfo(_root)).Directory.FullName, _findingPath);

            if (!Directory.Exists(_root))
            {
                Context.WriteLine($"Root Folder '{_root}' does not exists", ConsoleColor.Red);
                return null;
            }

            DirectoryInfo folder = new DirectoryInfo(_root);
            if (folder.Parent == null)
            {
                Context.WriteLine($"Reached the end of the parent line: '{_root}'", ConsoleColor.Red);
                return null;
            }

            var suggestedFile = Path.Combine(_root, _findingPath);
            if (File.Exists(suggestedFile))
                return (new FileInfo(suggestedFile)).FullName;

            return _ResolvePath(folder.Parent.FullName, _findingPath);
        }
        public String ResolvePath(String Path)
        {
            Path = Regex.Replace(Path, "^/{0,1}(.*)$", "$1");
            var path = _ResolvePath(Root.Source.Identifier(), Path);
            if (path == null)
                Context.WriteLine($"Unable to find path {Path} from reference {Root.Source.Identifier()}", ConsoleColor.Red);
            return path;
        }
        public virtual String BuildInterfaceFile()
        {
            return $"// No Interface File Defined for {Name}";
        }
        public virtual String BuildClassFile()
        {
            return $"// No Class File Defined for {Name}";
        }

        public virtual String BuildInterfaceNamespace()
        {
            return $"// No Interface Namespace Defined for {Name}";
        }
        public virtual String BuildClassNamespace()
        {
            return $"// No Class Namespace Defined for {Name}";
        }

        public virtual String BuildInterface()
        {
            return $"{CommonCode.Spacing_CodeInterface}// No Interface Defined for {Name}";
        }
        public virtual String BuildClass()
        {
            return $"{CommonCode.Spacing_CodeClass}// No Class Defined for {Name}";
        }

        public virtual String BuildInterfaceMethods()
        {
            return $"{CommonCode.Spacing_CodeInterfaceMethod}// No Interface Methods Defined for {Name}";
        }
        public virtual String BuildClassMethods()
        {
            return $"{CommonCode.Spacing_CodeClassMethod}// No Class Methods Defined for {Name}";
        }
    }
}
