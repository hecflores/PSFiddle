using PSFiddle.UIAutomation.POM.Models.EventArguments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace PSFiddle.UIAutomation.POM.Models.Collections
{
    public class POMParsedElementCollection : BaseCollectionOfTreeNodes<POMParsedRawElement, POMParsedElementCollection>
    {
        public POMParsedElementCollection(POMParsingContext context) : base(context)
        {
           
        }

        


        public String UIElementXML()
        {
            string uiElements = "";
            List<String> elementUsed = new List<string>();
            foreach (POMParsedRawElement item in this)
            {
                string uiElement = item.UIElementXML();
                if (!elementUsed.Equals(uiElement))
                {
                    uiElements += (uiElement + "\r\n");
                    elementUsed.Add(uiElement);
                }

            }
            return uiElements;
        }
        public String SimpleElementsXML()
        {
            string simpleElements = "";
            foreach (POMParsedRawElement item in this)
            {
                simpleElements += item.SimpleElementXML();
            }
            return simpleElements;
        }

        public String PageObjectsXML()
        {
            string pageObjects = "";
            foreach (POMParsedRawElement item in this)
            {
                pageObjects += item.PageObjectXML();
            }
            return pageObjects;
        }

        public virtual String BuildInterfaceNamespace(String Format = "{0}")
        {
            string pageObjects = "";
            foreach (POMParsedRawElement item in this)
            {
                pageObjects += String.Format(Format, item.BuildInterfaceNamespace());
            }
            return pageObjects;
        }
        public virtual String BuildClassNamespace(String Format = "{0}")
        {
            string pageObjects = "";
            foreach (POMParsedRawElement item in this)
            {
                pageObjects += String.Format(Format, item.BuildClassNamespace());
            }
            return pageObjects;
        }

        public virtual String BuildInterface(String Format = "{0}", Predicate<POMParsedRawElement> predicate = null)
        {
            predicate = predicate ?? ((b) => true);
            string pageObjects = "";
            foreach (POMParsedRawElement item in this)
            {
                if (!predicate(item))
                    continue;
                pageObjects += String.Format(Format, item.BuildInterface());
            }
            return pageObjects;
        }
        public virtual String BuildClass(String Format = "{0}", Predicate<POMParsedRawElement> predicate = null)
        {
            predicate = predicate ?? ((b) => true);
            string pageObjects = "";
            foreach (POMParsedRawElement item in this)
            {
                if (!predicate(item))
                    continue;
                pageObjects += String.Format(Format, item.BuildClass());
            }
            return pageObjects;
        }

        public virtual String BuildInterfaceMethods(String Format = "{0}", Predicate<POMParsedRawElement> predicate = null)
        {
            predicate = predicate ?? ((b) => true);
            string pageObjects = "";
            foreach (POMParsedRawElement item in this)
            {
                if (!predicate(item))
                    continue;
                pageObjects += String.Format(Format, item.BuildInterfaceMethods());
            }
            return pageObjects;
        }
        public virtual String BuildClassMethods(String Format = "{0}", Predicate<POMParsedRawElement> predicate = null)
        {
            predicate = predicate ?? ((b) => true);
            string pageObjects = "";
            foreach (POMParsedRawElement item in this)
            {
                if (!predicate(item))
                    continue;
                pageObjects += String.Format(Format, item.BuildClassMethods());
            }
            return pageObjects;
        }

        public virtual String BuildInterfaceFile(String Format = "{0}")
        {
            string pageObjects = "";
            foreach (POMParsedRawElement item in this)
            {
                pageObjects += String.Format(Format, item.BuildInterfaceFile());
            }
            return pageObjects;
        }
        public virtual String BuildClassFile(String Format = "{0}")
        {
            string pageObjects = "";
            foreach (POMParsedRawElement item in this)
            {
                pageObjects += String.Format(Format, item.BuildClassFile());
            }
            return pageObjects;
        }
    }
}
