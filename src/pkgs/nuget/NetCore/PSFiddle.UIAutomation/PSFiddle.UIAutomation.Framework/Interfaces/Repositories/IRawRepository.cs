using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Repositories
{
    public interface IRawRepository
    {
        DataSet ExecuteReaderQuery(string query, String failMessage = null);
        string ExecuteScalarQuery(string command);
        bool ExecuteReaderQuery(String query, Action<SqlDataReader> callback);

        DataSet FillTheDataSetBasedOnTables(Dictionary<string, string> queries, string dataSetName);

        DataTable FillTheDataTableBasedOnQuery(string command);
    }
}
