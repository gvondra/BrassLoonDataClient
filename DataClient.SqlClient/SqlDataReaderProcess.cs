using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient.SqlClient
{
    public class SqlDataReaderProcess : DataReaderProcess, ISqlDataReaderProcess
    {
        public Task Read(
            ISqlSettings settings,
            ISqlDbProviderFactory providerFactory,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Func<DbDataReader, Task> readAction = null)
        {
            return Read(
                () => providerFactory.OpenConnection(settings),
                commandText,
                commandType,
                parameters,
                readAction);
        }

        public Task<T> Read<T>(
            ISqlSettings settings,
            ISqlDbProviderFactory providerFactory,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Func<DbDataReader, Task<T>> readAction = null)
        {
            return Read<T>(
                () => providerFactory.OpenConnection(settings),
                commandText,
                commandType,
                parameters,
                readAction);
        }
    }
}
