using System;
using System.Data.Common;
using System.Threading.Tasks;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class TimespanComponent : PrimativeLoaderComponent<TimeSpan>, ILoaderComponent
    {
        public async Task<object> GetValue(DbDataReader reader, int ordinal, ColumnMapping columnMapping)
        {
            return await base.GetFieldValue(reader, ordinal);
        }
    }
}