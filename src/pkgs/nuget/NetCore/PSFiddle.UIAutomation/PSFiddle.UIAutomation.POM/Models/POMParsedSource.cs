using PSFiddle.UIAutomation.POM.Models.EventArguments;
using PSFiddle.UIAutomation.POM.Models.POMParsedElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models
{
    public abstract class POMParsedSource
    {
        private UIRootPOMParsedElement _parsedContent = null;

        public event EventHandler<CurrentPOMParsedRawElementChangedEventArg> CurrentParsedElementChangedEvent;

        public POMParsingContext Context { get; }
        public UIRootPOMParsedElement Root { get => _parsedContent; set {
                _parsedContent = value;
                CurrentParsedElementChangedEvent?.Invoke(this, new CurrentPOMParsedRawElementChangedEventArg() { CurrentParsedRawElement = value });
            } }

        public POMParsedSource(POMParsingContext context)
        {
            this.Context = context;
        }

        public abstract String Identifier();
        public abstract UIRootPOMParsedElement ParseHtml();

        public override bool Equals(object obj)
        {
            if (obj is POMParsedSource source)
            {
                return source.Identifier() == this.Identifier();
            }
            return false;
        }
        public override int GetHashCode()
        {
            return this.Identifier().GetHashCode();
        }

    }
}
