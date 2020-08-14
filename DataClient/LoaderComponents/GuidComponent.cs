using System;
using System.Data.Common;
using System.Threading.Tasks;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class GuidComponent : PrimativeLoaderComponent<Guid>, ILoaderComponent
    {
        public async Task<object> GetValue(DbDataReader reader, int ordinal)
        {
            object result = null;
            if (!await reader.IsDBNullAsync(ordinal))
            {                
                if (reader.GetFieldType(ordinal).Equals(typeof(string)))
                    result = Guid.Parse((await reader.GetFieldValueAsync<string>(ordinal)).Trim());
                else if (reader.GetFieldType(ordinal).Equals(typeof(byte[])))
                    result = new Guid(await reader.GetFieldValueAsync<byte[]>(ordinal));
                else
                    result = await reader.GetFieldValueAsync<Guid>(ordinal);
            }
            return result;
        }
    }
}