using System.Data;
namespace BrassLoon.DataClient 
{
    public interface IDbProviderFactory
    {
        IDbConnection CreateConnection();
        IDataParameter CreateParameter();
        /// Open connection will create a connection instance, specify the connection string, and open the connection
        IDbConnection OpenConnection(string connectionString);
        IDbConnection OpenConnection(ISettings settings);
        /// First, checks the settings object for an instance of a connection and if no connection is present then a connection is opened
        /// Second, if no transaction is present then a transaction is started
        void EstablishTransaction(ITransactionHandler transactionHandler);
        void EstablishTransaction(ITransactionHandler transactionHandler, params IDbTransactionObserver[] observers);
    }
}