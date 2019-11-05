using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MC.Track.TestSuite.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Attributes;

namespace MC.Track.TestSuite.Services.Services
{
    [AthenaRegister(typeof(IAuthenticationService), Model.Enums.AthenaRegistrationType.Singleton)]
    public class AuthenticationService : IAuthenticationService
    {
        public async Task<String> AcquireTokenAsyncForApplication(AzureActiveDirectoryConfig azureActiveDirectoryConfig)
        {
            if (azureActiveDirectoryConfig.UserSecurity)
            {
                return await AcquireTokenAsyncForUser(azureActiveDirectoryConfig);
            }
            else
            {
                return await GetTokenForApplication(azureActiveDirectoryConfig);
            }
        }

        /// <summary>
        /// Get Token for Application.
        /// </summary>
        /// <returns>Token for application.</returns>
        private async Task<string> GetTokenForApplication(AzureActiveDirectoryConfig activeDirectoryConfiguration)
        {
            AuthenticationContext authenticationContext = new AuthenticationContext(activeDirectoryConfiguration.Authority, false);
            // Config for OAuth client credentials 
            ClientCredential clientCred = new ClientCredential(activeDirectoryConfiguration.ClientId,
                activeDirectoryConfiguration.AppKey);
            AuthenticationResult authenticationResult =
                await authenticationContext.AcquireTokenAsync(activeDirectoryConfiguration.ResourceId,
                    clientCred);
            return authenticationResult.AccessToken;
        }


        /// <summary>
        /// Async task to acquire token for User.
        /// </summary>
        /// <returns>Token for user.</returns>
        private async Task<string> AcquireTokenAsyncForUser(AzureActiveDirectoryConfig activeDirectoryConfiguration)
        {
            return await GetTokenForUser(activeDirectoryConfiguration);
        }

        /// <summary>
        /// Get Token for User.
        /// </summary>
        /// <returns>Token for user.</returns>
        private async Task<string> GetTokenForUser(AzureActiveDirectoryConfig activeDirectoryConfiguration)
        {
            var redirectUri = new Uri(activeDirectoryConfiguration.RedirectUri);
            AuthenticationContext authenticationContext = new AuthenticationContext(activeDirectoryConfiguration.UserAuthString, false);
            AuthenticationResult userAuthnResult = await authenticationContext.AcquireTokenAsync(activeDirectoryConfiguration.ResourceId,
                activeDirectoryConfiguration.UserClientId, redirectUri, new PlatformParameters(PromptBehavior.RefreshSession));
            return userAuthnResult.AccessToken;
        }
    }
}
