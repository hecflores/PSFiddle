using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using PSFiddle.UIAutomation.POM.Models.POMParsedSources;
using PSFiddle.UIAutomation.POM.Models.POMParsingRules.AngularParsingRules;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;

namespace PSFiddle.UIAutomation.POM.Models.POMFileParsingStrategies
{
    public class JSDirectivePOMFileParsingStrategy : POMFileParsingStrategy
    {
        public JSDirectivePOMFileParsingStrategy(string Name, string Directory, POMParsingContext pOMParsingContext) : base(Name, Directory, pOMParsingContext)
        {
        }

        
        public override List<string> DiscoverParsingFiles()
        {
            var directory = new DirectoryInfo(Directory);
            var csHtmlFiles = directory.GetFiles("*.js", SearchOption.AllDirectories).Select(b => b.FullName).ToList();
            return csHtmlFiles.Where(b => !b.Contains("obj")).ToList();
        }
        private String QuoteGrouping(String name)
        {
            var quoteGrouping = @"(?x)(?:\'(?<Quote>(?>[^']+|' (?<DEPTH>)|' (?<-DEPTH>))*?(?(DEPTH)(?!)))'|(?x)\""(?<Quote>(?>[^""]+|"" (?<DEPTH>)|"" (?<-DEPTH>))*?(?(DEPTH)(?!)))"")";
            quoteGrouping = quoteGrouping.Replace("Quote", name);
            return quoteGrouping;
        }
        public override void ParseFile(string file)
        {
            // I found this amazing regex online... Dont ask me how it works...
            // https://stackoverflow.com/questions/1860740/powershell-regex-question-involving-balancing-parenthesis
            var regexString = @"(?x)
directive *?\(
   (?>
       [^()]+
     |
       \( (?<DEPTH>)
     |
       \) (?<-DEPTH>)
   )*
   (?(DEPTH)(?!))
\)";
            var jsonContent = File.ReadAllText(file);
            var matches = Regex.Matches(jsonContent, regexString);

            var urlMatch = $@"directive.*?{this.QuoteGrouping("DirectiveName")}[\s\S]*?templateUrl *:[\s\S]*?{this.QuoteGrouping("TemplateUrl")}";
            var rawMatch = $@"directive.*?{this.QuoteGrouping("DirectiveName")}[\s\S]*?template *:[\s\S]*?{this.QuoteGrouping("Template")}";
            var templateUrlMatches = matches.Cast<Match>().Where<Match>(b => Regex.Match(b.Value, urlMatch).Success);
            var templateRawMatches = matches.Cast<Match>().Where<Match>(b => Regex.Match(b.Value, rawMatch).Success);

            var templateUrls = new List<String>();
            var templateRaws = new List<String>();

            if(templateUrlMatches.Count() == 0 && templateRawMatches.Count() == 0)
            {
                Context.WriteLine($"{file}{Environment.NewLine}  No directives with template or templateUrl found", ConsoleColor.Yellow);
                return;
            }

            Context.WriteLine($"File: {file}", ConsoleColor.Green);

            foreach(Match templateUrlMatch in templateUrlMatches)
            {
                var match = Regex.Match(templateUrlMatch.Value, urlMatch);
                var directiveName = match.Groups["DirectiveName"].Value;
                var templateUrl   = match.Groups["TemplateUrl"].Value;
                var templateFile  = Context.ResolvePath(file, templateUrl);
                if (templateFile == null)
                    continue;

                var htmlFileSource  = new HtmlFilePOMParsedSource(Context, templateFile);
                var directiveSource = new HtmlDirectiveTemplateFilePOMParsedSource(Context, file, directiveName, htmlFileSource);
                var parsedSource = Context.ParsedHtmls.Add(directiveSource);
                parsedSource.Source().Root = null;
                var root = parsedSource.ParseHtml();

                Context.WriteLine($"    Directive Name: {directiveName}{Environment.NewLine}    Template Url: {templateUrl}", ConsoleColor.Green);

                var components = root.Children().OfType<UIComponentPOMParsedElement>();
                if (components.Count() == 0)
                {
                    Context.WriteLine($"{templateFile}{Environment.NewLine}   No ui-component foundelements found at all", ConsoleColor.Yellow);
                    continue;
                }
                foreach(var component in components)
                {
                    var newParsingRule = new DynamicNgDirectivePOMParsingRule(Context, directiveName, component.Name);
                    Context.ParsingRules.Add(newParsingRule);
                }
                
                
            }

            foreach (Match templateRawMatch in templateRawMatches)
            {
                var match = Regex.Match(templateRawMatch.Value, urlMatch);
                var directiveName = match.Groups["DirectiveName"].Value;
                var template = match.Groups["Template"].Value;

                var rawSource       = new HtmlTextPOMParsedSource(Context, template);
                var directiveSource = new HtmlDirectiveTemplateTextPOMParsedSource(Context, file, directiveName, rawSource);
                var parsedSource = Context.ParsedHtmls.Add(directiveSource);
                parsedSource.Source().Root = null;
                var root = parsedSource.ParseHtml();

                Context.WriteLine($"    Directive Name: {directiveName}{Environment.NewLine}    Template....: {template}", ConsoleColor.Green);

                var components = root.Children().OfType<UIComponentPOMParsedElement>();
                if (components.Count() == 0)
                {
                    Context.WriteLine($"{template}{Environment.NewLine}   No ui-component found elements found at all", ConsoleColor.Yellow);
                    continue;
                }

                foreach(var component in components)
                {
                    var newParsingRule = new DynamicNgDirectivePOMParsingRule(Context, directiveName, component.Name);
                    Context.ParsingRules.Add(newParsingRule);
                }
            }
        }

        public override int Priority()
        {
            return 1;
        }
    }
}
