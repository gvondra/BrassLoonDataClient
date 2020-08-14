using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    public class DbProviderFactory : IDbProviderFactory
    {
        public DbProviderFactory()
        {
            InnerFactory = Microsoft.Data.SqlClient.SqlClientFactory.Instance;
        }    
        public DbProviderFactory(System.Data.Common.DbProviderFactory providerFactory)
        {
            InnerFactory = providerFactory;
        }        
        protected System.Data.Common.DbProviderFactory InnerFactory { get; set;}

        public DbConnection CreateConnection() => InnerFactory.CreateConnection();
        public IDataParameter CreateParameter() => InnerFactory.CreateParameter();

        public async Task EstablishTransaction(ITransactionHandler transactionHandler) => await EstablishTransaction(transactionHandler, null);

        public async Task EstablishTransaction(ITransactionHandler transactionHandler, params IDbTransactionObserver[] observers)
        {
            // first check the connection state.  If it's not open then dispose of it
            if (transactionHandler.Connection != null)
            {
                if (transactionHandler.Connection.State != ConnectionState.Open)
                {
                    transactionHandler.Connection.Dispose();
                    transactionHandler.Connection = null;
                }                
            }
            // second open a connection if no connection is already set
            if (transactionHandler.Connection == null)
            {
                transactionHandler.Connection = await OpenConnection(transactionHandler);
            }
            // third begin a transaction
            if (transactionHandler.Transaction == null)
            {
                transactionHandler.Transaction = new DbTransaction(transactionHandler.Connection.BeginTransaction());
            }
            // fourth  add obserevers
            if (transactionHandler.Transaction != null && observers != null)
            {
                for (int i = 0; i < observers.Length; i += 1)
                {
                    transactionHandler.Transaction.AddObserver(observers[i]);
                }
            }
        }

        public virtual async Task<DbConnection> OpenConnection(ISettings settings)
        {
            return await OpenConnection(settings.ConnectionString);
        }

        public virtual async Task<DbConnection> OpenConnection(string connectionString)
        {
            DbConnection connection = null;
            if (!string.IsNullOrEmpty(connectionString))
            {
                connection = InnerFactory.CreateConnection();
                connection.ConnectionString = connectionString;
                await connection.OpenAsync();
            }
            return connection;
        }
    }
}