using MySql.Data.MySqlClient;

namespace BrassLoon.DataClient.MySql
{
    public class MySqlProviderFactory : DbProviderFactory
    {
        public MySqlProviderFactory()
            : base(MySqlClientFactory.Instance)
        {
        }
    }
}
