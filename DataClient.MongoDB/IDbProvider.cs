using MongoDB.Driver;
using System.Threading.Tasks;

namespace BrassLoon.DataClient.MongoDB
{
    public interface IDbProvider
    {
        Task<MongoClient> CreateClient(ISettings settings);

        Task<IMongoDatabase> GetDatabase(ISettings settings);
        IMongoDatabase GetDatabase(MongoClient client, string dbName);

        IMongoCollection<T> GetCollection<T>(IMongoDatabase db, string collectionName);
        IMongoCollection<T> GetCollection<T>(MongoClient client, string dbName, string collectionName);
        Task<IMongoCollection<T>> GetCollection<T>(ISettings settings, string collectionName);
    }
}
