using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Enums
{
    public partial class ExpectedActualData
    {
        private static String AutoDefine([CallerMemberName] String varName = "")
        {
            return varName;
        }
        public const String FileExtension = "FileExtension";
        public const String FilePrefix = "FilePrefix";
        public const String FileName = "FileName";
        public const String StatusCode = "StatusCode";
        public const String ResponseFileName = "ResponseFileName";
        public const String TransactionStatusCode = "TransactionStatusCode";
        public const String TransactionAcceptanceCode = "TransactionAcceptanceCode";
        public const String CorrelationID = "CorrelationID";


    }
}
