namespace Azure.CfS.Library.Options
{
    public class CfsLibraryOptions
    {
        public string CfsApiPrimaryKey { get; set; } = default!;
        public string CfsApiScope { get; set; } = Constants.DefaultCfsApiScope;
        public string CfsApiVersion { get; set; } = Constants.DefaultCfsApiVersion;
        public string AzureAdTenantId { get; set; } = default!;
        public string AzureAdClientId { get; set; } = default!;
        public string AzureAdClientSecret { get; set; } = default!;
    }
}
