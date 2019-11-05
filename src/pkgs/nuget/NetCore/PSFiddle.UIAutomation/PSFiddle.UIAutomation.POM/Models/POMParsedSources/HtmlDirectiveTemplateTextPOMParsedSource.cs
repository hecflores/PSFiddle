using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedSources
{
    public class HtmlDirectiveTemplateTextPOMParsedSource : HtmlTextPOMParsedSource
    {
        private readonly string directiveFile;
        private readonly string directiveName;
        private readonly HtmlTextPOMParsedSource templateSource;

        public HtmlDirectiveTemplateTextPOMParsedSource(POMParsingContext context, string directiveFile, String directiveName, HtmlTextPOMParsedSource templateSource) : base(context, templateSource.Content())
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
