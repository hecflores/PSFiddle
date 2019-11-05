using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.EventArguments
{
    public class CollectionItemAddingEventArg<T> : EventArgs
    {
        public T Item { get; set; }
        public bool Deny { get; set; }
        public String ReasonForDeny { get; set; }
    }
}
