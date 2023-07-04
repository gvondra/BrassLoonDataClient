using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Xml;
namespace BrassLoon.DataClient
{
    public static class DataUtil
    {
        public static IDataParameter CreateParameter(IDbProviderFactory providerFactory, DbType type) => CreateParameter(providerFactory, null, type);

        public static IDataParameter CreateParameter(IDbProviderFactory providerFactory, string name, DbType type)
        {
            IDataParameter parameter = providerFactory.CreateParameter();
            parameter.DbType = type;
            if (!string.IsNullOrEmpty(name)) { parameter.ParameterName = name; }
            return parameter;
        }

        public static IDataParameter CreateParameter(
            IDbProviderFactory providerFactory,
            string name,
            DbType dbType,
            object value
        )
        {
            IDataParameter parameter = CreateParameter(providerFactory, name, dbType);
            parameter.Value = value;
            return parameter;
        }

        public static object GetParameterValue(Guid? value)
        {
            if (value.HasValue && !value.Value.Equals(Guid.Empty))
                return value.Value;
            else
                return DBNull.Value;
        }

        /// <summary>
        /// If value is null, then returns an empty string or DBNull depending upon the value of treatNullAsEmpty.
        /// If value is not null, then value is returned with white space trimmed from the end.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="treatNullAsEmpty">Directs how null values are handled</param>
        /// <returns></returns>
        public static object GetParameterValue(string value, bool treatNullAsEmpty = true)
        {
            if (value == null)
            {
                if (treatNullAsEmpty)
                    return string.Empty;
                else
                    return DBNull.Value;
            }
            else
            {
                return value.TrimEnd();
            }
        }

        public static object GetParameterValue(decimal? value)
        {
            if (value.HasValue)
                return value.Value;
            else
                return DBNull.Value;
        }

        public static object GetParameterValue(double? value)
        {
            if (value.HasValue)
                return value.Value;
            else
                return DBNull.Value;
        }

        public static object GetParameterValue(long? value)
        {
            if (value.HasValue)
                return value.Value;
            else
                return DBNull.Value;
        }

        public static object GetParameterValue(int? value)
        {
            if (value.HasValue)
                return value.Value;
            else
                return DBNull.Value;
        }

        public static object GetParameterValue(short? value)
        {
            if (value.HasValue)
                return value.Value;
            else
                return DBNull.Value;
        }

        public static object GetParameterValue(byte? value)
        {
            if (value.HasValue)
                return value.Value;
            else
                return DBNull.Value;
        }

        public static object GetParameterValue(DateTime? value)
        {
            if (value.HasValue)
                return value.Value;
            else
                return DBNull.Value;
        }

        public static object GetParameterValue(byte[] value)
        {
            if (value == null)
                return DBNull.Value;
            else
                return value;
        }

        public static object GetParameterValue(XmlNode value)
        {
            if (value == null)
                return DBNull.Value;
            else
                return value.OuterXml;
        }

        public static object GetParameterValue(bool? value)
        {
            if (value.HasValue)
                return value.Value;
            else
                return DBNull.Value;
        }

        public static object GetParameterValueString(bool? value, string trueResponse = "Y", string falseResponse = "N")
        {
            if (value.HasValue)
            {
                if (value.Value)
                    return trueResponse;
                else
                    return falseResponse;
            }
            else
            {
                return string.Empty;
            }
        }

        public static void AddParameter(
            IDbProviderFactory providerFactory,
            IList parameterCollection,
            string name,
            DbType dbType,
            object value)
            => parameterCollection.Add(CreateParameter(providerFactory, name, dbType, value));

        public static void AssignDataStateManager(IEnumerable<IDataManagedState> data)
        {
            foreach (IDataManagedState state in data)
            {
                state.Manager = new DataStateManager(state.Clone());
            }
        }

        /// <summary>
        /// Executes the named stored proc. The resultant data set is read, and the first field stored to list.
        /// </summary>
        /// <typeparam name="T">Type of resultant list and target field</typeparam>
        /// <param name="providerFactory"></param>
        /// <param name="settings"></param>
        /// <param name="storedProcedureName">name of stored proc. to execute</param>
        /// <param name="dataParameters">data parameters passed to the stroed proc.</param>
        /// <returns>List of values from the executed stored proc.</returns>
        public static async Task<IEnumerable<T>> ReadList<T>(
            IDbProviderFactory providerFactory,
            ISettings settings,
            string storedProcedureName,
            params IDataParameter[] dataParameters
            )
        {
            List<T> items = new List<T>();
            using (DbConnection connection = await providerFactory.OpenConnection(settings))
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    if (dataParameters != null && dataParameters.Length > 0)
                    {
                        for (int i = 0; i < dataParameters.Length; i += 1)
                        {
                            command.Parameters.Add(dataParameters[i]);
                        }
                    }
                    using (DbDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (await reader.IsDBNullAsync(0))
                                items.Add(default(T));
                            else
                                items.Add(await reader.GetFieldValueAsync<T>(0));
                        }
                    }
                }
            }
            return items;
        }
    }
}