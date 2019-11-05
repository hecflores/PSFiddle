using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSFiddle.UIAutomation.Framework.Models.Types
{
    public class StackTraceType
    {
        public Type ClassType { get; set; }
        public String FullClassName { get; set; }
        public String MethodName { get; set; }
        public List<String> MethodParams { get; set; } = new List<string>();
        public String FileName { get; set; }
        public int LineNumber { get; set; }
    }
}
