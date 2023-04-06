namespace Azure.CfS.Library
{
    public static class Constants
    {
        public const string AuthorizationHeaderName = "Authorization";
        public const string DefaultCfsApiVersion = "v1.0";
        public const string DefaultCfsApiScope = "c3163bf1-092f-436b-b260-7ade5973e5b9/.default";
        public const string OcpApimSubscriptionKeyHeaderName = "Ocp-Apim-Subscription-Key";
        public const string CfsApiBaseUrl = "https://api.mcfs.microsoft.com/api";
        public const string AzureAdInstance = "https://login.microsoftonline.com/{0}";
        public static class CfsQueryParamNames
        {
            public const string Apply = "apply";
            public const string Count = "count";
            public const string Expand = "expand";
            public const string Filter = "filter";
            public const string OrderBy = "orderby";
            public const string Select = "select";
            public const string Skip = "skip";
            public const string Top = "top";
        }
        public const string InvalidScopeErrorCode = "AADSTS70011";
        public static class CfsOperations
        {
            public const string EmissionsByEnrollment = "emissions";
            public const string Metadata = "$metadata";
            public const string ProjectionsByEnrollment = "projections";
            public const string UsageByEnrollment = "usage";
        }
        public static class ErrorCodes
        {
            public const int Emissions = 900;
            public const int Metadata = 925;
            public const int Projections = 950;
            public const int Usage = 975;
        }
    }
}
