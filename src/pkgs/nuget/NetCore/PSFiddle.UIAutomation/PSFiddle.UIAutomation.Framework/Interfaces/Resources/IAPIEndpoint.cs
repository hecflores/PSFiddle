using System.Threading.Tasks;
using MC.Track.TestSuite.Model.Domain;

namespace MC.Track.TestSuite.Interfaces.Resources
{
    public interface IAPIEndpoint
    {
        Task<ApiResult> PerformRequest(string RelativePath, string HttpMethod, string ContentType, string DataPath);
    }
}