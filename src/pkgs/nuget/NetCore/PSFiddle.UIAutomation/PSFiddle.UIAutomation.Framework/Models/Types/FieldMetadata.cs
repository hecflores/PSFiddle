using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class FieldMetadata
    {
        public FieldMetadata(string name, string normalizedName, string databaseColumnName, Type fieldType, bool required)
        {
            Name = name;
            Required = required;
            NormalizedName = normalizedName;
            DatabaseColumnName = databaseColumnName;
            FieldType = fieldType;
        }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string DatabaseColumnName { get; set; }

        public Type FieldType { get; set; }

        public bool Required { get; set; }
    }
}
