using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AthenaInterceptor : Attribute
    {
        public Type[] Types { get; set; }
        public AthenaInterceptor(params Type[] Types)
        {
            this.Types = Types;
        }

        public bool CanIntercept<T>()
        {
            if (Types == null)
                return true;

            return Types.Where(b => b.Equals(typeof(T))).Count() != 0;
        }

    }
}
