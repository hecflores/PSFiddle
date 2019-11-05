using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Diagnostics
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// Formats the specified process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns></returns>
        public static String Format(this Process process)
        {
            return string.Format("{0,-10}{1,12}{2,12}{3}", process.Id, process.ProcessName, process.MachineName, process.MainModule?.FileName);
        }


        /// <summary>
        /// Formats the specified process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns></returns>
        public static String Format(this Process process, String status)
        {
            return string.Format("{0,4}:{1}", " Ok ", process.Format());
        }
    }
}
