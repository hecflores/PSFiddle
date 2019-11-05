using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    
    public class DisposableResourceType
    {
        public List<Type> DisposeTypesFirst { get; set; }
        public bool isDisposed { get; set; }
        public Object resource { get; set; }
        public Action<Object> disposeDelegate { get; set; }
    }
}
