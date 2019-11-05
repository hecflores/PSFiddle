using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models
{
    public class POMParsedHtml
    {
        private POMParsingContext context;
        protected POMParsedSource source;
        protected UIRootPOMParsedElement root;

        protected POMParsingContext Context { get => context; set => context = value; }

        public POMParsedHtml(POMParsingContext context, POMParsedSource source, UIRootPOMParsedElement root)
        {
            this.context = context;
            this.source = source;
            this.root = root;
        }
        public POMParsedSource Source()
        {
            return this.source;
        }
        public UIRootPOMParsedElement Root()
        {
            return this.source.Root;
        }
        public UIRootPOMParsedElement ParseHtml()
        {
            Context.WriteLine($"#### Parsing Html '{source.Identifier()}' ####",ConsoleColor.Magenta);
            return this.source.ParseHtml();
        }
        public String Identifier()
        {
            return this.source.Identifier();
        }
        public override int GetHashCode()
        {
            return source.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if(obj is POMParsedHtml html)
            {
                return html.Source().Equals(this.Source());
            }
            return false;
        }
    }
}
