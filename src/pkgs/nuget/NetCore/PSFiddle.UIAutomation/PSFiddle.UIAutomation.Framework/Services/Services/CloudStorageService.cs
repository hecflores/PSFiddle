using MC.Track.TestSuite.Interfaces.Services;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;

namespace MC.Track.TestSuite.Services.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _cloudBlobContainer;
        private CloudBlockBlob _cloudBlockBlob;
        public CloudStorageService(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
            // Create the blob client.
            _blobClient = _storageAccount.CreateCloudBlobClient();
        }

        public bool UploadFile(string containerName, string fileName, string filePath)
        {
            try
            {
                _cloudBlobContainer = _blobClient.GetContainerReference(containerName);

                // Retrieve reference to a blob named "myblob".
                _cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(fileName);

                // Create or overwrite the "myblob" blob with contents from a local file.
                using (var fileStream = System.IO.File.OpenRead(filePath))
                {
                    _cloudBlockBlob.UploadFromStream(fileStream);
                }
                return true;

            }
            catch
            {
                return false;
            }
        }

        public string DownloadFile(string containerName, string fileName, string prefix, string category, bool isValid = true)
        {
            string fileText = string.Empty;
            string fileExtention = string.Empty;
            switch (category)
            {
                case "FileFormat":
                    fileExtention = "FileValidatorResponse.json";
                    break;
                case "OnBehalf":
                    if (isValid)
                        fileExtention = "PIF_PIFResponse.json";
                    else
                        fileExtention = "InvoiceResponse.json";
                    break;
                case "DirectPif":
                    fileExtention = "PIFResponse.json";
                    break;
                case "Invoice":
                    fileExtention = "InvoiceResponse.json";
                    break;
                case "PaymentResponse":
                    fileExtention = "PIF_PaymentResponse.json";
                    break;
            }


            string blobName = string.Format("{0}_{1}_{2}", prefix, fileName, fileExtention);
            // Retrieve reference to a previously created container.

            try
            {
                _cloudBlobContainer = _blobClient.GetContainerReference(containerName);

                _cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(blobName);
                for (int time = 0; time < 180000; time = time + 10000)
                {
                    if (!string.IsNullOrEmpty(_cloudBlockBlob.Name))
                    {
                        if (_cloudBlockBlob.Name.Equals(blobName))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                fileText = _cloudBlockBlob.DownloadText();
                                _cloudBlockBlob.DownloadToStream(memoryStream);
                                fileText = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                            }
                            return fileText;

                        }
                    }
                }
                return fileText;
            }
            catch
            {

                throw new Exception(blobName + " Not Found in response topic");
            }

        }


    }
}
