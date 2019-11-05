using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using HtmlAgilityPack;
using System.Reflection;
using PSFiddle.UIAutomation.POM.Attributes;

namespace PSFiddle.UIAutomation.POM.Models.Collections
{
    
    public class POMParsingRuleCollection : BaseCollection<POMParsingRule>
    {

        public POMParsingRuleCollection(POMParsingContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets  any rule selector. Meaning will give you the XPath selector that will find any of the rules in the collection
        /// </summary>
        /// <value>
        /// Any rule selector.
        /// </value>
        public String AnyRuleSelector { get => String.Join(" | ", this.Items.Where(b=>b.DiscoveryXPath != null).Select(b => b.DiscoveryXPath)); }

        /// <summary>
        /// Parses the specified HTML element.
        /// </summary>
        /// <param name="htmlElement">The HTML element.</param>
        /// <param name="currentParsedUI">The current parsed UI.</param>
        /// <returns></returns>
        public POMParsedRawElement Parse(HtmlNode htmlElement, POMParsedRawElement currentParsedUI)
        {
            var sortedParsedUIItems = this.OrderByDescending(x => x.Priority()).ToList();

            // Pre Parsed
            foreach (POMParsingRule item in sortedParsedUIItems)
            {
                POMParsedRawElement parsed = item.PreParse(htmlElement, currentParsedUI);
                if (parsed != null)
                {
                    currentParsedUI = item.AddNewParsedElement(htmlElement, currentParsedUI, null, parsed);
                    break;
                }
            }

            // Find a match
            POMParsedRawElement matchedParsedUI = null;
            foreach (POMParsingRule item in sortedParsedUIItems)
            {
                POMParsedRawElement parsed = item.Parse(htmlElement, currentParsedUI);
                if (parsed != null)
                {
                    matchedParsedUI = item.AddNewParsedElement(htmlElement, currentParsedUI, matchedParsedUI, parsed);
                    break;
                }
            }
            
            // In the scenario where their was a match
            if (matchedParsedUI != null)
            {
                foreach (POMParsingRule item in sortedParsedUIItems)
                {
                    POMParsedRawElement parsed = item.ParseAfterMatch(htmlElement, matchedParsedUI);
                    if (parsed != null)
                    {
                        item.AddNewParsedElement(htmlElement, matchedParsedUI, matchedParsedUI, parsed);
                    }
                }
            }

            // In any scenario, perform "Alone" parse...
            if (matchedParsedUI != null || matchedParsedUI == null)
            {
                foreach (POMParsingRule item in sortedParsedUIItems)
                {
                    POMParsedRawElement parsed = item.ParseAlone(htmlElement, currentParsedUI, matchedParsedUI);
                    if (parsed != null)
                    {
                        item.AddNewParsedElement(htmlElement, currentParsedUI, matchedParsedUI,  parsed);
                    }

                }
            }

            return matchedParsedUI ?? currentParsedUI;
        }
        public void Init()
        {
            var sortedParsedUIItems = this.OrderByDescending(x => x.Priority()).ToList();
            foreach (POMParsingRule item in sortedParsedUIItems)
            {
                item.Init();
            }
        }
        public void PopulateFromAssemblyDiscovery()
        {
            var foundTypes = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => Attribute.GetCustomAttribute(type, typeof(ParsingRuleDefinitionAttribute)) != null);
            foreach(var type in foundTypes)
            {
                if (!typeof(POMParsingRule).IsAssignableFrom(type))
                    throw new Exception($"Type '{type}' was expected to be assignable from type '{typeof(POMParsingRule)}'");

                var rule = Activator.CreateInstance(type, Context) as POMParsingRule;
                this.Add(rule);
            }
                    


                    
        }
    }
}
