using MC.Track.TestSuite.Interfaces.Repositories;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Repository
{
    public class RawRepository : IRawRepository
    {
        private readonly ISqlInstanceService sqlInstanceService;
        public RawRepository(ISqlInstanceService sqlInstanceService)
        {
            this.sqlInstanceService = sqlInstanceService;
        }
        public DataSet ExecuteReaderQuery(string query, String failMessage = null)
        {
            try
            {
                using (var connection = this.sqlInstanceService.Connect())
                {
                    using (var cmd = connection.CreateCommand(query))
                    {
                        connection.Open();
                        return cmd.Fill();
                    }
                }
            }
            catch (Exception e)
            {
                failMessage = failMessage == null ? e.Message : failMessage;
                XConsole.WriteLine($"************** Connect Database Error *************{query}\nMessage:{e.Message}\nStacktrace:{e.StackTrace}");
                throw new Exception($"DB Error: {failMessage}", e);
            }

        }
        public bool ExecuteReaderQuery(string query, Action<SqlDataReader> callback)
        {
            throw new NotImplementedException();
        }

        public string ExecuteScalarQuery(string command)
        {
            try
            {
                using (var connection = this.sqlInstanceService.Connect())
                {
                    using (var cmd = connection.CreateCommand(command))
                    {
                        connection.Open();
                        var value = cmd.ExecuteScalar();
                        return value != null ? value.ToString() : String.Empty;
                    }
                }
            }
            catch (Exception e)
            {
                XConsole.WriteLine($"************** Connect Database Error *************\nMessage:{e.Message}\nStacktrace:{e.StackTrace}");
                throw new Exception($"Db Error - {e.Message}");
            }


        }

        public DataSet FillTheDataSetBasedOnTables(Dictionary<string, string> queries,string dataSetName)
        {

            try
            {
                DataSet ds = null;
                foreach (var q in queries)
                {
                    using (var connection = this.sqlInstanceService.Connect())
                    {
                        connection.Fill(q.Value, q.Key, dataSetName);

                    }

                }
                return ds;

            }
            catch (Exception e)
            {
                XConsole.WriteLine($"************** Connect Database Error *************\nMessage:{e.Message}\nStacktrace:{e.StackTrace}");
                throw new Exception($"Failed to perform Query'", e);
            }
        }

        public DataTable FillTheDataTableBasedOnQuery(string command)
        {
            try
            {
                using (var connection = this.sqlInstanceService.Connect())
                {
                    return connection.Fill(command);                  
                }
            }

            catch (Exception e)
            {
                XConsole.WriteLine($"************** Connect Database Error *************\nMessage:{e.Message}\nStacktrace:{e.StackTrace}");
                throw new Exception($"Failed to perform Query '{command}'", e);
            }
        }
    }
}
