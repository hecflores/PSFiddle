using System.Threading;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IServiceBusTopicListener
    {
        void Abort();
        Task CloseAsync(CancellationToken cancellationToken);
        Task OpenAsync();
    }
}