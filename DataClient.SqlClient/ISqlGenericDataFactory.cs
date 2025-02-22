using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BrassLoon.DataClient.SqlClient
{
    public interface ISqlGenericDataFactory<T> : IGenericDataFactory<T>
    {
        Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, CommandType commandType = CommandType.StoredProcedure);
        Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, IEnumerable<IDataParameter> parameters, CommandType commandType = CommandType.StoredProcedure);
        Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, CommandType commandType = CommandType.StoredProcedure);
        Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, IEnumerable<IDataParameter> parameters, CommandType commandType = CommandType.StoredProcedure);
    }
}
