using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.EventArguments
{
    public class POMElementAddingEventArg : EventArgs
    {
        public POMRawElement POMElement { get; set; }
        public bool Deny { get; set; }
        public String ReasonForDeny { get; set; }
    }
}
