

using System;
using System.Collections.Generic;

namespace MC.Track.TestSuite.Model.Types
{
    public class FileUploadResponse
    {
        public String CorrelationID { get; set; }
        public String FileName { get; set; }
        public FileUploadResponse()
        {
            ErrorMessages = new List<string>();
        }

        public bool Success { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
