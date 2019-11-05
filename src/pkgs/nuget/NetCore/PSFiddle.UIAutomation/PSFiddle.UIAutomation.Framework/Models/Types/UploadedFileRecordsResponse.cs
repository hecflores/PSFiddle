using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class UploadedFileRecordsResponse
    {
        public UploadedFileRecordsResponse()
        {
            ErrorMessages = new List<string>();
        }

        public bool Success { get; set; }

        public IList<UploadedFileRecord> Records { get; set; }

        public List<string> ErrorMessages { get; set; }
    }
}
