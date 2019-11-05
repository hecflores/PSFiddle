using MC.Track.TestSuite.Model.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Resources
{
    public interface ITemporaryFileResource
    {
        void UpdateUri(Uri uri);
        FileInfo File { get; }
        TemporaryFileSettings Settings { get;  }
        void AppendLine(String text);
        void Append(String text);
        Uri Flush();
    }
}
