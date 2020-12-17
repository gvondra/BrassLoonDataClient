using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
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
        {
            return GetData(settings, providerFactory, commandText, createModelObject, null, CommandType.StoredProcedure);
        }

        public Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject)
        {
            return GetData(settings, providerFactory, commandText, createModelObject, null, CommandType.StoredProcedure);
        }

        public Task<IEnumerable<T>> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, IEnumerable<IDataParameter> parameters)
        {
            return GetData(settings, providerFactory, commandText, createModelObject, parameters, CommandType.StoredProcedure);
        }

        public Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, IEnumerable<IDataParameter> parameters)
        {
            return GetData(settings, providerFactory, commandText, createModelObject, parameters, CommandType.StoredProcedure);
        }


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
            IEnumerable<T> result;
            int parameterCount = 0;
            if (parameters != null)
                parameterCount = parameters.Count();
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
        {
            return GetData(settings, providerFactory, commandText, createModelObject, assignDataStateManager, null, CommandType.StoredProcedure);
        }

        public Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager)
        {
            return GetData(settings, providerFactory, commandText, createModelObject, assignDataStateManager, null, CommandType.StoredProcedure);
        }

        public Task<IEnumerable<T>> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, IEnumerable<IDataParameter> parameters)
        {
            return GetData(settings, providerFactory, commandText, createModelObject, assignDataStateManager, parameters, CommandType.StoredProcedure);
        }

        public Task<IEnumerable<T>> GetData(ISqlSettings settings, ISqlDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, IEnumerable<IDataParameter> parameters)
        {
            return GetData(settings, providerFactory, commandText, createModelObject, assignDataStateManager, parameters, CommandType.StoredProcedure);
        }

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

        public async Task<IEnumerable<R>> LoadData<R>(DbDataReader reader, Func<R> createModelObject)
        {
            return await this.LoadData(this.LoaderFactory.CreateLoader(), reader, createModelObject);
        }

        public async Task<IEnumerable<R>> LoadData<R>(DbDataReader reader, Func<R> createModelObject, Action<IEnumerable<R>> assignDataStateManager)
        {
            IEnumerable<R> data = await LoadData(reader, createModelObject);
            if (assignDataStateManager != null)
            {
                assignDataStateManager(data);
            }
            return data;
        }

        public async Task<IEnumerable<R>> LoadData<R>(ILoader loader, DbDataReader reader, Func<R> createModelObject)
        {
            List<R> result = new List<R>();            
            while (await reader.ReadAsync())
            {
                R data = createModelObject();
                result.Add((R)(await loader.Load(data, reader)));
            }
            return result;
        }
    }
}