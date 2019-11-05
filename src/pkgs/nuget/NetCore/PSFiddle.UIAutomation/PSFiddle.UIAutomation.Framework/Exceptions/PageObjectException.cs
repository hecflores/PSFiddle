using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSFiddle.UIAutomation.Framework.Exceptions
{
    [Serializable]
    public class PageObjectException : Exception
    {
        private readonly string pageName;

        public PageObjectException(String PageName, string message) : base(message)
        {
            pageName = PageName;
        }

        public PageObjectException(String PageName, string message, Exception innerException) : base(message, innerException)
        {
        }

        public string PageName => pageName;
    }
}
