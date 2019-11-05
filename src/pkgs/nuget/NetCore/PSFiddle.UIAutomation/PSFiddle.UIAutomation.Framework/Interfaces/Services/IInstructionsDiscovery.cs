using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IInstructionsDiscovery
    {
        void Setup();
        void Find(String InstructionName);
    }
}
