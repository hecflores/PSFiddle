using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MC.Track.TestSuite.Model.Helpers
{
    public class XmlHelper
    {
        public XmlDocument xmlDoc { get; set; }
        public XmlNamespaceManager nsmgr { get; set; }
        public object XMLConvert { get; private set; }

        public XmlHelper(string filePath)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            var namespaceName = "ns";
            var namespacePrefix = string.Empty;
            if (xmlDoc.DocumentElement.Attributes != null)
            {
                var xmlns = xmlDoc.DocumentElement.Attributes["xmlns"];
                var xmlnscac = xmlDoc.DocumentElement.Attributes["xmlns:cac"];
                var xmlnscbc = xmlDoc.DocumentElement.Attributes["xmlns:cbc"];
                if (xmlns != null)
                {
                    nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                    nsmgr.AddNamespace(namespaceName, xmlns.Value);
                    nsmgr.AddNamespace("cac", xmlnscac.Value);
                    nsmgr.AddNamespace("cbc", xmlnscbc.Value);
                    namespacePrefix = namespaceName + ":";
                }
            }
        }

        public string getValue(string key)
        {
            string value = string.Empty;
            string xpathKey = key.Replace("/Invoice", "/ns:Invoice");
            try
            {
                XmlNode node = xmlDoc.DocumentElement.SelectSingleNode(xpathKey, nsmgr);
                if (node != null)
                {
                    value = node.InnerText;
                }
                return value;
            }
            catch
            {
                return value;
            }
        }

        public void UpdateValue(string key, string value)
        {
            string xpathKey = key.Replace("/Invoice", "/ns:Invoice");
            XmlNode node = xmlDoc.DocumentElement.SelectSingleNode(xpathKey, nsmgr);
            if (node != null)
            {
                node.InnerText = value;

            }
            //return value;
        }


    }
}
