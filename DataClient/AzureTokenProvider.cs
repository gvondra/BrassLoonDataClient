using Azure.Core;
using Azure.Identity;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    public static class AzureTokenProvider
    {
        public static Task<AccessToken> GetDefaultToken()
            => GetDefaultToken(GetDefaultAzureCredentialOptions());

        public static async Task<AccessToken> GetDefaultToken(DefaultAzureCredentialOptions options)
        {
            TokenRequestContext context = new TokenRequestContext(new[] { "https://database.windows.net/.default" });
            return await new DefaultAzureCredential(options)
                .GetTokenAsync(context);
        }

        public static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptions()
        {
            return new DefaultAzureCredentialOptions()
            {
                ExcludeAzureCliCredential = false,
                ExcludeAzurePowerShellCredential = false,
                ExcludeSharedTokenCacheCredential = false,
                ExcludeEnvironmentCredential = false,
                ExcludeManagedIdentityCredential = false,
                ExcludeVisualStudioCodeCredential = false,
                ExcludeVisualStudioCredential = false
            };
        }
    }
}
