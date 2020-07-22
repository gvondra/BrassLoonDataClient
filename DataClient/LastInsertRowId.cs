using System.Data;
namespace BrassLoon.DataClient
{
    public class LastInsertRowId 
    {
        public long GetLastInsertRowId(IDbConnection connection)
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "Select last_insert_rowid();";
                return (long)command.ExecuteScalar();
            }
        }
    }
}