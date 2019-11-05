
using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Interfaces.Dependencies.Builders;
using MC.Track.TestSuite.Interfaces.Dependencies.Builders.Shared;
using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Pages.Shared;
using MC.Track.TestSuite.Interfaces.Proxies;
using MC.Track.TestSuite.Interfaces.Repositories;
using MC.Track.TestSuite.Interfaces.Services.Functional;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace MC.Track.TestSuite.Proxies
{
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    public class FrameworkLogger : IFrameworkLogger, IInterceptionBehavior
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private ILogger logger;
        private FrameworkType[] types;
        public FrameworkLogger(FrameworkType[] types, ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException("logger");
            this.types = types ?? throw new ArgumentNullException("logger");
        }

        public bool WillExecute => true;

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            IMethodReturn originalMethod;
            if(input.Target is ILogger)
            {
                return getNext().Invoke(input, getNext); 
            }
            //if(this.types.Contains(FrameworkType.DEPENDENCIES) && !(input.GetType().GetInterfaces().Contains(typeof(IDependency))))
            //    return getNext().Invoke(input, getNext);
            //if (this.types.Contains(FrameworkType.DEPENDENCIES_BUILDERS) && !(input.GetType().GetInterfaces().Contains(typeof(IBaseBuilder<,>))))
            //    return getNext().Invoke(input, getNext);
            //if (this.types.Contains(FrameworkType.DEPENDENCIES_FUNCTIONAL_SERVICES) && !(input.GetType().GetInterfaces().Contains(typeof(IFunctionalService))))
            //    return getNext().Invoke(input, getNext);
            //if (this.types.Contains(FrameworkType.DEPENDENCIES_REPOSITORY) && !(input.GetType().GetInterfaces().Contains(typeof(IFrameworkBaseRepository))))
            //    return getNext().Invoke(input, getNext);
            //if (this.types.Contains(FrameworkType.DEPENDENCIES_RAW_DATA_FUNCTIONS) && !(input.GetType().GetInterfaces().Contains(typeof(IBaseRepository<>))))
            //    return getNext().Invoke(input, getNext);
            //if (this.types.Contains(FrameworkType.PAGES) && !(input.GetType().GetInterfaces().Contains(typeof(IPageBase))))
            //    return getNext().Invoke(input, getNext);
            //if (this.types.Contains(FrameworkType.PAGE_FUNCTIONALALITY) && !(input.GetType().GetInterfaces().Contains(typeof(IPageBase))))
            //    throw new Exception("Page Functionality logging no implmented yet");
            //if (this.types.Contains(FrameworkType.PAGE_BROWSER_FUNCTIONALITY) && !(input.GetType().GetInterfaces().Contains(typeof(ITrackBrowser))))
            //    throw new Exception("Page Browser logging not implmented yet");
            //if (this.types.Contains(FrameworkType.PAGE_RAW_SELENIUM_FUNCTIONALITY))
            //    throw new Exception("Selenium Logging not implmented yet");
            
            using (this.logger.Section($"{input.MethodBase.Name}"))
            {
                originalMethod = getNext().Invoke(input, getNext);
            }

            //this.logger.LogTrace($"Finished Executing Method: {input.MethodBase.ToString()}. {(originalMethod.Exception == null ? "Success" : "Failed")}");
            if (originalMethod.Exception != null)
            {
                this.logger.LogError(originalMethod.Exception);
            }

            return originalMethod;
        }
    }
}
