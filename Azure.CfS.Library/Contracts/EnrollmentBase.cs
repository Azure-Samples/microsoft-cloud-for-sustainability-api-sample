using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Azure.CfS.Library.Contracts
{
    public abstract class EnrollmentBase
    {
        [JsonPropertyName("dateKey")]
        public int? DateKey { get; set; }

        [JsonPropertyName("enrollmentId")]
        public string? EnrollmentId { get; set; }
    }
}
