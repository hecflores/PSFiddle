using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.EventArgs
{
    public class PreInvokedEventBasedProxyEventArgs : System.EventArgs
    {
        public MethodBase MethodBase { get; set; }
        public Object[] OutputVariables { get; set; }
        public Exception Exception { get; set; }
        public String FunctionName { get; set; }
        public Type ClassType { get; set; }
        public Object TargetObj { get; set; }
        public Func<String, Object> GetArgmentByName { get; set; }
        public Func<int, Object> GetArgmentByIndex { get; set; }
        public Object ReturnObj { get; set; }
        public bool Execute { get; set; }

    }
}
