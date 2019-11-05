using MC.Track.TestSuite.Interfaces.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Factories
{
    public interface IFunctionProxyFactory
    {
        IFunctionProxy Create<T>(MemberInfo method);
    }
}
