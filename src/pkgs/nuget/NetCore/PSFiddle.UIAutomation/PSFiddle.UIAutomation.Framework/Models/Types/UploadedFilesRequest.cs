using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class UploadedFilesRequest
    {
        public string UserId { get; set; }

        public long OrganizationKey { get; set; }

        public long UserKey { get; set; }

        public string FileType { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
