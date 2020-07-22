using System;
using System.Data;
namespace BrassLoon.DataClient 
{
    public interface ISqlDbProviderFactory : IDbProviderFactory
    {
        IDbConnection OpenConnection(string connectionString, Func<string> getAccessToken);
        IDbConnection OpenConnection(ISqlSettings settings);
    }
}