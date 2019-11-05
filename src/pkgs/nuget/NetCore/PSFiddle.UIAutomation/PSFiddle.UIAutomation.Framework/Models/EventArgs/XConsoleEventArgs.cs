using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.EventArgs
{
    public class XConsoleEventArgs : System.EventArgs
    {
        public String RawText { get; set; }
        public String HtmlText { get; set; }
    }
}
