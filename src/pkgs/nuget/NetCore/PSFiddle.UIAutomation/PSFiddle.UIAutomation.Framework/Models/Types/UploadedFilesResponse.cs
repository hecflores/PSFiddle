using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class UploadedFilesResponse
    {
        public UploadedFilesResponse()
        {
            ErrorMessages = new List<string>();
        }

        public bool Success { get; set; }

        public List<string> ErrorMessages { get; set; }

        public IList<FileMetadata> Files { get; set; }

        public int TotalRecordCount { get; set; }
    }
}
