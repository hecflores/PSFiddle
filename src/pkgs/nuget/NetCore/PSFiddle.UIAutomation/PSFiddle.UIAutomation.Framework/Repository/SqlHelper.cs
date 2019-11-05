
using MC.Track.TestSuite.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Repository
{
    public class SqlHelper
    {
        public enum ReadExpectations
        {
            None,
            NotNull
        }

        public static E ReadColumn<E>(IDataReader reader, String Column, ReadExpectations Expectations = ReadExpectations.None)
        {
            try
            {
                var obj = reader[Column];
                if ((obj == null || obj is System.DBNull) && Expectations.Equals(ReadExpectations.NotNull))
                {
                    throw new Exception($"Invalid column ({Column})");
                }

                if (obj is System.DBNull)
                {
                    return default(E);
                }
                E Value = (E)reader[Column];

                if (Value == null && Expectations.Equals(ReadExpectations.NotNull))
                {
                    throw new Exception($"Invalid column ({Column})");
                }

                return Value;
            }
            catch(Exception e)
            {
                XConsole.WriteLine($"Invalid column ({Column}). Exception: {e.Message}");
                return default(E);
            }

        }
    }
}
