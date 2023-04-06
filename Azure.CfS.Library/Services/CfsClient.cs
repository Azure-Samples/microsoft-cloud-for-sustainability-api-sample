using Azure.CfS.Library.Contracts;
using Azure.CfS.Library.Interfaces;
using Azure.CfS.Library.Models;
using Azure.CfS.Library.Options;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.CfS.Library.Services
{
    internal class CfsClient : ICfsClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILoggerAdapter<CfsClient> _logger;

        public CfsClient(HttpClient httpClient, ILoggerAdapter<CfsClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        
        public async Task<Result<GetEnrollmentEmissionsResponse>> GetEmissionsByEnrollmentAsync(CfsApiOptions cfsApiOptions, CancellationToken ct)
        {
            ValidateCfsApiOptions(cfsApiOptions);

            try
            {
                _httpClient.DefaultRequestHeaders.Add(Constants.AuthorizationHeaderName, $"Bearer {cfsApiOptions.AccessToken}");
                
                var httpResponseMessage = await _httpClient.GetAsync(BuildUrl(cfsApiOptions, Constants.CfsOperations.EmissionsByEnrollment), ct).ConfigureAwait(false);
                
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new Result<GetEnrollmentEmissionsResponse>
                    {
                        Data = (await httpResponseMessage.Content.ReadFromJsonAsync<GetEnrollmentEmissionsResponse>(cancellationToken: ct).ConfigureAwait(false))!
                    };
                }

                await LogErrorMessageAsync(httpResponseMessage, ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in {nameof(CfsClient)} -> {nameof(GetEmissionsByEnrollmentAsync)} method.");
            }

            return GenerateErrorResult<GetEnrollmentEmissionsResponse>(Constants.ErrorCodes.Emissions, "Unable to fetch emissions for enrollment.");
        }

        public async Task<Result<string>> GetMetadataAsync(CfsApiOptions cfsApiOptions, CancellationToken ct)
        {
            ValidateCfsApiOptions(cfsApiOptions);

            try
            {
                _httpClient.DefaultRequestHeaders.Add(Constants.AuthorizationHeaderName, $"Bearer {cfsApiOptions.AccessToken}");
                
                var httpResponseMessage = await _httpClient.GetAsync(BuildUrl(cfsApiOptions, Constants.CfsOperations.Metadata), ct).ConfigureAwait(false);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new Result<string>
                    {
                        Data = (await httpResponseMessage!.Content.ReadAsStringAsync(ct).ConfigureAwait(false))!
                    };
                }

                await LogErrorMessageAsync(httpResponseMessage, ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in {nameof(CfsClient)} -> {nameof(GetMetadataAsync)} method.");
            }

            return GenerateErrorResult<string>(Constants.ErrorCodes.Metadata, "Unable to fetch metadata.");
        }

        public async Task<Result<GetEnrollmentProjectionsResponse>> GetProjectionsByEnrollmentAsync(CfsApiOptions cfsApiOptions, CancellationToken ct)
        {
            ValidateCfsApiOptions(cfsApiOptions);

            try
            {
                _httpClient.DefaultRequestHeaders.Add(Constants.AuthorizationHeaderName, $"Bearer {cfsApiOptions.AccessToken}");
                
                var httpResponseMessage = await _httpClient.GetAsync(BuildUrl(cfsApiOptions, Constants.CfsOperations.ProjectionsByEnrollment), ct).ConfigureAwait(false);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new Result<GetEnrollmentProjectionsResponse>
                    {
                        Data = (await httpResponseMessage.Content.ReadFromJsonAsync<GetEnrollmentProjectionsResponse>(cancellationToken: ct).ConfigureAwait(false))!
                    };
                }

                await LogErrorMessageAsync(httpResponseMessage, ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in {nameof(CfsClient)} -> {nameof(GetProjectionsByEnrollmentAsync)} method.");
            }

            return GenerateErrorResult<GetEnrollmentProjectionsResponse>(Constants.ErrorCodes.Projections, "Unable to fetch projections for enrollment.");
        }

        public async Task<Result<GetEnrollmentUsagesResponse>> GetUsageByEnrollmentAsync(CfsApiOptions cfsApiOptions, CancellationToken ct)
        {
            ValidateCfsApiOptions(cfsApiOptions);

            try
            {
                _httpClient.DefaultRequestHeaders.Add(Constants.AuthorizationHeaderName, $"Bearer {cfsApiOptions.AccessToken}");
                
                var httpResponseMessage = await _httpClient.GetAsync(BuildUrl(cfsApiOptions, Constants.CfsOperations.UsageByEnrollment), ct).ConfigureAwait(false);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new Result<GetEnrollmentUsagesResponse>
                    {
                        Data = (await httpResponseMessage.Content.ReadFromJsonAsync<GetEnrollmentUsagesResponse>(cancellationToken: ct).ConfigureAwait(false))!
                    };
                }

                await LogErrorMessageAsync(httpResponseMessage, ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in {nameof(CfsClient)} -> {nameof(GetUsageByEnrollmentAsync)} method.");
            }

            return GenerateErrorResult<GetEnrollmentUsagesResponse>(Constants.ErrorCodes.Usage, "Unable to fetch usage for enrollment.");
        }

        private static void ValidateCfsApiOptions(CfsApiOptions cfsApiOptions)
        {
            if (cfsApiOptions == null)
            {
                throw new ArgumentNullException(nameof(cfsApiOptions));
            }

            if (string.IsNullOrEmpty(cfsApiOptions.AccessToken))
            {
                throw new ArgumentException($"The {nameof(cfsApiOptions.AccessToken)} cannot be null or empty.");
            }
        }

        private static string BuildUrl(CfsApiOptions cfsApiOptions, string operation)
        {
            var result = new StringBuilder($"instances/{cfsApiOptions.InstanceId}/enrollments/{cfsApiOptions.EnrollmentId}/{operation}");
            
            if (cfsApiOptions.QueryParams is not null)
            {
                result.Append('?');

                if (cfsApiOptions.QueryParams.Skip != default)
                {
                    result.Append($"$skip={cfsApiOptions.QueryParams.Skip}");
                    result.Append('&');
                }
                if (cfsApiOptions.QueryParams.Count != default)
                {
                    result.Append($"$count={cfsApiOptions.QueryParams.Count}");
                    result.Append('&');
                }
                if (!string.IsNullOrWhiteSpace(cfsApiOptions.QueryParams.Select))
                {
                    result.Append($"$select={cfsApiOptions.QueryParams.Select}");
                    result.Append('&');
                }
                if (!string.IsNullOrWhiteSpace(cfsApiOptions.QueryParams.Filter))
                {
                    result.Append($"$filter={cfsApiOptions.QueryParams.Filter}");
                    result.Append('&');
                }
                if (!string.IsNullOrWhiteSpace(cfsApiOptions.QueryParams.Apply))
                {
                    result.Append($"$apply={cfsApiOptions.QueryParams.Apply}");
                    result.Append('&');
                }
                if (!string.IsNullOrWhiteSpace(cfsApiOptions.QueryParams.Expand))
                {
                    result.Append($"$expand={cfsApiOptions.QueryParams.Expand}");
                    result.Append('&');
                }
                if (!string.IsNullOrWhiteSpace(cfsApiOptions.QueryParams.OrderBy))
                {
                    result.Append($"$orderby={cfsApiOptions.QueryParams.OrderBy}");
                    result.Append('&');
                }
                if (cfsApiOptions.QueryParams.Top != default)
                {
                    result.Append($"$top={cfsApiOptions.QueryParams.Top}");
                }
            }

            return result.ToString().TrimEnd('&');
        }

        private static Result<T> GenerateErrorResult<T>(int errorCode, string errorMessage)
            where T : class
        {
            return new Result<T>
            {
                Error = new Error
                {
                    Code = errorCode,
                    Message = errorMessage
                }
            };
        }

        private async Task LogErrorMessageAsync(HttpResponseMessage httpResponseMessage, CancellationToken ct)
        {
            var errorMessage = await httpResponseMessage.Content?.ReadAsStringAsync(ct)!;

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                _logger.LogError(errorMessage);
            }
        }
    }
}
