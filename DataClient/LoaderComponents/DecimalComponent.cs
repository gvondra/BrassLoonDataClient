using System.Data.Common;
using System.Threading.Tasks;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class DecimalComponent : PrimativeLoaderComponent<decimal>, ILoaderComponent
    {
        public async Task<object> GetValue(DbDataReader reader, int ordinal)
        {
            return await base.GetFieldValue(reader, ordinal);
        }
    }
}