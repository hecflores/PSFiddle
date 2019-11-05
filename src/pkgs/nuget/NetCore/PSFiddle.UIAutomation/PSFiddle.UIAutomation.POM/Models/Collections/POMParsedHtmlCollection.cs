using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace PSFiddle.UIAutomation.POM.Models.Collections
{
    public class POMParsedHtmlCollection : BaseCollection<POMParsedHtml>
    {
        public POMParsedHtmlCollection(POMParsingContext context) : base(context)
        {
        }
        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(POMParsedSource item)
        {
            return this.Where(b => b.Identifier() == item.Identifier()).FirstOrDefault() != null;
        }

        /// <summary>
        /// Gets the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public POMParsedHtml Get(POMParsedSource item)
        {
            return this.Where(b => b.Identifier() == item.Identifier()).FirstOrDefault();
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public POMParsedHtml Add(POMParsedSource item)
        {
            var parsedHtml = Get(item);
            if(parsedHtml == null)
            {
                parsedHtml = new POMParsedHtml(Context, item, null);
                this.Add(parsedHtml);
            }
            return parsedHtml;
        }
    }
}
