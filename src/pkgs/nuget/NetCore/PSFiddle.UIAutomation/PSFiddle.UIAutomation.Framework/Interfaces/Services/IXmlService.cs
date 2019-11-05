using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IXmlService
    {
        string GetValue(string key);
        void UpdateValue(string key, string value);
    }
}
