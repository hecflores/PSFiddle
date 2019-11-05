using System.Threading.Tasks;
using MC.Track.TestSuite.Model.Domain;
using MC.Track.TestSuite.Model.Enums;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IRawAPIRequestService
    {
        Task<ApiResult> ExecuteApiAsync(string requestUrl, AuthenticationType authenticationType, string accesstoken, string httpMethod, string contentType, string requestContent = "");
    }
}