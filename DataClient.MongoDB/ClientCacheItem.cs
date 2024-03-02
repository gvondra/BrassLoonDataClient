using MongoDB.Driver;
using System;

namespace BrassLoon.DataClient.MongoDB
{
    internal readonly struct ClientCacheItem
    {
        public ClientCacheItem(MongoClient client)
        {
            Client = client;
            Expiration = DateTime.UtcNow.AddMinutes(20);
        }

        public MongoClient Client { get; }
        public DateTime Expiration { get; }

        public bool IsExpired() => Expiration < DateTime.UtcNow;
    }
}
