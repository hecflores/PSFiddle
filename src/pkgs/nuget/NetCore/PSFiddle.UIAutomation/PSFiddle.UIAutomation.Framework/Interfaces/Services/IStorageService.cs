using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IStorageService
    {
        Task<FileInfo> DownloadFile(Uri BlobUrl);
        Task<Uri> AppendToFile(String Container, String RelativeBlobPath, String LocalFilePath);
        Task<Uri> AppendToFile(Uri BlobUrl, String LocalFilePath, bool createEmty = false);
        Task<Uri> UploadFile(String Container, String RelativeBlobPath, String LocalFilePath, bool SkipDuplicatePrevention = false);
        Task<Uri> UploadFile(Uri BlobUrl, String FilePath);
        Task<Uri> UploadFile(String Container, String FilePath);
        Uri BuildUri(String Container, String FilePath);
        Task<Uri> SaveStreamToStorageAccount(Stream stream, Uri BlobUrl, String ContentType = null);
        Task<Stream> GetStreamFromStorageAcount(Stream stream, Uri BlobUrl);
    }
}
