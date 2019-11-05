using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.EventArgs
{
    public class IncomingServiceFeedEventArgs<T>: System.EventArgs
    {
        public DateTime IncomingDate { get;set; }
        public T Data { get; set; }
    }
}
