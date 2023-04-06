using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Azure.CfS.Library.Options
{
    public class CfsApiOptions
    {
        public Guid InstanceId { get; set; }
        public string EnrollmentId { get; set; } = default!;
        public string? AccessToken { get; set; } = default!;
        internal QueryParams QueryParams { get; set; } = default!;

        public CfsApiOptions(Guid instanceId, string enrollmentId, IEnumerable<KeyValuePair<string, StringValues>>? queryParams)
        {
            if (instanceId == Guid.Empty)
            {
                throw new ArgumentException($"The {nameof(instanceId)} cannot be empty.");
            }

            if (string.IsNullOrEmpty(enrollmentId))
            {
                throw new ArgumentException($"The {nameof(enrollmentId)} cannot be null or empty.");
            }

            InstanceId = instanceId;
            EnrollmentId = enrollmentId;

            if (queryParams is not null && queryParams.Any())
            {
                QueryParams = new QueryParams();
                
                foreach (var param in queryParams)
                {
                    var paramKey = param.Key;
                    var paramValue = param.Value;

                    switch (paramKey)
                    {
                        case Constants.CfsQueryParamNames.Apply:
                            QueryParams.Apply = paramValue!;
                            break;
                        case Constants.CfsQueryParamNames.Count:
                            if (bool.TryParse(paramValue, out bool countResult))
                            {
                                QueryParams.Count = countResult;
                            }
                            break;
                        case Constants.CfsQueryParamNames.Expand:
                            QueryParams.Expand = paramValue!;
                            break;
                        case Constants.CfsQueryParamNames.Filter:
                            QueryParams.Filter = paramValue!;
                            break;
                        case Constants.CfsQueryParamNames.OrderBy:
                            QueryParams.OrderBy = paramValue!;
                            break;
                        case Constants.CfsQueryParamNames.Select:
                            QueryParams.Select = paramValue!;
                            break;
                        case Constants.CfsQueryParamNames.Skip:
                            if (int.TryParse(paramValue, out int skipResult))
                            {
                                QueryParams.Skip = skipResult;
                            }
                            break;
                        case Constants.CfsQueryParamNames.Top:
                            if (int.TryParse(paramValue, out int topResult))
                            {
                                QueryParams.Top = topResult;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    internal class QueryParams
    {
        internal string Apply { get; set; } = default!;
        internal bool Count { get; set; }
        internal string Expand { get; set; } = default!;
        internal string Filter { get; set; } = default!;
        internal string OrderBy { get; set; } = default!;
        internal string Select { get; set; } = default!;
        internal int Skip { get; set; }
        internal int Top { get; set; }
    }
}
