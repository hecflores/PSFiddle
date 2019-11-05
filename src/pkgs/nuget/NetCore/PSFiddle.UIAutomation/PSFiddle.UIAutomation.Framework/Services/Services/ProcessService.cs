using PSFiddle.UIAutomation.Framework.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;

namespace PSFiddle.UIAutomation.Framework.Services.Services
{
    [AthenaRegister(typeof(IProcessService))]
    public class ProcessService : IProcessService
    {
        private readonly ILogger logger;

        public ProcessService(ILogger logger)
        {
            this.logger = logger;
        }
        /// <summary>
        /// Gets the processes.
        /// </summary>
        /// <param name="ProcessName">Name of the process.</param>
        /// <param name="Folder">The folder.</param>
        /// <param name="FileName">Name of the file.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<Process> GetProcesses(string ProcessName = null, Predicate<Process> filter = null)
        {
            List<Process> _processes = new List<Process>();

            if (ProcessName == null)
                _processes = Process.GetProcesses().ToList();
            else
                _processes = Process.GetProcessesByName(ProcessName).ToList();

            List<Process> _finalProcess = new List<Process>();
            using(logger.Section($"Getting all Processes{(ProcessName == null ?"":$" (By Process Name = '{ProcessName}')")}"))
            {
                foreach(var process in _processes)
                {
                    var keepProcess = filter?.Invoke(process);
                    if (!keepProcess.HasValue)
                        continue;

                    if (keepProcess.Value)
                    {
                        logger.LogTrace(string.Format("{0,4}:{1}", " Ok ", process.Format()));
                        _finalProcess.Add(process);
                    }
                    else
                    {
                        logger.LogTrace(string.Format("{0,4}:{1}", "    ", process.Format()));
                    }
                }
            }

            return _finalProcess;
        }

        /// <summary>
        /// Kills the process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool KillProcess(Process process)
        {
            try
            {
                process.Kill();

                logger.LogTrace($"  Ok    - Killing Process[{process.ProcessName}]");
                return true;
            }
            catch(Exception e)
            {
                logger.LogTrace($"*BROKE* - Killing Process[{process.ProcessName}] - {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Kills the processes.
        /// </summary>
        /// <param name="processes">The processes.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool KillProcesses(List<Process> processes)
        {
            bool killSuccess = true;
            var failedAttempts = 0;
            using (logger.Section($"Killing [{processes.Count()}] processess"))
            {
                
                foreach (var process in processes)
                {
                    var result = KillProcess(process);
                    failedAttempts += result ? 0 : 1;
                    killSuccess |= result;
                }
            }
            if(killSuccess)
            {
                logger.LogTrace($"  Ok    - Killing [{processes.Count()}] processess");
                return true;
            }
            else
            {
                logger.LogTrace($"*BROKE* - Killing [{processes.Count()}] processess");
                return false;
            }

        }
    }
}
