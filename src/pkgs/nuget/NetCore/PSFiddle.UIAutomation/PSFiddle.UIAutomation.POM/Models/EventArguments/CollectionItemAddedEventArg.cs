using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.EventArguments
{
    public class CollectionItemAddedEventArg<T>
    {
        public T Item { get; set; }
    }
}
