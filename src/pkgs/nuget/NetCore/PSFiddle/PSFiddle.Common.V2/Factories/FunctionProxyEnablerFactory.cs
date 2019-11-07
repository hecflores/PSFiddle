using MC.Track.TestSuite.Interfaces.Factories;
using MC.Track.TestSuite.Interfaces.Proxies;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Factories
{
    public class FunctionProxyEnablerFactory : IFunctionProxyEnablerFactory
    {
        private readonly IFunctionProxyFactory factory;

        public FunctionProxyEnablerFactory(IFunctionProxyFactory factory)
        {
            this.factory = factory;
        }
        public IFunctionProxyEnabler<T> Create<T>()
        {
            return new FunctionProxyEnabler<T>(factory);
        }
    }
}
