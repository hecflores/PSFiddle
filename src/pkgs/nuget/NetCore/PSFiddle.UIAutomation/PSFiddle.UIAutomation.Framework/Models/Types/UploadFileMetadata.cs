using System;

namespace MC.Track.TestSuite.Model.Types
{
    public class UploadFileMetadata
    {
        public long? FileMetadataKey { get; set; }
        public long? OrganizationKey { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public string CorrelationID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public long? ModifiedByKey { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long? CreatedByKey { get; set; }

    }
}