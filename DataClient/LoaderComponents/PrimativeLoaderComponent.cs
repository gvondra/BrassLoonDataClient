using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient.LoaderComponents
{
    public abstract class PrimativeLoaderComponent<T>
        where T : struct
    {
        public virtual bool IsApplicable(ColumnMapping mapping)
            => mapping.Info.PropertyType.Equals(typeof(T)) || mapping.Info.PropertyType.Equals(typeof(T?));

        protected static async Task<T?> GetFieldValue(DbDataReader reader, int ordinal)
        {
            T? result = null;
            if (!await reader.IsDBNullAsync(ordinal))
                result = await reader.GetFieldValueAsync<T>(ordinal);
            return result;
        }
    }
}
