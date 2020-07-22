using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.Common;
namespace BrassLoon.DataClient
{
    public class SqlClientProviderFactory : DbProviderFactory, ISqlDbProviderFactory
    {
        public SqlClientProviderFactory()
        : base(Microsoft.Data.SqlClient.SqlClientFactory.Instance)
        {}

        public IDbConnection OpenConnection(string connectionString, Func<string> getAccessToken)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            DbConnection connection = InnerFactory.CreateConnection();
            connection.ConnectionString = connectionString;
            this.SetAccessToken(connection, getAccessToken);
            connection.Open();
            return connection;            
        }

        public IDbConnection OpenConnection(ISqlSettings settings)
        {
            return this.OpenConnection(settings.ConnectionString, settings.GetAccessToken);
        }

        private void SetAccessToken(DbConnection connection, Func<string> getAccessToken)
        {
            if (getAccessToken != null && !connection.GetType().Equals(typeof(SqlConnection)))
                throw new ArgumentException($"Unable to set access token on connection of type {connection.GetType().FullName}");
            if (getAccessToken != null)
                ((SqlConnection)connection).AccessToken = getAccessToken();
        }

        public override IDbConnection OpenConnection(ISettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (typeof(ISqlSettings).IsAssignableFrom(settings.GetType()))
                return this.OpenConnection((ISqlSettings)settings);
            else 
                return base.OpenConnection(settings.ConnectionString);
        }
    }
}