using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSFiddle.Common.Extensions
{
    public static class NumberExtensions
    {
        public static String Prefix(this String main, String content)
        {
            return Regex.Replace(main, @"(^|\n)", $"$1{content}");
        }
        public static String ThatManyCharacters(this int count, String charItem)
        {
            var str = "";
            for (var i = 0; i < count; i++)
            {
                str += charItem;
            }

            return str;
        }
    }
}
