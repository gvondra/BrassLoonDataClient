using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    public interface IDataReaderProcess
    {
        int? CommandTimeout { get; set; }

        Task Read(
            ISettings settings,
            IDbProviderFactory providerFactory,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Func<DbDataReader, Task> readAction = null);

        Task<T> Read<T>(
            ISettings settings,
            IDbProviderFactory providerFactory,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Func<DbDataReader, Task<T>> readAction = null);

        Task Read(
            Func<Task<DbConnection>> openConnection,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Func<DbDataReader, Task> readAction = null);

        Task<T> Read<T>(
            Func<Task<DbConnection>> openConnection,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Func<DbDataReader, Task<T>> readAction = null);
    }
}
