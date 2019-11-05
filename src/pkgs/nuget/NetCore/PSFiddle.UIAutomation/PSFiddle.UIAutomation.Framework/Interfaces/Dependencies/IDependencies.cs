using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Dependencies
{
    public interface IDependencies : IDependency
    {
        IResolver Resolver();
        IMatchingDependencies Matching();
        IEmailDependencies Email();
        IBrowserDependencies Browser();
        IOrganizationDependencies Organization();
        IRunnerDependencies Runner();
        T Get<T>(String Parm);
        void Set<T>(String Parm, T obj);
        bool Has<T>(String Parm);
        IFilesDependencies Files();
    }
}
