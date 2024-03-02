using BrassLoon.DataClient.MongoDB;
using MongoDB.Driver;
using Moq;
using System.Threading.Tasks;

namespace DataClient.MongoDBTest
{
    [TestClass]
    public class DbProviderTest
    {
        [TestMethod]
        public async Task GetClientTest()
        {
            string[] connectionStrings = new string[]
            {
                "mongodb://localhost:27017",
                "MONGODB://LOCALHOST:27017",
                "mongodb://127.0.0.1:27017"
            };
            int index = 0;
            Mock<ISettings> settings = new Mock<ISettings>();
            settings.Setup(s => s.GetConnectionString()).Returns(() =>
            {
                int i = index;
                index += 1;
                return Task.FromResult(connectionStrings[i]);
            });
            DbProvider provider = new DbProvider();
            provider.ClearCache();
            MongoClient[] clients = new MongoClient[]
            {
                await provider.GetClient(settings.Object),
                await provider.GetClient(settings.Object),
                await provider.GetClient(settings.Object)
            };
            Assert.AreSame(clients[0], clients[1]);
            Assert.AreNotSame(clients[0], clients[2]);
        }
    }
}
