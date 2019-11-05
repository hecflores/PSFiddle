using PSFiddle.UIAutomation.Framework.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Repository.Util
{
    public static class ServiceLogger
    {
        public static string LogError(Exception ex)
        {
            if (ex == null)
            {
                Trace.TraceError("Could not access exception error!");
            }

            Trace.TraceError($"Exception: {ex.GetType()} \r\n InnerException: {ex.InnerException} \r\n Message:  {ex.Message} \r\nStackTrace: {ex.StackTrace}");
            return ex.Message;
        }
        public static string LogParameterNull(string message)
        {
            Trace.TraceError($"Error: Service encountered null parameters \r\n Time: {DateTime.Now}");
            return message;
        }
        public static string LogParameterNull()
        {
            return LogParameterNull(Resources.Error_ArgumentNullOrEmpty);
        }
        public static string LogError(string message, [CallerMemberName] string callerName = "")
        {
            Trace.TraceError($"Error: \r\n Calling method: {callerName} \r\n Message to user: {message} \r\n Time: {DateTime.Now}");
            return message;
        }
    }
}
