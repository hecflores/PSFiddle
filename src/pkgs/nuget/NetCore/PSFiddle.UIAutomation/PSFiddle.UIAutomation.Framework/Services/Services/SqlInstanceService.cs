using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class SqlInstanceService : ISqlInstanceService
    {
        String _connectionString;

        public SqlInstanceService(String connectionString)
        {
            _connectionString = connectionString;
        }
        public ISqlConnectionService Connect()
        {
            ISqlConnectionService service = new SqlConnectionService(_connectionString);
            return service;
        }


    }
}
