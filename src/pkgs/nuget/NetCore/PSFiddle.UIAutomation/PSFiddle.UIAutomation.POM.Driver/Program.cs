using PSFiddle.UIAutomation.POM.Models;
using PSFiddle.UIAutomation.POM.Models.POMFileParsingStrategies;
using System;
using System.IO;
namespace PSFiddle.UIAutomation.POM.Driver
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
                throw new Exception("Expected atleast argument. 'Root Directory'");
            var context = new POMParsingContext();
            var userInterface = args[0];
            var uiAutomationFolder = args[1];

            Console.WriteLine("Parsing...");
            context.Init();
            context.ParsingStategies.Add(new RazorHtmlFilesPOMFileParsingStrategy("CSHTML Views", Path.Combine(userInterface, @".\"), context));
            context.ParsingStategies.Add(new HtmlFilesPOMFileParsingStrategy("HTML Files", Path.Combine(userInterface, @".\ClientApp"), context));
            context.ParsingStategies.Add(new HtmlFilesPOMFileParsingStrategy("HTML Files (Mocking)", Path.Combine(userInterface, @".\ABCs\Mocking\ClientApp"), context));
            context.ParsingStategies.Add(new JSDirectivePOMFileParsingStrategy("JS Directive Files", Path.Combine(userInterface, @".\"), context));
            context.ParsingStategies.ExecuteParsingStategies();
            context.CreateAutomationMasterFiles();
            context.CreateCodeFiles(Path.Combine(uiAutomationFolder, @".pom\IPages.cs"), 
                                    Path.Combine(uiAutomationFolder, @".pom\Pages.cs"),
                                    Path.Combine(userInterface, @".\"),
                                    Path.Combine(uiAutomationFolder, @".pom"));
            Console.WriteLine("All Done.");

        }
    }
}
