using MC.Track.TestSuite.Interfaces.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Util
{
    public class Logger : ILogger
    {
        private static String _prefix = "";
        private Func<String, String> _tempFilter;

        public Logger()
        {
            this._tempFilter = new Func<string, string>((message) =>
            {
                return $"{_prefix}{message}";
            });
        }
        private void LogIt(String message, ConsoleColor color)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(this._tempFilter(message));
            Console.ForegroundColor = defaultColor;

        }
        public void Dispose()
        {
            this.IndentDown();
        }

        public void IndentDown()
        {
            _prefix=_prefix.Substring(0, Math.Max(_prefix.Length - 4, 0));
        }

        public void IndentUp()
        {
            _prefix += "    ";
        }

        public void LogError(string message)
        {
            LogIt(message, ConsoleColor.Red);
        }

        public void LogTrace(string message)
        {
            LogIt(message, ConsoleColor.Cyan);
        }

        public void LogWarning(string message)
        {
            LogIt(message, ConsoleColor.Yellow);
        }

        public ILogger Section(string Name)
        {
            this.LogTrace($"\n{Name}");
            this.IndentUp();
            return this;
        }

        public void LogError(Exception e)
        {
            this.LogError("Exception: ", e);
        }

        public void LogError(string message, Exception e)
        {
            this.LogError($"{message} - {e.Message}\n{e.StackTrace}\n");
        }
    }
}
