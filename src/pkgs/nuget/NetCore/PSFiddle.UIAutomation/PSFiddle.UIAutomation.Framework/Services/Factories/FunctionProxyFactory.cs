using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Factories;
using MC.Track.TestSuite.Interfaces.Proxies;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Factories
{
    [AthenaRegister(typeof(IFunctionProxyFactory), Model.Enums.AthenaRegistrationType.Singleton)]
    public class FunctionProxyFactory : IFunctionProxyFactory
    {
        private readonly IEventBasedProxy _eventBasedProxy;
        private readonly IResolver resolver;
        public FunctionProxyFactory(IResolver resolver, IEventBasedProxy eventBasedProxy)
        {
            this._eventBasedProxy = eventBasedProxy;
            this.resolver = resolver;
        }
        public IFunctionProxy Create<T>(MemberInfo method)
        {
            return new FunctionProxy(typeof(T), method, this._eventBasedProxy, resolver);
        }
    }
}
