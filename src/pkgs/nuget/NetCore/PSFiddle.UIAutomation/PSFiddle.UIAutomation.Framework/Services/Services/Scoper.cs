using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class Scoper : IScoper
    {
        private Action disposeCallback;
        public Scoper(Action dispose)
        {
            this.disposeCallback = dispose;
        }
        public void Dispose()
        {
            this.disposeCallback();
        }
    }
}
