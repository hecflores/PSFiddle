
namespace MC.Track.TestSuite.Model.Types
{
    public class FileUploadRequest
    {
        /// <summary>
        /// Name of file 
        /// </summary>
        public string FileName { get; set; }

        public string FileType { get; set; }

        public long OrganizationKey { get; set; }

        public string Status { get; set; }

        /// <summary>
        /// User who uploaded file
        /// </summary>
        public string UploadedBy { get; set; }

        public long AlphaUserKey { get; set; }

        public string StatusMessage { get; set; }

        /// <summary>
        /// Records of file
        /// </summary>
        public string FileData { get; set; }
    }
}
