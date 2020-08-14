using System.Data.Common;
using System.Threading.Tasks;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class BytesComponent : ILoaderComponent
    {
        public async Task<object> GetValue(DbDataReader reader, int ordinal)
        {
            byte[] result = null;
            if (!await reader.IsDBNullAsync(ordinal))
            {                
                result = await reader.GetFieldValueAsync<byte[]>(ordinal);
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(byte[]));
        }
    }
}