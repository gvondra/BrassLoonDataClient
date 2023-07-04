using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    public interface IDbProviderFactory
    {
        DbConnection CreateConnection();
        IDataParameter CreateParameter();
        /// Open connection will create a connection instance, specify the connection string, and open the connection
        Task<DbConnection> OpenConnection(string connectionString);
        Task<DbConnection> OpenConnection(ISettings settings);
        /// First, checks the settings object for an instance of a connection and if no connection is present then a connection is opened
        /// Second, if no transaction is present then a transaction is started
        Task EstablishTransaction(ITransactionHandler transactionHandler);
        Task EstablishTransaction(ITransactionHandler transactionHandler, params IDbTransactionObserver[] observers);
    }
}