using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    public class DataReaderProcess
    {
        public int? CommandTimeout { get; set; }

        public Task Read(ISqlSettings settings,
            ISqlDbProviderFactory providerFactory,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Action<DbDataReader> readAction = null)
        {
            return Read(() => providerFactory.OpenConnection(settings),
                commandText,
                commandType,
                parameters,
                readAction
                );
        }

        public Task Read(ISettings settings,
            IDbProviderFactory providerFactory,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Action<DbDataReader> readAction = null)
        {
            return Read(() => providerFactory.OpenConnection(settings),
                commandText,
                commandType,
                parameters,
                readAction
                );
        }

        public async Task Read(Func<Task<DbConnection>> openConnection, 
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Action<DbDataReader> readAction = null)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentNullException(nameof(commandText));
            if (readAction == null)
                throw new ArgumentNullException(nameof(readAction));
            using (DbConnection connection = await openConnection())
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandType = commandType;
                    command.CommandText = commandText;
                    if (parameters != null)
                    {
                        foreach (IDataParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    if (CommandTimeout.HasValue && CommandTimeout.Value > 0)
                        command.CommandTimeout = CommandTimeout.Value;
                    using (DbDataReader reader = await command.ExecuteReaderAsync())
                    {
                        readAction(reader);
                        reader.Close();
                    }
                }
                connection.Close();
            }
        }
    }
}
