using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
namespace PSFiddle.UIAutomation.POM.Models.POMFileParsingStrategies
{
    public class RazorHtmlFilesPOMFileParsingStrategy : POMFileParsingStrategy
    {
        public RazorHtmlFilesPOMFileParsingStrategy(string Name, string Directory, POMParsingContext pOMParsingContext) : base(Name, Directory, pOMParsingContext)
        {
        }

        public override List<string> DiscoverParsingFiles()
        {
            var directory = new DirectoryInfo(Directory);
            var csHtmlFiles = directory.GetFiles("*.cshtml", SearchOption.AllDirectories).Select(b=>b.FullName).ToList();
            return csHtmlFiles.Where(b => !b.Contains("obj")).ToList();
        }

        public override void ParseFile(string File)
        {
            Context.ParseHtmlFile(File);
        }

        public override int Priority()
        {
            return 0;
        }
    }
}
