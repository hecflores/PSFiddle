using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.EventArguments
{
    public class CurrentPOMParsedRawElementChangedEventArg : EventArgs
    {
        public POMParsedRawElement CurrentParsedRawElement { get; set; }
    }
}
