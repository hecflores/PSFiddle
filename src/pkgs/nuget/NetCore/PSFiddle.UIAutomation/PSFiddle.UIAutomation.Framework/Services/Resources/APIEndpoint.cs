using MC.Track.TestSuite.Interfaces.Resources;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Resources
{
    public class APIEndpoint : IAPIEndpoint
    {
        private readonly Uri rootUrl;
        private readonly AzureActiveDirectoryConfig azureActiveDirectoryConfig;
        private IAuthenticationService authenticationService;
        private IRawAPIRequestService rawAPIRequestService;

        public APIEndpoint(String RootUrl, AzureActiveDirectoryConfig azureActiveDirectoryConfig, IResolver resolver)
        {
            if (string.IsNullOrEmpty(RootUrl))
            {
                throw new ArgumentException($"Was null or empty", nameof(RootUrl));
            }

            this.rootUrl = new Uri(RootUrl);
            this.azureActiveDirectoryConfig = azureActiveDirectoryConfig ?? throw new ArgumentNullException(nameof(azureActiveDirectoryConfig));

            this.authenticationService = resolver.Resolve<IAuthenticationService>();
            this.rawAPIRequestService = resolver.Resolve<IRawAPIRequestService>();
        }

        public async Task<ApiResult> PerformRequest(String RelativePath, String HttpMethod, String ContentType, String DataPath)
        {
            var accessToken = await this.authenticationService.AcquireTokenAsyncForApplication(this.azureActiveDirectoryConfig);

            var url = new Uri(this.rootUrl, RelativePath);

            return await this.rawAPIRequestService.ExecuteApiAsync(url.ToString(), Model.Enums.AuthenticationType.Bearer, accessToken, HttpMethod, ContentType, DataPath);
        }

    }
}
