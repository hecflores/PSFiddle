using PSFiddle.UIAutomation.Framework.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSFiddle.UIAutomation.Framework.Interfaces.Services
{
    public interface IRuntimeService
    {
        List<StackTraceType> GetStackTrace();
        List<StackTraceType> GetStackTrace(Exception exception);
    }
}
