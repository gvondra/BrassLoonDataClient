using System.Data.Common;
using System.Threading.Tasks;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class ByteComponent : PrimativeLoaderComponent<byte>, ILoaderComponent
    {
        public async Task<object> GetValue(DbDataReader reader, int ordinal, ColumnMapping columnMapping)
        {
            return await base.GetFieldValue(reader, ordinal);
        }
    }
}