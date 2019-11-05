using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace PSFiddle.UIAutomation.POM.Models.Collections
{
    public class POMFileParsingStrategyCollection : BaseCollection<POMFileParsingStrategy>
    {
        public POMFileParsingStrategyCollection(POMParsingContext context) : base(context)
        {
        }

        public void ExecuteParsingStategies()
        {
            var stategies = this.OrderByDescending(b => b.Priority()).ToList();
            
            foreach (var stategy in stategies)
            {
                var files = stategy.DiscoverParsingFiles();

                // Some Counts
                var maxItems = files.Count();
                var currentItem = 0;
                var failedItems = 0;
                var passedItems = 0;

                foreach (var file in files)
                {
                    var percentDone = (int)((((double)currentItem) / ((double)maxItems))*100.0);
                    Console.Write($"{String.Format("{0,-20}",stategy.Name)} - {percentDone}% Failed({failedItems}), Passed({passedItems})            \r");

                    try
                    {
                        stategy.ParseFile(file);
                        passedItems++;
                    }
                    catch(Exception e)
                    {
                        Context.WriteLine($"{e.Message}{Environment.NewLine}{e.StackTrace}", ConsoleColor.Red);
                        failedItems++;
                    }
                    finally
                    {
                        currentItem++;
                    }
                }
                Console.WriteLine($"{String.Format("{0,-20}", stategy.Name)} - {100}% Failed({failedItems}), Passed({passedItems})");
            }
        }
    }
}
