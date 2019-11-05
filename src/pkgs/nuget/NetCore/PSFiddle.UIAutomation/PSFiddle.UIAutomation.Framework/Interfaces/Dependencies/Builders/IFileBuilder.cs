using MC.Track.TestSuite.Interfaces.Dependencies.Builders.Shared;
using MC.Track.TestSuite.Model.Enums;
using MC.Track.TestSuite.Model.Types;
using MC.Track.TestSuite.UI.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Dependencies.Builders
{
    public interface IUploadedFileBuilder 
    {
        
        IFileBuilder Done();
    }

    public interface IFileBuilder : IBaseBuilder<GenericFileType, IFileBuilder>, IDependencyBuilder, IUploadedFileBuilder
    {

        IFileBuilder UsingUser(UserType user);
        IFileBuilder MakeCopy();
        IFileBuilder Rename(String Name);
        IFileBuilder RunAsPowershellContent(int timeout = 1000000);
        IFileBuilder RenameToRandomName(String Extension);
        IFileBuilder DeleteOnExit();
        IFileBuilder DeleteNow();
        IFileBuilder TokenReplace(String TokenName, String TokenValue);
        IFileBuilder ExecuteAsSqlStatment(Action<DataSet> callback = null);
    }
}
