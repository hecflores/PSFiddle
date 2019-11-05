using MC.Track.TestSuite.Model.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Feeds
{
    public interface IServiceFeed<T> : IDisposable
    {
        EventHandler<IncomingServiceFeedEventArgs<T>> IncomingFeed { get; set; }
        void Create();
        void StartFeed();
    }
}
