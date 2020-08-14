using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient 
{
    public interface ISqlDbProviderFactory : IDbProviderFactory
    {
        Task<DbConnection> OpenConnection(string connectionString, Func<Task<string>> getAccessToken);
        Task<DbConnection> OpenConnection(ISqlSettings settings);
    }
}