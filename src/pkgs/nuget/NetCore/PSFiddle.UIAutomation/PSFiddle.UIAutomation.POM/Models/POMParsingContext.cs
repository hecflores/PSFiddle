using System;
using System.Collections.Generic;
using System.Text;
using PSFiddle.UIAutomation.POM.Models.Collections;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using PSFiddle.UIAutomation.POM.Models.POMParsedSources;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using PSFiddle.UIAutomation.POM.Models.EventArguments;

namespace PSFiddle.UIAutomation.POM.Models
{
    public class POMParsingContext
    {
        /// <summary>
        /// Gets the parsing stategies.
        /// </summary>
        /// <value>
        /// The parsing stategies.
        /// </value>
        public POMFileParsingStrategyCollection ParsingStategies { get; private set; }

        /// <summary>
        /// Gets the parsing rules.
        /// </summary>
        /// <value>
        /// The parsing rules.
        /// </value>
        public POMParsingRuleCollection ParsingRules { get; private set; } 

        /// <summary>
        /// Gets the parsing files.
        /// </summary>
        /// <value>
        /// The parsing files.
        /// </value>
        public POMParsingFileCollection ParsingFiles { get; private set; }

        /// <summary>
        /// Gets the parsed HTMLS.
        /// </summary>
        /// <value>
        /// The parsed HTMLS.
        /// </value>
        public POMParsedHtmlCollection ParsedHtmls { get; private set; }

        public POMParsingContext()
        {
            this.ParsingRules = new POMParsingRuleCollection(this);
            this.ParsingFiles = new POMParsingFileCollection(this);
            this.ParsedHtmls  = new POMParsedHtmlCollection(this);
            this.ParsingStategies = new POMFileParsingStrategyCollection(this);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init()
        {
            this.ParsingRules.PopulateFromAssemblyDiscovery();
        }

        /// <summary>
        /// Parses the HTML file.
        /// </summary>
        /// <param name="HtmlFile">The HTML file.</param>
        /// <returns></returns>
        public UIRootPOMParsedElement ParseHtmlFile(String HtmlFile)
        {
            var source = ParsedHtmls.Add(new HtmlFilePOMParsedSource(this, HtmlFile));
            return source.ParseHtml();
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="Message">The message.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        public void WriteLine(String Message, ConsoleColor foregroundColor)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(Message);
            Console.ForegroundColor = defaultColor;
        }

        /// <summary>
        /// Creates the automation master file.
        /// </summary>
        /// <param name="HtmlFile">The HTML file.</param>
        public void CreateAutomationMasterFile(String HtmlFile)
        {
            var root = ParseHtmlFile(HtmlFile);
            if(root.Children().Count() > 0)
            {
                if(root.Children().Where(b=>b is UIPagePOMParsedElement || b is UIComponentPOMParsedElement).Count() == 0)
                {
                    WriteLine($"{HtmlFile}{Environment.NewLine}   No ui-page or ui-component found", ConsoleColor.Red);
                    return;
                }

                var automationMasterFile = Path.ChangeExtension(HtmlFile, ".automationmaster.generated");
                WriteLine($"Generating {automationMasterFile}", ConsoleColor.White);
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
            }
        }
        public void CreateAutomationMasterFiles()
        {
            foreach(var html in ParsedHtmls)
            {
                try
                {
                    WriteLine($"Creating Automation Master for {html.Source().Identifier()} of source type {html.Source().GetType()}", ConsoleColor.White);
                    html.Source().Root.CreateAutomationMasterFile();
                }
                catch(Exception e)
                {
                    WriteLine($"Failed to create automation master{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}", ConsoleColor.Red);
                }
            }
        }
        public void CreateCodeFiles(String originalInterfaceFile, String originalClassFile, String rootParsingFolder, String rootDumpingFolder)
        {
            rootParsingFolder = Path.GetFullPath(rootParsingFolder);
            rootDumpingFolder = Path.GetFullPath(rootDumpingFolder);
            originalInterfaceFile = Path.GetFullPath(originalInterfaceFile);
            originalClassFile = Path.GetFullPath(originalClassFile);

            UIRootPOMParsedElement.ClearCodeFile(originalInterfaceFile, originalClassFile);

            var rootFolder = Path.Combine(originalInterfaceFile, "../");
            
            // Some Counts
            var maxItems = ParsedHtmls.Count();
            var currentItem = 0;
            var failedItems = 0;
            var passedItems = 0;
            
            foreach (var html in ParsedHtmls)
            {
                var percentDone = (int)((((double)currentItem) / ((double)maxItems)) * 100.0);
                Console.Write($"{String.Format("{0,-20}", "Creating Code")} - {percentDone}% Failed({failedItems}), Passed({passedItems})            \r");

                try
                {
                    String interfaceCode = "";
                    String classCode = "";

                    var interfaceFile = originalInterfaceFile;
                    var classFile = originalClassFile ;

                    var identifier = Path.GetFullPath(html.Source().Identifier());
                    
                    if(File.Exists(identifier))
                    {
                        var relativeFilePath = identifier.Replace(rootParsingFolder, "");   
                        var ghostedHtmlFile = Path.Combine(rootDumpingFolder, relativeFilePath);
                        var ghostedDirectory = Path.GetFullPath(Path.Combine(ghostedHtmlFile, "../"));

                        interfaceFile =  Path.ChangeExtension(ghostedHtmlFile, ".cs");
                        classFile =  Path.ChangeExtension(ghostedHtmlFile, ".cs");

                        Directory.CreateDirectory(ghostedDirectory);
                    }
                    WriteLine($"Creating Code File for {html.Source().Identifier()} of source type {html.Source().GetType()}", ConsoleColor.White);
                    var tempInterfaceCode = "";
                    var tempClassCode = "";
                    html.Source().Root.CreateCodeFile(out tempInterfaceCode, out tempClassCode);
                    passedItems++;
                    interfaceCode += tempInterfaceCode;
                    classCode += tempClassCode;
                    
                    
                    File.AppendAllText(interfaceFile, interfaceCode);
                    File.AppendAllText(classFile, classCode);
                }
                catch (Exception e)
                {
                    WriteLine($"Failed to create code file{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}", ConsoleColor.Red);
                    failedItems++;
                }
                finally
                {
                    currentItem++;
                }
            }

            
        }
        private String _ResolvePath(String _root, String _findingPath)
        {
            if (File.Exists(_root))
                return _ResolvePath((new FileInfo(_root)).Directory.FullName, _findingPath);

            if (!Directory.Exists(_root))
            {
                WriteLine($"Root Folder '{_root}' does not exists", ConsoleColor.Red);
                return null;
            }

            DirectoryInfo folder = new DirectoryInfo(_root);
            if (folder.Parent == null)
            {
                WriteLine($"Reached the end of the parent line: '{_root}'", ConsoleColor.Red);
                return null;
            }

            var suggestedFile = Path.Combine(_root, _findingPath);
            if (File.Exists(suggestedFile))
                return suggestedFile;

            return _ResolvePath(folder.Parent.FullName, _findingPath);
        }
        public String ResolvePath(String ReferencedFrom, String ReferencePath)
        {
            var Path = Regex.Replace(ReferencePath, "^/{0,1}(.*)$", "$1");
            var path = _ResolvePath(ReferencedFrom, Path);
            if (path == null)
                WriteLine($"Unable to find path {Path} from reference {ReferencedFrom}", ConsoleColor.Red);
            return path;
        }
    }
}
