using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Toolkit.Dependencies.Shared
{
    public class FactoryHelper
    {
        public static T Create<T>(IResolver resolver, String Subtype)
        {
            var factory = resolver.Resolve<IMagicFactory<T>>(Subtype);
            return factory.Create();
        }
    }
}
