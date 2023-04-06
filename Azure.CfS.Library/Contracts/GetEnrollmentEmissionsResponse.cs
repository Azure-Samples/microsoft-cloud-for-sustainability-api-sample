using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Azure.CfS.Library.Contracts
{
    public class GetEnrollmentEmissionsResponse
    {
        [JsonPropertyName("value")]
        public List<EnrollmentEmission> EnrollmentEmissions { get; set; } = new List<EnrollmentEmission>();
    }

    public class EnrollmentEmission : AzureDetailsBase
    {
        [JsonPropertyName("azureServiceName")]
        public string? AzureServiceName { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("scopeId")]
        public int? ScopeId { get; set; }

        [JsonPropertyName("totalEmissions")]
        public double? TotalEmissions { get; set; }
    }
}
