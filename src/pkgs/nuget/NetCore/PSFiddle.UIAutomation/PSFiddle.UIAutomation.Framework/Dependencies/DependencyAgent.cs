using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Toolkit.Extensions;
using MC.Track.TestSuite.Interfaces.Config;
using AE.Net.Mail;
using MC.Track.TestSuite.Interfaces.Services.Functional;
using MC.Track.TestSuite.UI.Types;
using MC.Track.Shared;
using PSFiddle.UIAutomation.Framework.Shared;
using System.IO;
using MC.Track.TestSuite.Interfaces.Dependencies;

namespace MC.Track.TestSuite.Toolkit.Dependencies
{
    public class Dependencies : IDependencies
    {
        private readonly IMatchingDependencies matchingDependencies;
        private readonly IEmailDependencies emailDependencies;
        private readonly IBrowserDependencies browserDependencies;
        private readonly IOrganizationDependencies organizationDependencies;
        private readonly IFilesDependencies filesDependencies;
        private readonly IStateManagment stateManagment;
        private readonly IRunnerDependencies runnerDependencies;
        private readonly IResolver resolver;
        public Dependencies(IResolver resolver)
        {
            this.emailDependencies = resolver.Resolve<IEmailDependencies>();
            this.browserDependencies = resolver.Resolve<IBrowserDependencies>();
            this.organizationDependencies = resolver.Resolve<IOrganizationDependencies>();
            this.stateManagment = resolver.Resolve<IStateManagment>();
            this.filesDependencies= resolver.Resolve<IFilesDependencies>();
            this.matchingDependencies = resolver.Resolve<IMatchingDependencies>();
            this.runnerDependencies = resolver.Resolve<IRunnerDependencies>();
            this.resolver = resolver;
        }
        public IResolver Resolver() { return this.resolver; }
        public IMatchingDependencies Matching() { return matchingDependencies; }
        public IEmailDependencies Email() { return emailDependencies; }
        public IBrowserDependencies Browser() { return browserDependencies; }
        public IOrganizationDependencies Organization() { return organizationDependencies; }
        public IFilesDependencies Files() { return filesDependencies; }
        public IRunnerDependencies Runner() { return runnerDependencies; }
        public bool Has<T>(String Parm)
        {
            return stateManagment.Has<T>(Parm);
        }
        public T Get<T>(String Parm)
        {
            return stateManagment.Get<T>(Parm);
        }
        public void Set<T>(String Parm, T obj)
        {
            stateManagment.Set<T>(Parm, obj);
        }

        
    }
}
