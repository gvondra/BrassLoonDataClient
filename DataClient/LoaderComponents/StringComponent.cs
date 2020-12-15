using System.Data.Common;
using System.Threading.Tasks;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class StringComponent : ILoaderComponent
    {
        public async Task<object> GetValue(DbDataReader reader, int ordinal, ColumnMapping columnMapping)
        {
            string result = null;
            if (!await reader.IsDBNullAsync(ordinal))
            {                
                result = (await reader.GetFieldValueAsync<string>(ordinal)).TrimEnd();
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(string));
        }
    }
}