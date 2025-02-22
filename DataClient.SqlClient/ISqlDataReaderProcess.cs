﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient.SqlClient
{
    public interface ISqlDataReaderProcess : IDataReaderProcess
    {
        Task Read(
            ISqlSettings settings,
            ISqlDbProviderFactory providerFactory,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Func<DbDataReader, Task> readAction = null);

        Task<T> Read<T>(
            ISqlSettings settings,
            ISqlDbProviderFactory providerFactory,
            string commandText,
            CommandType commandType = CommandType.Text,
            IEnumerable<IDataParameter> parameters = null,
            Func<DbDataReader, Task<T>> readAction = null);
    }
}
