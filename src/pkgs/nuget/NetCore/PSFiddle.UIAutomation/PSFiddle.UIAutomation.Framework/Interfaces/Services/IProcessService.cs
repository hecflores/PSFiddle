using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IProcessService
    {
        /// <summary>
        /// Gets the processes.
        /// </summary>
        /// <param name="ProcessName">Name of the process.</param>
        /// <param name="Folder">The folder.</param>
        /// <param name="FileName">Name of the file.</param>
        /// <returns></returns>
        List<Process> GetProcesses(string ProcessName = null, Predicate<Process> filter = null);

        /// <summary>
        /// Kills the process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns></returns>
        bool KillProcess(Process process);

        /// <summary>
        /// Kills the processes.
        /// </summary>
        /// <param name="processes">The processes.</param>
        /// <returns></returns>
        bool KillProcesses(List<Process> processes);
    }
}
