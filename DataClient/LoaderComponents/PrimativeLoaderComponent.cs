using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient.LoaderComponents
{
    public abstract class PrimativeLoaderComponent<T> where T : struct
    {
        protected async Task<Nullable<T>> GetFieldValue(DbDataReader reader, int ordinal)
        {
            Nullable<T> result = null;
            if (!await reader.IsDBNullAsync(ordinal))
                result = await reader.GetFieldValueAsync<T>(ordinal);
            return result;
        }

        public virtual bool IsApplicable(ColumnMapping mapping)
            => mapping.Info.PropertyType.Equals(typeof(T)) || mapping.Info.PropertyType.Equals(typeof(Nullable<T>));
    }
}
