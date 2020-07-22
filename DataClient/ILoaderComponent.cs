using System.Data;
namespace BrassLoon.DataClient
{
    public interface ILoaderComponent 
    {
        bool IsApplicable(ColumnMapping mapping);
        object GetValue(IDataReader reader, int ordinal);
    }
}