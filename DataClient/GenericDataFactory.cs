using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public IEnumerable<T> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject)
        {
            return GetData(settings, providerFactory, commandText, createModelObject, null, CommandType.StoredProcedure);
        }

        public IEnumerable<T> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, IEnumerable<IDataParameter> parameters)
        {
            return GetData(settings, providerFactory, commandText, createModelObject, parameters, CommandType.StoredProcedure);
        }

        public IEnumerable<T> GetData(
            ISettings settings, 
            IDbProviderFactory providerFactory, 
            string commandText, 
            Func<T> createModelObject, 
            IEnumerable<IDataParameter> parameters, 
            CommandType commandType)
        {
            IEnumerable<T> result;
            int parameterCount = 0;
            if (parameters != null)
                parameterCount = parameters.Count();
            using (IDbConnection connection = providerFactory.OpenConnection(settings))
            {
                using (IDbCommand command = connection.CreateCommand())
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
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        result = LoadData(reader, createModelObject);
                        reader.Close();
                    }
                }
                connection.Close();
            }
            return result;
        }

        public IEnumerable<T> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager)
        {
            return GetData(settings, providerFactory, commandText, createModelObject, assignDataStateManager, null, CommandType.StoredProcedure);
        }

        public IEnumerable<T> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, IEnumerable<IDataParameter> parameters)
        {
            return GetData(settings, providerFactory, commandText, createModelObject, assignDataStateManager, parameters, CommandType.StoredProcedure);
        }

        public IEnumerable<T> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, Action<IEnumerable<T>> assignDataStateManager, IEnumerable<IDataParameter> parameters, CommandType commandType)
        {
            IEnumerable<T> data = GetData(settings, providerFactory, commandText, createModelObject, parameters, commandType);
            if (assignDataStateManager != null)
                assignDataStateManager(data);
            return data;
        }

        public IEnumerable<R> LoadData<R>(IDataReader reader, Func<R> createModelObject)
        {
            return this.LoadData(this.LoaderFactory.CreateLoader(), reader, createModelObject);
        }

        public IEnumerable<R> LoadData<R>(IDataReader reader, Func<R> createModelObject, Action<IEnumerable<R>> assignDataStateManager)
        {
            IEnumerable<R> data = LoadData(reader, createModelObject);
            if (assignDataStateManager != null)
            {
                assignDataStateManager(data);
            }
            return data;
        }

        public IEnumerable<R> LoadData<R>(ILoader loader, IDataReader reader, Func<R> createModelObject)
        {
            List<R> result = new List<R>();            
            while (reader.Read())
            {
                R data = createModelObject();
                result.Add((R)loader.Load(data, reader));
            }
            return result;
        }
    }
}