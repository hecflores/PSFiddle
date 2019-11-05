using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IFileManager
    {
        String ResolvePath(String RelativeFileName);
        Result<String> GetAllContent(String RelativeFileName);
        void ClearDirectory(String DirectoryName);
        String GenerateFileName(String DirectoryName, String Name, String Extension);
        bool CreateFileIfNotExistsOrContentChanged(String DirectoryName, string Name, String Extension, String content);
    }
}
