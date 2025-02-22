using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient.SqlClient
{
    public static class LastInsertRowId
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
}
