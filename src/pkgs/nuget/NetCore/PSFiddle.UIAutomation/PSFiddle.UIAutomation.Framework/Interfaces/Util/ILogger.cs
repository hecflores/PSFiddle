using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Util
{
    public interface ILogger: IDisposable
    {
        ISmartResourceDestroyerService<EventHandler<String>> Subscribe(EventHandler<String> handler);
        IScoper UseRawProvider();
        void LogTrace(String message);
        void LogWarning(String message);
        void LogError(String message);
        void LogError(Exception e);
        void LogError(String message, Exception e);
        void IndentUp();
        void IndentDown();
        ILogger Section(String Name);

    }
}
