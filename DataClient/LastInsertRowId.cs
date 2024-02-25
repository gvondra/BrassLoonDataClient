using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
#pragma warning disable S1118 // Utility classes should not have public constructors
    public class LastInsertRowId
    {
        public static async Task<T> GetLastInsertRowId<T>(DbConnection connection)
            where T : struct
        {
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "Select SCOPE_IDENTITY();";
                return (T)await command.ExecuteScalarAsync();
            }
        }
    }
#pragma warning restore S1118 // Utility classes should not have public constructors
}