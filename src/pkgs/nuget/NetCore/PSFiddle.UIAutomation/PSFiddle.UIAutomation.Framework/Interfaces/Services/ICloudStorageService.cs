using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface ICloudStorageService
    {
        string DownloadFile(string containerName, string fileName, string prefix, string category, bool isValid = true);
        bool UploadFile(string containerName, string fileName, string filePath);
    }
}
