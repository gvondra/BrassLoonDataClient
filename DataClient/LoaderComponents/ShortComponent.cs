using System.Data.Common;
using System.Threading.Tasks;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class ShortComponent : PrimativeLoaderComponent<short>, ILoaderComponent
    {
        public async Task<object> GetValue(DbDataReader reader, int ordinal, ColumnMapping columnMapping) => await base.GetFieldValue(reader, ordinal);
    }
}