using MC.Track.TestSuite.Model.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AthenaMethodInterceptor : Attribute
    {
        public AthenaMethodInterceptor()
        {

        }
        public void Intercept(PreInvokedEventBasedProxyEventArgs args)
        {

        }
    }
}
