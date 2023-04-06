using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Azure.CfS.Library.Contracts
{
    public class GetEnrollmentProjectionsResponse
    {
        [JsonPropertyName("value")]
        public List<EnrollmentProjection> EnrollmentProjections { get; set; } = new List<EnrollmentProjection>();
    }

    public class EnrollmentProjection : EnrollmentBase
    {
        [JsonPropertyName("actualEmissions")]
        public double? ActualEmissions { get; set; }

        [JsonPropertyName("actualUsage")]
        public double? ActualUsage { get; set; }

        [JsonPropertyName("projectedEmissions")]
        public double? ProjectedEmissions { get; set; }

        [JsonPropertyName("projectedUsage")]
        public double? ProjectedUsage { get; set; }
    }
}
