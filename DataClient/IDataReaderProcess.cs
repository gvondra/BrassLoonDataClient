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
            ISqlSettings settings,
            ISqlDbProviderFactory providerFactory,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Action<DbDataReader> readAction = null);

        Task Read(
            ISettings settings,
            IDbProviderFactory providerFactory,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Action<DbDataReader> readAction = null);

        Task Read(
            Func<Task<DbConnection>> openConnection,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Action<DbDataReader> readAction = null);
    }
}
