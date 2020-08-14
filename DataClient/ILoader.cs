using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    public interface ILoader 
    {
        Task<object> Load(object data, DbDataReader reader);
    }
}