using System;
using System.Data.Common;
using System.Threading.Tasks;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class DateComponent : PrimativeLoaderComponent<DateTime>, ILoaderComponent
    {
        public async Task<object> GetValue(DbDataReader reader, int ordinal, ColumnMapping columnMapping)
        {
            DateTime? result = await GetFieldValue(reader, ordinal);
            if (columnMapping.MappingAttribute.IsUtc && result.HasValue)
                result = DateTime.SpecifyKind(result.Value, DateTimeKind.Utc);
            return result;
        }
    }
}