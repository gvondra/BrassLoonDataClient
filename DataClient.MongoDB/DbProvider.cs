using MongoDB.Driver;
using System.Threading.Tasks;

namespace BrassLoon.DataClient.MongoDB
{
    public class DbProvider : IDbProvider
    {
        public async Task<MongoClient> CreateClient(ISettings settings) => new MongoClient(await settings.GetConnectionString());

        public IMongoDatabase GetDatabase(MongoClient client, string dbName) => client.GetDatabase(dbName);
        public async Task<IMongoDatabase> GetDatabase(ISettings settings) => GetDatabase(await CreateClient(settings), await settings.GetDatabaseName());

        public IMongoCollection<T> GetCollection<T>(IMongoDatabase db, string collectionName) => db.GetCollection<T>(collectionName);
        public IMongoCollection<T> GetCollection<T>(MongoClient client, string dbName, string collectionName) => GetCollection<T>(GetDatabase(client, dbName), collectionName);
        public async Task<IMongoCollection<T>> GetCollection<T>(ISettings settings, string collectionName) => GetCollection<T>(await GetDatabase(settings), collectionName);
    }
}
