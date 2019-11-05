using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IThreadControlService
    {
        IScoper Mutex(String Name);
        void Wait(String semName);
        void Wait(String semName, int numberOfTimes);
        void Release(String semName);

    }
}
