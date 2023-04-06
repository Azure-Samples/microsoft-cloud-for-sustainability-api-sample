using Azure.CfS.Library;
using Azure.CfS.Library.Options;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

[assembly: FunctionsStartup(typeof(Fta.CfsSample.Api.Startup))]
namespace Fta.CfsSample.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Environment.CurrentDirectory)
                 .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .Build();
            
            builder.Services.AddCfsLibrary(new CfsLibraryOptions
            {
                CfsApiPrimaryKey = configuration.GetValue<string>("CfsApi:PrimaryKey"),
                AzureAdClientId = configuration.GetValue<string>("AzureAd:ClientId"),
                AzureAdClientSecret = configuration.GetValue<string>("AzureAd:ClientSecret"),
                AzureAdTenantId = configuration.GetValue<string>("AzureAd:TenantId")
            });
        }
    }
}
