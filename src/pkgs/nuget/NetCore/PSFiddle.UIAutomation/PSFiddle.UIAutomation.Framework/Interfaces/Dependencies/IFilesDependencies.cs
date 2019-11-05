using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Dependencies.Builders;
using MC.Track.TestSuite.Model.Types;

namespace MC.Track.TestSuite.Interfaces.Dependencies
{
    public interface IFilesDependencies : IDependency
    {
        IFilesBuilder Factory();
    }
}
