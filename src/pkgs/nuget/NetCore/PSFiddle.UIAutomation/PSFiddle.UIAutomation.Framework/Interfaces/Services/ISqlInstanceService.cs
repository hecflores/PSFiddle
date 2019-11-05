using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface ISqlInstanceService
    {
        ISqlConnectionService Connect();
    }
}
