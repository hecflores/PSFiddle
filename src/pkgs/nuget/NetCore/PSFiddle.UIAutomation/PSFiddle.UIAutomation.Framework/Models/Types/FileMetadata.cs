using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class FileMetadata
    {
        public long? AuditHistoryKey { get; set; }
        public long? FileMetadataKey { get; set; }
        public long? OrganizationKey { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string CorrelationID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public long? ModifiedByKey { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long? CreatedByKey { get; set; }
    }
}
