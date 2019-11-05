using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedSources
{
    public class HtmlTextPOMParsedSource : POMParsedSource
    {
        private readonly string text;

        public HtmlTextPOMParsedSource(POMParsingContext context, String Text) : base(context)
        {
            text = Text;
        }
        public string Content()
        {
            return text;
        }
        public override string Identifier()
        {
            return text;
        }
        /// <summary>
        /// Populates the HTML node.
        /// </summary>
        /// <param name="htmlNode">The HTML node.</param>
        /// <param name="containsUIElements">The contains UI elements.</param>
        public void TraverseHtml(HtmlNode htmlNode, POMParsedRawElement parsedUIElement)
        {
            var firstNode = htmlNode.SelectSingleNode(Context.ParsingRules.AnyRuleSelector);
            while (firstNode != null)
            {
                parsedUIElement = Context.ParsingRules.Parse(firstNode, parsedUIElement);
                TraverseHtml(firstNode, parsedUIElement);

                firstNode.Remove();
                firstNode = htmlNode.SelectSingleNode(Context.ParsingRules.AnyRuleSelector);
            }

        }
        public override UIRootPOMParsedElement ParseHtml()
        {
            if (this.Root != null)
            {
                return this.Root;
            }

            var doc = new HtmlDocument();
            doc.Load(text);
            var newRoot = new UIRootPOMParsedElement(Context, this);
            Context.ParsingRules.Init();
            TraverseHtml(doc.DocumentNode, newRoot);
            Root = newRoot;

            return newRoot;


        }
    }
}
