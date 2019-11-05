using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.Extension;
using Unity.Interception.Interceptors;
using Unity.Policy;
using PSFiddle.Common.Extensions;
using PSFiddle.Common.Unity.BuildStrategies;
using Unity.Events;

namespace PSFiddle.Common.Unity.Extensions
{
    /// <summary>
    /// Used to enable the interception of types that were registered as instances
    /// </summary>
    /// <seealso cref="Unity.Extension.UnityContainerExtension" />
    public class InterceptorInstancesExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.Add(new InterceptableBuildStrategy(), UnityBuildStage.Creation);
        }
        
    }

}
