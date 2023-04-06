using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Azure.CfS.Library.Contracts
{
    public class GetEnrollmentUsagesResponse
    {
        [JsonPropertyName("value")]
        public List<EnrollmentUsage> EnrollmentUsage { get; set; } = new List<EnrollmentUsage>();
    }

    public class EnrollmentUsage : AzureDetailsBase
    {
        [JsonPropertyName("usage")]
        public double? Usage { get; set; }
    }
}
