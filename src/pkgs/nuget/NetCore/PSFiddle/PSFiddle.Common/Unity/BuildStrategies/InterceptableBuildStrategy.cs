using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Builder;
using Unity.Builder.Strategy;
using PSFiddle.Common.Extensions;
using Unity.Registration;

namespace PSFiddle.Common.Unity.BuildStrategies
{

    public class InterceptableBuildStrategy : BuilderStrategy
    {
        public override bool RequiredToBuildType(IUnityContainer container, INamedType registration, params InjectionMember[] injectionMembers)
        {
            return true;
        }
        public override bool RequiredToResolveInstance(IUnityContainer container, INamedType registration)
        {
            return true;
        }
        public override void PostBuildUp(IBuilderContext context)
        {
            if (context.Existing != null && context.OriginalBuildKey.Type.CanIntercept())
            {
                context.Existing = context.Container.MakeInterceptable(context.OriginalBuildKey.Type, context.Existing);
                context.BuildComplete = true;
            }

        }
    }
}
