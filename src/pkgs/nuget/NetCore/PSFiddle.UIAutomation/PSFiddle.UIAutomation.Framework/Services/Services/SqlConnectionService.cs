using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class SqlConnectionService : ISqlConnectionService
    {
        private SqlConnection _underlineSqlConnection;

        public SqlConnectionService(String ConnectionString)
        {
            _underlineSqlConnection = new SqlConnection(ConnectionString);
        }

        public ISqlCommandService CreateCommand(string CommandStr)
        {
            return new SqlCommandService(CommandStr, _underlineSqlConnection);
        }

        public DataSet Fill(string CommandStr,string param,string dataSetName)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(CommandStr, _underlineSqlConnection);
            var ds = new DataSet(dataSetName);
            sqlDataAdapter.FillSchema(ds, SchemaType.Source, param);
            sqlDataAdapter.Fill(ds, param);
            return ds;
        }

        public DataTable Fill(string CommandStr)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(CommandStr, _underlineSqlConnection);
            var dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            return dt;
        }



        public void Dispose()
        {
            _underlineSqlConnection.Close();
            _underlineSqlConnection.Dispose();
        }

        public void Open()
        {
            _underlineSqlConnection.Open();
        }
    }
}
