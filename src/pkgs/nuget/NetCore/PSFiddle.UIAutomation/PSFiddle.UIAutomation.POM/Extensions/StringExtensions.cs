using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        public static String MultiLinedStringLiteral(this String str)
        {
            str = str.Replace("\"", "\"\"")
                     .Replace("{", "{{")
                     .Replace("}", "}}");

            str = $"@\"{str}\"";
            return str;
        }
    }
}
