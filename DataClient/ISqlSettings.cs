using System;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    // Extending the ISettings interface, this interface
    // needs to be implemented to authenticate with
    // Azure SQL databases that require an access token.
    public interface ISqlSettings : ISettings
    {
        Func<Task<string>> GetAccessToken { get; }

        /// <summary>
        /// When true and GetAccessToken is null, a DefaultAzureCredential is retrieved from the default context "https://database.windows.net/.default"
        /// </summary>
        bool UseDefaultAzureToken { get; }
    }
}