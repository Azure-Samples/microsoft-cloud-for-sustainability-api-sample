using Azure.CfS.Library.Contracts;
using Azure.CfS.Library.Interfaces;
using Azure.CfS.Library.Models;
using Azure.CfS.Library.Options;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.CfS.Library.Services
{
    internal class CfsAuthorizationClientDecorator : ICfsClient
    {
        private readonly ICfsClient _innerCfsClient;
        private readonly IOptions<CfsLibraryOptions> _cfsLibraryOptions;
        private readonly IConfidentialClientApplication _confidentialClientApplication;
        private readonly ITokenCacheService _tokenCacheService;
        private readonly ILoggerAdapter<CfsAuthorizationClientDecorator> _logger;

        public CfsAuthorizationClientDecorator(ICfsClient innerCfsClient,
            IOptions<CfsLibraryOptions> cfsLibraryOptions,
            IConfidentialClientApplication confidentialClientApplication, 
            ITokenCacheService tokenCacheService, 
            ILoggerAdapter<CfsAuthorizationClientDecorator> logger)
        {
            _innerCfsClient = innerCfsClient;
            _cfsLibraryOptions = cfsLibraryOptions;
            _confidentialClientApplication = confidentialClientApplication;
            _tokenCacheService = tokenCacheService;
            _logger = logger;
        }

        public async Task<Result<GetEnrollmentEmissionsResponse>> GetEmissionsByEnrollmentAsync(CfsApiOptions cfsApiOptions, CancellationToken ct)
        {
            cfsApiOptions.AccessToken = await GetClientCredentialsTokenAsync(ct).ConfigureAwait(false);
            
            return await _innerCfsClient.GetEmissionsByEnrollmentAsync(cfsApiOptions, ct).ConfigureAwait(false);
        }

        public async Task<Result<string>> GetMetadataAsync(CfsApiOptions cfsApiOptions, CancellationToken ct)
        {
            cfsApiOptions.AccessToken = await GetClientCredentialsTokenAsync(ct).ConfigureAwait(false);

            return await _innerCfsClient.GetMetadataAsync(cfsApiOptions, ct).ConfigureAwait(false);
        }

        public async Task<Result<GetEnrollmentProjectionsResponse>> GetProjectionsByEnrollmentAsync(CfsApiOptions cfsApiOptions, CancellationToken ct)
        {
            cfsApiOptions.AccessToken = await GetClientCredentialsTokenAsync(ct).ConfigureAwait(false);

            return await _innerCfsClient.GetProjectionsByEnrollmentAsync(cfsApiOptions, ct).ConfigureAwait(false);
        }

        public async Task<Result<GetEnrollmentUsagesResponse>> GetUsageByEnrollmentAsync(CfsApiOptions cfsApiOptions, CancellationToken ct)
        {
            cfsApiOptions.AccessToken = await GetClientCredentialsTokenAsync(ct).ConfigureAwait(false);

            return await _innerCfsClient.GetUsageByEnrollmentAsync(cfsApiOptions, ct).ConfigureAwait(false);
        }
        
        private async Task<string> GetClientCredentialsTokenAsync(CancellationToken ct)
        {
            var cacheKey = $"{_cfsLibraryOptions.Value.AzureAdClientId}:{_cfsLibraryOptions.Value.AzureAdTenantId}|{_cfsLibraryOptions.Value.CfsApiScope}";

            var cachedToken = await _tokenCacheService.GetCachedTokenAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedToken))
            {
                return cachedToken;
            }

            var authenticationResult = await GetAuthenticationResultAsync(ct).ConfigureAwait(false);
            
            // TODO - Get the seconds from the authenticationResult.ExpiresOn - Default is 60 minutes, but better to take it from the access token response.
            await _tokenCacheService.CacheTokenAsync(cacheKey, authenticationResult.AccessToken, TimeSpan.FromSeconds(3540)); // 59 minutes in cache

            return authenticationResult.AccessToken;
        }

        private async Task<AuthenticationResult> GetAuthenticationResultAsync(CancellationToken ct)
        {
            AuthenticationResult? result = null;

            try
            {
                result = await _confidentialClientApplication.AcquireTokenForClient(new string[] { _cfsLibraryOptions.Value.CfsApiScope }).ExecuteAsync(ct).ConfigureAwait(false);
            }
            catch (MsalUiRequiredException ex)
            {
                // The application doesn't have sufficient permissions.
                // - Did you declare enough app permissions during app creation?
                // - Did the tenant admin grant permissions to the application?
                _logger.LogError(ex, $"{nameof(MsalUiRequiredException)} in {nameof(CfsAuthorizationClientDecorator)} -> {nameof(GetAuthenticationResultAsync)} method.");
            }
            catch (MsalServiceException ex) when (ex.Message.Contains(Constants.InvalidScopeErrorCode))
            {
                // Invalid scope. The scope has to be in the form "https://resourceurl/.default"
                // Mitigation: Change the scope to be as expected.
                _logger.LogError(ex, $"{nameof(MsalServiceException)} in {nameof(CfsAuthorizationClientDecorator)} -> {nameof(GetAuthenticationResultAsync)} method.");
            }

            if (result is null || string.IsNullOrEmpty(result.AccessToken))
            {
                throw new InvalidOperationException("Could not fetch access token to call the MCfS API.");
            }

            return result;
        }
    }
}
