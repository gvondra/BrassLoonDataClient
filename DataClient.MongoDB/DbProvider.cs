using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BrassLoon.DataClient.MongoDB
{
    public class DbProvider : IDbProvider
    {
        private static readonly ConcurrentDictionary<string, ClientCacheItem> _clients = new ConcurrentDictionary<string, ClientCacheItem>();

        public async Task<MongoClient> GetClient(ISettings settings)
        {
            string connectionString = await settings.GetConnectionString();
            string hash = Hash.Compute(connectionString);
            ClientCacheItem clientCacheItem;
            if (!_clients.TryGetValue(hash, out clientCacheItem) || clientCacheItem.IsExpired())
            {
                clientCacheItem = GetCacheItem(hash, connectionString);
            }
            return clientCacheItem.Client;
        }

        public IMongoDatabase GetDatabase(MongoClient client, string dbName) => client.GetDatabase(dbName);
        public async Task<IMongoDatabase> GetDatabase(ISettings settings) => GetDatabase(await GetClient(settings), await settings.GetDatabaseName());

        public IMongoCollection<T> GetCollection<T>(IMongoDatabase db, string collectionName) => db.GetCollection<T>(collectionName);
        public IMongoCollection<T> GetCollection<T>(MongoClient client, string dbName, string collectionName) => GetCollection<T>(GetDatabase(client, dbName), collectionName);
        public async Task<IMongoCollection<T>> GetCollection<T>(ISettings settings, string collectionName) => GetCollection<T>(await GetDatabase(settings), collectionName);

        public void ClearCache()
        {
            lock (_clients)
            {
                _clients.Clear();
            }
        }

        private static ClientCacheItem GetCacheItem(string key, string connectionString)
        {
            ClientCacheItem clientCacheItem;
            lock (_clients)
            {
                foreach (string existingKey in _clients.Keys)
                {
                    if (_clients[existingKey].IsExpired())
                        _clients.TryRemove(existingKey, out ClientCacheItem _);
                }
                if (!_clients.TryGetValue(key, out clientCacheItem) || clientCacheItem.IsExpired())
                {
                    clientCacheItem = new ClientCacheItem(new MongoClient(connectionString));
                    _clients[key] = clientCacheItem;
                }
            }
            return clientCacheItem;
        }
    }
}
