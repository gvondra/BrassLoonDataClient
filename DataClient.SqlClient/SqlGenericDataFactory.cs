using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BrassLoon.DataClient.SqlClient
{
    public class SqlGenericDataFactory<T> : GenericDataFactory<T>, ISqlGenericDataFactory<T>
    {
        public Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, CommandType commandType = CommandType.StoredProcedure)
            => GetData(settings, providerFactory, commandText, createModelObject, parameters: null, commandType: commandType);

        public Task<IEnumerable<T>> GetData(
            ISqlSettings settings,
            ISqlDbProviderFactory providerFactory,
            string commandText,
            Func<T> createModelObject,
            IEnumerable<IDataParameter> parameters,
            CommandType commandType = CommandType.StoredProcedure)
        {
            return GetData(
                () => providerFactory.OpenConnection(settings),
                commandText,
                createModelObject,
                parameters,
                commandType);
        }

        public Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, CommandType commandType = CommandType.StoredProcedure)
            => GetData(settings, providerFactory, commandText, createModelObject, assignDataStateManager, null, commandType);

        public async Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, IEnumerable<IDataParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            IEnumerable<T> data = await GetData(settings, providerFactory, commandText, createModelObject, parameters, commandType);
            if (assignDataStateManager != null)
                assignDataStateManager(data);
            return data;
        }
    }
}
