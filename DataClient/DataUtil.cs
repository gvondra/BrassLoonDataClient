using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml;
namespace BrassLoon.DataClient 
{
    public static class DataUtil
    {
        public static IDataParameter CreateParameter(IDbProviderFactory providerFactory, DbType type)
        {
            return CreateParameter(providerFactory, null, type);
        }
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
                return value.TrimEnd();
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
                return string.Empty;
        }

        public static void AddParameter(
            IDbProviderFactory providerFactory,
            IList parameterCollection,
            string name,
            DbType dbType,
            object value
        ) 
        {
            parameterCollection.Add(CreateParameter(providerFactory, name, dbType, value));
        }

        public static void AssignDataStateManager(IEnumerable<IDataManagedState> data)
        {
            foreach (IDataManagedState state in data)
            {
                state.Manager = new DataStateManager(state.Clone());
            }
        }
    }
}