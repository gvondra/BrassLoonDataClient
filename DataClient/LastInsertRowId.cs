using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    public class LastInsertRowId 
    {
        public async Task<T> GetLastInsertRowId<T>(DbConnection connection) where T : struct
        {
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "Select last_insert_rowid();";
                return (T)(await command.ExecuteScalarAsync());
            }
        }
    }
}