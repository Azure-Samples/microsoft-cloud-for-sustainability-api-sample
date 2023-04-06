using Azure.CfS.Library.Contracts;
using Azure.CfS.Library.Models;
using Azure.CfS.Library.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.CfS.Library.Interfaces
{
    public interface ICfsClient
    {
        Task<Result<GetEnrollmentEmissionsResponse>> GetEmissionsByEnrollmentAsync(CfsApiOptions cfsApiOptions, CancellationToken ct);
        Task<Result<string>> GetMetadataAsync(CfsApiOptions cfsApiOptions, CancellationToken ct);
        Task<Result<GetEnrollmentProjectionsResponse>> GetProjectionsByEnrollmentAsync(CfsApiOptions cfsApiOptions, CancellationToken ct);
        Task<Result<GetEnrollmentUsagesResponse>> GetUsageByEnrollmentAsync(CfsApiOptions cfsApiOptions, CancellationToken ct);
    }
}
