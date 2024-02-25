using System.Threading.Tasks;

namespace BrassLoon.DataClient.MongoDB
{
    public interface ISettings
    {
        Task<string> GetConnectionString();
        Task<string> GetDatabaseName();
    }
}
