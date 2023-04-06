using System.Text.Json.Serialization;

namespace Azure.CfS.Library.Contracts
{
    public abstract class AzureDetailsBase : EnrollmentBase
    {
        [JsonPropertyName("azureRegionName")]
        public string? AzureRegionName { get; set; }

        [JsonPropertyName("orgName")]
        public string? OrgName { get; set; }

        [JsonPropertyName("subService")]
        public string? SubService { get; set; }

        [JsonPropertyName("subscriptionId")]
        public string? SubscriptionId { get; set; }

        [JsonPropertyName("subscriptionName")]
        public string? SubscriptionName { get; set; }
    }
}
