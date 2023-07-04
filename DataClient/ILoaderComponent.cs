using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    public interface ILoaderComponent
    {
        bool IsApplicable(ColumnMapping mapping);
        Task<object> GetValue(DbDataReader reader, int ordinal, ColumnMapping columnMapping);
    }
}