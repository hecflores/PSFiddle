using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface ISqlCommandService : IDisposable
    {

        CommandType CommandType { get; set; }
        SqlParameter AddParameter(SqlParameter parameter);

        //Different Sql Parameters
        DataSet Fill();
        IDataReader ExecuteReader();
        Task<SqlDataReader> ExecuteReaderAsync();
        Task ExecuteNonQueryAsync();
        Object ExecuteScalar();
        int ExecuteNonQuery();

    }
}
