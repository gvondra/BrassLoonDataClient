using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    // This is the base settings interface. Consumers of the client must implement this interface.
    // Implementations of this class are used to get the connection string
    public interface ISettings
    {
        Task<string> GetConnectionString();
    }
}