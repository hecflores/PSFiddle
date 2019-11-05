using System;
using System.Collections.Generic;
using System.Text;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using HtmlAgilityPack;
using PSFiddle.UIAutomation.POM.Models.Collections;

namespace PSFiddle.UIAutomation.POM.Models.POMParsedSources
{
    public class HtmlFilePOMParsedSource : POMParsedSource
    {
        private readonly string file;

        public HtmlFilePOMParsedSource(POMParsingContext context, String File) : base(context)
        {
            file = File;
            context.ParsingRules.ItemAddedEvent += ParsingRuleWasAdded;
        }

        private void ParsingRuleWasAdded(object sender, EventArguments.CollectionItemAddedEventArg<POMParsingRule> e)
        {
            Root = null;
        }

        public string File()
        {
            return file;
        }
        public override string Identifier()
        {
            return file;
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
                var newParsedUIElement = Context.ParsingRules.Parse(firstNode, parsedUIElement);
                TraverseHtml(firstNode, newParsedUIElement);

                firstNode.Remove();
                firstNode = htmlNode.SelectSingleNode(Context.ParsingRules.AnyRuleSelector);
            }

        }
        public override UIRootPOMParsedElement ParseHtml()
        {
            if(this.Root != null){
                // Console.WriteLine($"  Cached '{this.file}'");
                return this.Root;
            }
            lock (file)
            {
                var doc = new HtmlDocument();
                doc.Load(file);
                var newRoot = new UIRootPOMParsedElement(Context, this);

                // Context.WriteLine($"Using XPath Selector '{Context.ParsingRules.AnyRuleSelector}'", ConsoleColor.Cyan);

                var rules = new POMParsingRuleCollection(Context);


                Context.ParsingRules.Init();
                TraverseHtml(doc.DocumentNode, newRoot);
                Root = newRoot;
                return newRoot;
            }

            


        }
    }
}
