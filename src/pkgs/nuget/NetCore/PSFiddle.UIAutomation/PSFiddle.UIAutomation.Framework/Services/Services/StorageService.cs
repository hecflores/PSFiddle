using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Attributes;
using Microsoft.WindowsAzure.Storage.Blob;
using MC.Track.TestSuite.Services.Util;
using MC.Track.TestSuite.Interfaces.Util;
using Microsoft.ServiceBus.Messaging;
using MC.Track.TestSuite.Model.Types.CoreServices;
using static MC.Track.TestSuite.Model.Helpers.JSONResponseHelper;
using Newtonsoft.Json;
using MC.Track.TestSuite.Interfaces.Config;

namespace MC.Track.TestSuite.Services.Services
{
    public class StorageService : IStorageService
    {
        private readonly string _storageConnectionString;
        private readonly string _serviceBusConnectionString;
        private readonly IMIMEHelper iMIMEHelper;
        public StorageService(String ConnectionString, IResolver resolver)
        {
            _storageConnectionString = ConnectionString;
            _serviceBusConnectionString = resolver.Resolve<IConfiguration>().CoreServicesServiceBusConnectionString;
            this.iMIMEHelper = resolver.Resolve<IMIMEHelper>();
        }

      
        public void ExtractItemsFromUri(Uri BlobUri, out String Container, out String RelativePath)
        {
            var AbsolutePath = BlobUri.LocalPath;
            var Matcher = Regex.Match(AbsolutePath, @"^\/(.*?)\/(.*)$");
            Container = Matcher.Groups[1].Value;
            RelativePath = Matcher.Groups[2].Value;
        }

        public async Task<FileInfo> DownloadFile(Uri BlobUrl)
        {
            Guid guid = Guid.NewGuid();
            FileInfo file = new FileInfo(Path.GetTempFileName());
            using (var stream = file.Create())
            {
                await GetStreamFromStorageAcount(stream, BlobUrl);
            }

            return file;
        }
        public async Task<Uri> AppendToFile(Uri BlobUrl, String LocalFilePath, bool createEmpty = false)
        {
            String Container, RelativePath;
            ExtractItemsFromUri(BlobUrl, out Container, out RelativePath);

            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            var BlobClient = storageAccount.CreateCloudBlobClient();
            var ContainerReference = BlobClient.GetContainerReference(Container);
            var BlobReference = ContainerReference.GetAppendBlobReference(RelativePath);

            if (createEmpty) await BlobReference.CreateOrReplaceAsync();
            await BlobReference.AppendFromFileAsync(LocalFilePath);

            return BlobUrl;
        }
        public async Task<Uri> AppendToFile(String Container, String RelativeBlobPath, String LocalFilePath)
        {
            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            Uri BlobUri = new Uri(storageAccount.BlobStorageUri.PrimaryUri, Container + "/" + Guid.NewGuid().ToString() + "/" + RelativeBlobPath);

            String RelativePath;
            ExtractItemsFromUri(BlobUri, out Container, out RelativePath);

            var BlobClient = storageAccount.CreateCloudBlobClient();
            var ContainerReference = BlobClient.GetContainerReference(Container);
            var BlobReference = ContainerReference.GetAppendBlobReference(RelativePath);

            var contentType = iMIMEHelper.GetMimeType(Path.GetExtension(LocalFilePath));
            if (contentType != null)
                BlobReference.Properties.ContentType = contentType;

            await BlobReference.CreateOrReplaceAsync();

            var sas = BlobReference.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                SharedAccessStartTime = DateTime.UtcNow,
                SharedAccessExpiryTime = DateTime.UtcNow.AddYears(100),
                Permissions = SharedAccessBlobPermissions.Read,
            });
            var newURL = new Uri(BlobReference.Uri, sas);

            return await AppendToFile(newURL, LocalFilePath);
        }
        public async Task<Uri> UploadFile(String Container, String RelativeBlobPath, String LocalFilePath, bool SkipDuplicatePrevention = false)
        {
            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            Guid corId = Guid.NewGuid();
            Uri BlobUri = new Uri(storageAccount.BlobStorageUri.PrimaryUri, Container + (SkipDuplicatePrevention?"":("/" + Convert.ToString(corId))) + "/" + RelativeBlobPath);
           
            FileInfo uploadFile = new FileInfo(LocalFilePath);
            Uri url = await UploadFile(BlobUri, LocalFilePath);
            
            
            return url;
        }
       
        public async Task<Uri> UploadFile(String Container, String FilePath)
        {
            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            Uri BlobUri = new Uri(storageAccount.BlobStorageUri.PrimaryUri, Container + "/" + Guid.NewGuid().ToString());

            return await UploadFile(BlobUri, FilePath);
        }
        public async Task<Uri> UploadFile(Uri BlobUrl, String FilePath)
        {
            FileInfo file = new FileInfo(FilePath);
            if (!file.Exists)
            {
                throw new Exception("File " + FilePath + " was not found");
            }

            using (var stream = file.OpenRead())
            {
                return await SaveStreamToStorageAccount(stream, BlobUrl, iMIMEHelper.GetMimeType(Path.GetExtension(FilePath)));
            }
        }
        public static object LockObject = new object();
        public async Task<Uri> SaveStreamToStorageAccount(Stream stream, Uri BlobUrl, String ContentType = null)
        {
            String Container, RelativePath;
            ExtractItemsFromUri(BlobUrl, out Container, out RelativePath);

            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            var BlobClient = storageAccount.CreateCloudBlobClient();
            var ContainerReference = BlobClient.GetContainerReference(Container);

            var BlobReference = ContainerReference.GetBlockBlobReference(RelativePath);
            int count = 0;

            lock (LockObject)
            {
                // Fix Duplicates
                if (ContentType != null)
                {
                    BlobReference.Properties.ContentType = ContentType;
                }
                BlobReference.UploadFromStream(stream);
                stream.Flush();
                stream.Close();
            }
            var sas = BlobReference.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                SharedAccessStartTime = DateTime.UtcNow,
                SharedAccessExpiryTime = DateTime.UtcNow.AddYears(100),
                Permissions = SharedAccessBlobPermissions.Read,
            });
            return new Uri(BlobReference.Uri, sas);
        }
        public async Task<Stream> GetStreamFromStorageAcount(Stream stream, Uri BlobUrl)
        {
            String Container, RelativePath;
            ExtractItemsFromUri(BlobUrl, out Container, out RelativePath);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageConnectionString);

            var BlobClient = storageAccount.CreateCloudBlobClient();
            var ContainerReference = BlobClient.GetContainerReference(Container);
            var BlobReference = ContainerReference.GetBlockBlobReference(RelativePath);

            if (!await BlobReference.ExistsAsync())
            {
                throw new Exception($"Blob not found '{BlobUrl.ToString()}'");
            }
            lock (LockObject)
            {
                BlobReference.DownloadToStream(stream);
                stream.Flush();
            }
            return stream;

        }

        public Uri BuildUri(string Container, string FilePath)
        {
            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            var BlobClient = storageAccount.CreateCloudBlobClient();
            var ContainerReference = BlobClient.GetContainerReference(Container);


            var BlobReference = ContainerReference.GetBlockBlobReference(FilePath);

            return BlobReference.Uri;
        }

        
    }


}


