using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IProxyInterceptor
    {
        void Register<T>(String Method, Func<IMethodInvocation, GetNextInterceptionBehaviorDelegate, IMethodReturn> invoker);
    }
}
