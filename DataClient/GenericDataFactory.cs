using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    public class GenericDataFactory<T> : IGenericDataFactory<T>
    {
        public ILoaderFactory LoaderFactory { get; set; }
        public int? CommandTimeout { get; set; }

        public GenericDataFactory()
        {
            LoaderFactory = new LoaderFactory();
        }

        public GenericDataFactory(ILoaderFactory loaderFactory)
        {
            LoaderFactory = loaderFactory;
        }

        public Task<IEnumerable<T>> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject)
            => GetData(settings, providerFactory, commandText, createModelObject, null, CommandType.StoredProcedure);

        public Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject)
            => GetData(settings, providerFactory, commandText, createModelObject, null, CommandType.StoredProcedure);

        public Task<IEnumerable<T>> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, IEnumerable<IDataParameter> parameters)
            => GetData(settings, providerFactory, commandText, createModelObject, parameters, CommandType.StoredProcedure);

        public Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, IEnumerable<IDataParameter> parameters)
            => GetData(settings, providerFactory, commandText, createModelObject, parameters, CommandType.StoredProcedure);


        public Task<IEnumerable<T>> GetData(
            ISettings settings,
            IDbProviderFactory providerFactory,
            string commandText,
            Func<T> createModelObject,
            IEnumerable<IDataParameter> parameters,
            CommandType commandType)
        {
            return GetData(
                () => providerFactory.OpenConnection(settings),
                commandText,
                createModelObject,
                parameters,
                commandType
                );
        }


        public Task<IEnumerable<T>> GetData(
            ISqlSettings settings,
            ISqlDbProviderFactory providerFactory,
            string commandText,
            Func<T> createModelObject,
            IEnumerable<IDataParameter> parameters,
            CommandType commandType)
        {
            return GetData(
                () => providerFactory.OpenConnection(settings),
                commandText,
                createModelObject,
                parameters,
                commandType
                );
        }

        public async Task<IEnumerable<T>> GetData(
            Func<Task<DbConnection>> openConnection,
            string commandText,
            Func<T> createModelObject,
            IEnumerable<IDataParameter> parameters,
            CommandType commandType)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentNullException(nameof(commandText));
            IEnumerable<T> result;
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
                        result = await LoadData(reader, createModelObject);
                        reader.Close();
                    }
                }
                connection.Close();
            }
            return result;
        }

        public Task<IEnumerable<T>> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager)
            => GetData(settings, providerFactory, commandText, createModelObject, assignDataStateManager, null, CommandType.StoredProcedure);

        public Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager)
            => GetData(settings, providerFactory, commandText, createModelObject, assignDataStateManager, null, CommandType.StoredProcedure);

        public Task<IEnumerable<T>> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, IEnumerable<IDataParameter> parameters)
            => GetData(settings, providerFactory, commandText, createModelObject, assignDataStateManager, parameters, CommandType.StoredProcedure);

        public Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, IEnumerable<IDataParameter> parameters)
            => GetData(settings, providerFactory, commandText, createModelObject, assignDataStateManager, parameters, CommandType.StoredProcedure);

        public async Task<IEnumerable<T>> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, IEnumerable<IDataParameter> parameters, CommandType commandType)
        {
            IEnumerable<T> data = await GetData(settings, providerFactory, commandText, createModelObject, parameters, commandType);
            if (assignDataStateManager != null)
                assignDataStateManager(data);
            return data;
        }

        public async Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, IEnumerable<IDataParameter> parameters, CommandType commandType)
        {
            IEnumerable<T> data = await GetData(settings, providerFactory, commandText, createModelObject, parameters, commandType);
            if (assignDataStateManager != null)
                assignDataStateManager(data);
            return data;
        }

        public async Task<IEnumerable<TModel>> LoadData<TModel>(DbDataReader reader, Func<TModel> createModelObject)
            => await this.LoadData(this.LoaderFactory.CreateLoader(), reader, createModelObject);

        public async Task<IEnumerable<TModel>> LoadData<TModel>(DbDataReader reader, Func<TModel> createModelObject, Action<IEnumerable<TModel>> assignDataStateManager)
        {
            IEnumerable<TModel> data = await LoadData(reader, createModelObject);
            if (assignDataStateManager != null)
            {
                assignDataStateManager(data);
            }
            return data;
        }

        public async Task<IEnumerable<TModel>> LoadData<TModel>(ILoader loader, DbDataReader reader, Func<TModel> createModelObject)
        {
            List<TModel> result = new List<TModel>();
            while (await reader.ReadAsync())
            {
                TModel data = createModelObject();
                result.Add((TModel)await loader.Load(data, reader));
            }
            return result;
        }
    }
}