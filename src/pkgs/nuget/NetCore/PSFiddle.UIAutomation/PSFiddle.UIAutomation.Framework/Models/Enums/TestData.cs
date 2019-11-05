using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Enums
{
    public partial class TestData
    {
        private static String AutoDefine([CallerMemberName] String varName = "")
        {
            return varName;
        }


        public const  String FileName = "FileName";
        public static String ContainerName = AutoDefine();
        public const String PIFScenario = "PIFScenario";
        public const String FilePath = "FilePath";
        public const String Query = "Query"; 


    }
}
