using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BrassLoon.DataClient
{
    public class Loader : ILoader
    {
        private static readonly Dictionary<Type, List<ColumnMapping>> _columnMappings = new Dictionary<Type, List<ColumnMapping>>();
        private static readonly AutoResetEvent _mappingsLock = new AutoResetEvent(true);

        public List<ILoaderComponent> Components { get; set; }

        // data is the model that values will be assigned to
        public async Task<object> Load(object data, DbDataReader reader)
        {
            IEnumerable<ColumnMapping> columnMappings = Loader.GetColumnMappings(data);
            Dictionary<string, int> fields = Loader.GetFields(reader);
            int ordinal;
            foreach (ColumnMapping columnMapping in columnMappings)
            {
                string columnName = columnMapping.GetColumnName();
                if (!fields.TryGetValue(columnName, out ordinal))
                    ordinal = -1;
                if (!columnMapping.IsOptional && ordinal < 0)
                    throw new SourceColumnNotFoundException(columnName);
                if (ordinal >= 0)
                {
                    columnMapping.SetValue(data, await GetValue(reader, ordinal, columnMapping));
                }
            }
            return data;
        }

        private static List<ColumnMapping> GetColumnMappings(object data)
        {
            Type type = data.GetType();
            if (!_columnMappings.ContainsKey(type))
            {
                _mappingsLock.WaitOne();
                try
                {
                    if (!_columnMappings.ContainsKey(type))
                        _columnMappings.Add(type, Loader.LoadColumnMappings(type));
                }
                finally
                {
                    _mappingsLock.Set();
                }
            }
            return _columnMappings[type];
        }

        private static List<ColumnMapping> LoadColumnMappings(Type type)
        {
            List<ColumnMapping> mappings = new List<ColumnMapping>();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties != null)
            {
                for (int i = 0; i < properties.Length; i += 1)
                {
                    if (properties[i].CanWrite)
                    {
                        foreach (ColumnMappingAttribute attribute in properties[i].GetCustomAttributes<ColumnMappingAttribute>(true))
                        {
                            mappings.Add(new ColumnMapping() { MappingAttribute = attribute, Info = properties[i] });
                        }
                    }
                }
            }
            return mappings;
        }

        private static Dictionary<string, int> GetFields(DbDataReader reader)
        {
            Dictionary<string, int> fields = new Dictionary<string, int>();
            for (int i = 0; i < reader.FieldCount; i += 1)
            {
                fields.Add(reader.GetName(i), i);
            }
            return fields;
        }

        private async Task<object> GetValue(DbDataReader reader, int ordinal, ColumnMapping columnMapping)
        {
            bool found = false;
            object value = null;
            if (Components != null)
            {
                ILoaderComponent loaderComponent = Components.Find(c => c.IsApplicable(columnMapping));
                if (loaderComponent != null)
                {
                    value = await loaderComponent.GetValue(reader, ordinal, columnMapping);
                    found = true;
                }
            }
            if (!found)
                throw new LoaderComponentNotFoundException(columnMapping);
            return value;
        }
    }
}