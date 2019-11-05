using System.Threading.Tasks;
using MC.Track.TestSuite.Model.Domain;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<string> AcquireTokenAsyncForApplication(AzureActiveDirectoryConfig azureActiveDirectoryConfig);
    }
}