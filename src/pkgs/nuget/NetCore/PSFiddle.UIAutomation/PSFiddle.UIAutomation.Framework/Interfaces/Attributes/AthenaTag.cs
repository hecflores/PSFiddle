using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AthenaTag : Attribute
    {
        private readonly string tagName;
        private readonly string tagValue;

        public AthenaTag(String TagName, String TagValue)
        {
            tagName = TagName;
            tagValue = TagValue;
        }

        public bool isMatch(String tagName, String tagValue)
        {
            return tagName.Equals(this.tagName, StringComparison.CurrentCultureIgnoreCase) &&
                   tagValue.Equals(this.tagValue, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
