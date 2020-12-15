using System.Data.Common;
using System.Threading.Tasks;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class LongComponent : PrimativeLoaderComponent<long>, ILoaderComponent
    {
        public async Task<object> GetValue(DbDataReader reader, int ordinal, ColumnMapping columnMapping)
        {
            return await base.GetFieldValue(reader, ordinal);
        }
    }
}