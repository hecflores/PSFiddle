using MC.Track.TestSuite.Interfaces.Dependencies.Builders.Shared;
using MC.Track.TestSuite.Model.Types;
using MC.Track.TestSuite.UI.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Dependencies.Builders
{
    public interface IFilesBuilder : IBaseListBuilder<GenericFileType, IFilesBuilder>, IDependencyBuilder
    {
        IFilesBuilder Create(String Content, Action<IFileBuilder> buildIt = null);
        IFilesBuilder UseFile(string FilePath, Action<IFileBuilder> buildIt = null);
    }
}
