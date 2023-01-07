using Microsoft.Data.SqlClient;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    public class SqlClientProviderFactory : DbProviderFactory, ISqlDbProviderFactory
    {
        public SqlClientProviderFactory()
        : base(Microsoft.Data.SqlClient.SqlClientFactory.Instance)
        {}

        public async Task<DbConnection> OpenConnection(string connectionString, Func<Task<string>> getAccessToken, bool useDefaultAzureToken = false)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            DbConnection connection = InnerFactory.CreateConnection();
            connection.ConnectionString = connectionString;
            await this.SetAccessToken(connection, getAccessToken, useDefaultAzureToken);
            await connection.OpenAsync();
            return connection;            
        }

        public async Task<DbConnection> OpenConnection(ISqlSettings settings)
        {
            return await this.OpenConnection(await settings.GetConnectionString(), settings.GetAccessToken, useDefaultAzureToken: settings.UseDefaultAzureToken);
        }

        private async Task SetAccessToken(DbConnection connection, Func<Task<string>> getAccessToken, bool useDefaultAzureToken)
        {
            if (getAccessToken != null && !connection.GetType().Equals(typeof(SqlConnection)))
                throw new ArgumentException($"Unable to set access token on connection of type {connection.GetType().FullName}");
            string accessToken = null;
            if (getAccessToken != null)
                accessToken = await getAccessToken();
            if (string.IsNullOrEmpty(accessToken) && useDefaultAzureToken)
                accessToken = (await AzureTokenProvider.GetDefaultToken()).Token;
            if (!string.IsNullOrEmpty(accessToken))
                ((SqlConnection)connection).AccessToken = accessToken;
        }

        public override async Task<DbConnection> OpenConnection(ISettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (typeof(ISqlSettings).IsAssignableFrom(settings.GetType()))
                return await this.OpenConnection((ISqlSettings)settings);
            else 
                return await base.OpenConnection(await settings.GetConnectionString());
        }
    }
}