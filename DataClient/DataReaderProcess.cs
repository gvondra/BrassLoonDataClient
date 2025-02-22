using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    public class DataReaderProcess : IDataReaderProcess
    {
        public virtual int? CommandTimeout { get; set; }

        public Task Read(
            ISettings settings,
            IDbProviderFactory providerFactory,
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
            ISettings settings,
            IDbProviderFactory providerFactory,
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

        public async Task Read(
            Func<Task<DbConnection>> openConnection,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Func<DbDataReader, Task> readAction = null)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentNullException(nameof(commandText));
            if (openConnection == null)
                throw new ArgumentNullException(nameof(openConnection));
            if (readAction == null)
                readAction = (DbDataReader reader) => Task.CompletedTask;
            using (DbConnection connection = await openConnection())
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    using (DbDataReader reader = await ExecuteReader(command, commandText, commandType, parameters))
                    {
                        await readAction(reader);
#if NET6_0_OR_GREATER
                        await reader.CloseAsync();
#else
                        reader.Close();
#endif
                    }
                }
#if NET6_0_OR_GREATER
                await connection.CloseAsync();
#else
                connection.Close();
#endif
            }
        }

        public async Task<T> Read<T>(
            Func<Task<DbConnection>> openConnection,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Func<DbDataReader, Task<T>> readAction = null)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentNullException(nameof(commandText));
            if (openConnection == null)
                throw new ArgumentNullException(nameof(openConnection));
            if (readAction == null)
                readAction = (DbDataReader reader) => Task.FromResult<T>(default(T));
            T result;
            using (DbConnection connection = await openConnection())
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    using (DbDataReader reader = await ExecuteReader(command, commandText, commandType, parameters))
                    {
                        result = await readAction(reader);
#if NET6_0_OR_GREATER
                        await reader.CloseAsync();
#else
                        reader.Close();
#endif
                    }
                }
#if NET6_0_OR_GREATER
                await connection.CloseAsync();
#else
                connection.Close();
#endif
            }
            return result;
        }

        private Task<DbDataReader> ExecuteReader(
            DbCommand command,
            string commandText,
            CommandType commandType,
            IEnumerable<IDataParameter> parameters)
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
            return command.ExecuteReaderAsync();
        }
    }
}
