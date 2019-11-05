using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class GenericFileType
    {
        public String FilePath { get; set; }
        public String FileName { get; set; }
        public List<FileMetadata> Uploaded { get; set; }
    }
}
