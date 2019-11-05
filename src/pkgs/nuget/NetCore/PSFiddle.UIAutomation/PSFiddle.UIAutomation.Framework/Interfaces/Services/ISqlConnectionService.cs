using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface ISqlConnectionService : IDisposable
    {
        ISqlCommandService CreateCommand(String CommandStr);
        void Open();
        DataSet Fill(string CommandStr, string param, string dataSetName);
        DataTable Fill(string CommandStr);
    }
}
