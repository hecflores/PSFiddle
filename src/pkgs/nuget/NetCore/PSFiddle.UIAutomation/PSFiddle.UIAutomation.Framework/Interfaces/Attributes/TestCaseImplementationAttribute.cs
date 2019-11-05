using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSFiddle.UIAutomation.Framework.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TestCaseImplementationAttribute : Attribute
    {
        public TestCaseImplementationAttribute(int testCaseID)
        {

        }
    }
}
