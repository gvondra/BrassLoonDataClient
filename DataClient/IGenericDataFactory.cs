using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    public interface IGenericDataFactory<T>
    {
        int? CommandTimeout { get; set; }
        Task<IEnumerable<T>> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, CommandType commandType = CommandType.StoredProcedure);
        Task<IEnumerable<T>> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, IEnumerable<IDataParameter> parameters, CommandType commandType = CommandType.StoredProcedure);
        Task<IEnumerable<T>> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, CommandType commandType = CommandType.StoredProcedure);
        Task<IEnumerable<T>> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, IEnumerable<IDataParameter> parameters, CommandType commandType = CommandType.StoredProcedure);

        Task<IEnumerable<TModel>> LoadData<TModel>(DbDataReader reader, Func<TModel> createModelObject);
        Task<IEnumerable<TModel>> LoadData<TModel>(DbDataReader reader, Func<TModel> createModelObject, Action<IEnumerable<TModel>> assignDataStateManager);
    }
}