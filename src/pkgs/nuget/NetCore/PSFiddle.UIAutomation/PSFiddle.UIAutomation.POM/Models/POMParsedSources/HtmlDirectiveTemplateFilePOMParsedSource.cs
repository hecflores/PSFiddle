using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedSources
{
    public class HtmlDirectiveTemplateFilePOMParsedSource : HtmlFilePOMParsedSource
    {
        private readonly string directiveFile;
        private readonly string directiveName;
        private readonly HtmlFilePOMParsedSource templateSource;

        public HtmlDirectiveTemplateFilePOMParsedSource(POMParsingContext context, string directiveFile, String directiveName, HtmlFilePOMParsedSource templateSource) : base(context, templateSource.File())
        {
            this.directiveFile = directiveFile;
            this.directiveName = directiveName;
            this.templateSource = templateSource;
        }

        public string DirectiveFile => directiveFile;

        public string DirectiveName => directiveName;

        public override string Identifier()
        {
            return templateSource.Identifier();
        }
    }
}
