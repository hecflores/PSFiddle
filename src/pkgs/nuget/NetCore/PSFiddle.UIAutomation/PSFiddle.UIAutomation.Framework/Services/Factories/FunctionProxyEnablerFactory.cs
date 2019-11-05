using MC.Track.TestSuite.Interfaces.Attributes;
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
    [AthenaRegister(typeof(IFunctionProxyEnablerFactory), Model.Enums.AthenaRegistrationType.Singleton)]
    public class FunctionProxyEnablerFactory : IFunctionProxyEnablerFactory
    {
        private readonly IResolver resolver;

        public FunctionProxyEnablerFactory(IResolver resolver)
        {
            this.resolver = resolver;
        }
        public IFunctionProxyEnabler<T> Create<T>()
        {
            return new FunctionProxyEnabler<T>(resolver);
        }
    }
}
