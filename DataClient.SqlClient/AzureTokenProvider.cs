using Azure.Core;
using Azure.Identity;
using System.Threading.Tasks;

namespace BrassLoon.DataClient.SqlClient
{
    public static class AzureTokenProvider
    {
        private static readonly TokenRequestContext _databaseTokenRequestContext = CreateTokenRequestContext();
        private static readonly DefaultAzureCredential _defaultAzureCredential = CreateDefaultAzureCredential();
        private static readonly string[] _scopes = new[] { "https://database.windows.net/.default" };

        public static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptions()
        {
            return new DefaultAzureCredentialOptions()
            {
                ExcludeAzureCliCredential = false,
                ExcludeAzurePowerShellCredential = false,
                ExcludeSharedTokenCacheCredential = true,
                ExcludeEnvironmentCredential = false,
                ExcludeManagedIdentityCredential = false,
                ExcludeVisualStudioCodeCredential = false,
                ExcludeVisualStudioCredential = false
            };
        }

        public static async Task<AccessToken> GetDefaultToken() => await _defaultAzureCredential.GetTokenAsync(_databaseTokenRequestContext);

        private static TokenRequestContext CreateTokenRequestContext() => new TokenRequestContext(_scopes);

        private static DefaultAzureCredential CreateDefaultAzureCredential() => new DefaultAzureCredential(GetDefaultAzureCredentialOptions());
    }
}
