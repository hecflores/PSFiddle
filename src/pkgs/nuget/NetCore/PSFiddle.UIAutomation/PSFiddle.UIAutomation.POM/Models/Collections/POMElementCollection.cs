using PSFiddle.UIAutomation.POM.Models.EventArguments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace PSFiddle.UIAutomation.POM.Models.Collections
{
    public class POMElementCollection : BaseCollection<POMRawElement>
    {

        public POMElementCollection(POMParsingContext context) : base(context)
        {
            
        }

    }
}
