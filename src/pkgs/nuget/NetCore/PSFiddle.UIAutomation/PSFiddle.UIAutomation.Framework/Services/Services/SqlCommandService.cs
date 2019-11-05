using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace MC.Track.TestSuite.Services.Services
{
    public class SqlCommandService : ISqlCommandService, IDisposable
    {
        private SqlCommand _underlineCommand;
        private readonly string command;
        private readonly SqlConnection _underlineSqlConnection;

        public SqlCommandService(String Command, SqlConnection connection)
        {
            _underlineCommand = new SqlCommand(Command, connection);
            _underlineCommand.CommandTimeout = 0; // Wait Forever...
            command = Command;
            this._underlineSqlConnection = connection;
        }
        public CommandType CommandType { get => _underlineCommand.CommandType; set => _underlineCommand.CommandType = value; }

        public SqlParameter AddParameter(SqlParameter parameter)
        {
            if (parameter.Value == null)
            {
                parameter.Value = DBNull.Value;
            }
            _underlineCommand.Parameters.Add(parameter);
            return parameter;
        }

        public void Dispose()
        {
            _underlineCommand.Dispose();
        }

        public int ExecuteNonQuery()
        {
            //XConsole.WriteLine($"Command: {_underlineCommand.CommandText} Executing");

            return _underlineCommand.ExecuteNonQuery();
        }
        public DataSet Fill()
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command, _underlineSqlConnection);
            var ds = new DataSet();
            sqlDataAdapter.Fill(ds);
            return ds;
        }

        public async Task ExecuteNonQueryAsync()
        {
            await _underlineCommand.ExecuteNonQueryAsync();
        }
        public IDataReader ExecuteReader()
        {
            //XConsole.WriteLine($"Command: {_underlineCommand.CommandText} Executing");
            return _underlineCommand.ExecuteReader();
        }
        public async Task<SqlDataReader> ExecuteReaderAsync()
        {
            return await _underlineCommand.ExecuteReaderAsync();
        }
        public object ExecuteScalar()
        {
            //XConsole.WriteLine($"Command: {_underlineCommand.CommandText} Executing");
            return _underlineCommand.ExecuteScalar();
        }
    }
}
