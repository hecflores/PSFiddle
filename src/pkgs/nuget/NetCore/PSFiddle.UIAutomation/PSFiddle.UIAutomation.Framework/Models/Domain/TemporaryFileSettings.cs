using MC.Track.TestSuite.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Domain
{
    public class TemporaryFileSettings
    {
        public String InitialContent { get; set; }
        public String Title { get; set; }
        public String Extension { get; set; }
        public String Name { get; set; }
        public TemporaryFileLifetimes FileLifetime { get; set; } = TemporaryFileLifetimes.RESOLVER_LIFETIME;
        public bool UploadToTestCloadStorageWhenFinished { get; set; }
        public bool SaveFileWhenFinished { get; set; }
    }
}
