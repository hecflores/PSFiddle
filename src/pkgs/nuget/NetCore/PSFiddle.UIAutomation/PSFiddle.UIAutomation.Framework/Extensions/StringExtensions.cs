using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSFiddle.UIAutomation.Framework.Extensions
{
    public static class StringExtensions
    {
        public static String UndoCamelCase(this String str)
        {
            return Regex.Replace(str, "(.)([A-Z])", "$1 $2");
        }
    }
}
