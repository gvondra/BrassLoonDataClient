using System.Data;
namespace BrassLoon.DataClient
{
    public interface ILoader 
    {
        object Load(object data, IDataReader reader);
    }
}