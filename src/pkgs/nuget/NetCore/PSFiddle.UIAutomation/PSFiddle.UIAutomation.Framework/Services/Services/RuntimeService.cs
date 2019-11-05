using PSFiddle.UIAutomation.Framework.Interfaces.Services;
using PSFiddle.UIAutomation.Framework.Models.Types;
using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSFiddle.UIAutomation.Framework.Services.Services
{
    [AthenaRegister(typeof(IRuntimeService))]
    public class RuntimeService : IRuntimeService
    {
        public List<StackTraceType> GetStackTrace()
        {
            List<StackTraceType> stack = new List<StackTraceType>();
            StackTrace stackTrace = new StackTrace();           // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

            // XConsole.WriteLine($"\nStack Trace");
            foreach (var stackFrame in stackFrames)
            {
                // XConsole.WriteLine($"{stackFrame.GetMethod().Name} - {stackFrame.GetMethod()?.DeclaringType}");
                var stackItem = new StackTraceType()
                {
                    FullClassName = stackFrame.GetMethod()?.DeclaringType?.FullName,
                    MethodName = stackFrame.GetMethod()?.Name,
                    MethodParams = stackFrame.GetMethod()?.GetParameters()?.Select(p => $"{p?.ParameterType} {p?.Name}")?.ToList(),
                    FileName = stackFrame.GetFileName(),
                    LineNumber = stackFrame.GetFileLineNumber(),
                    ClassType = stackFrame.GetMethod()?.DeclaringType
                };

                stack.Add(stackItem);
            }
            // XConsole.WriteLine($"\n");
            return stack;
            //// Debuged Regex Using regex101.com
            //// https://regex101.com/r/suEJcx/2
            //var matches = Regex.Matches(Environment.StackTrace, @" {3}at (?<FullClassName>.*)\.(?<MethodName>.*)\((?<MethodParams>.*)\)(?: in (?<FileName>.*):line (?<LineNumber>\w+)){0,1}");
            //foreach(Match match in matches)
            //{
            //    var stackType = new StackTraceType()
            //    {
            //        FullClassName = match.Groups["FullClassName"].Value,
            //        MethodName = match.Groups["MethodName"].Value,
            //        MethodParams = match.Groups["MethodParams"].Value.Split(',').ToList(),
            //        FileName = match.Groups["FileName"].Value,
            //        LineNumber = match.Groups["LineNumber"].Success ? Int32.Parse(match.Groups["LineNumber"].Value, System.Globalization.NumberStyles.Any) : -1
            //    };
            //    stackType.ClassType = Regex.Match(stackType.FullClassName, "DynamicModule.ns.*").Success ? Type.GetType($"{stackType.FullClassName}, Unity_ILEmit_InterfaceProxies") : Type.GetType(stackType.FullClassName);
            //    stack.Add(stackType);
            //}

            //return stack;
        }

        public List<StackTraceType> GetStackTrace(Exception exception)
        {
            var stack = new List<StackTraceType>();

            // Debuged Regex Using regex101.com
            // https://regex101.com/r/suEJcx/2
            var matches = Regex.Matches(exception.StackTrace, @" {3}at (?<FullClassName>.*)\.(?<MethodName>.*)\((?<MethodParams>.*)\)(?: in (?<FileName>.*):line (?<LineNumber>\w+)){0,1}");
            foreach (Match match in matches)
            {
                var stackType = new StackTraceType()
                {
                    FullClassName = match.Groups["FullClassName"].Value,
                    MethodName = match.Groups["MethodName"].Value,
                    MethodParams = match.Groups["MethodParams"].Value.Split(',').ToList(),
                    FileName = match.Groups["FileName"].Value,
                    LineNumber = match.Groups["LineNumber"].Success ? Int32.Parse(match.Groups["LineNumber"].Value, System.Globalization.NumberStyles.Any) : -1
                };
                stackType.ClassType = Regex.Match(stackType.FullClassName, "DynamicModule.ns.*").Success ? Type.GetType($"{stackType.FullClassName}, Unity_ILEmit_InterfaceProxies") : Type.GetType(stackType.FullClassName);
                stack.Add(stackType);
            }

            return stack;
        }
    }
}
