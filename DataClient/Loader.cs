using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
namespace BrassLoon.DataClient
{
    public class Loader : ILoader
    {
        private static Dictionary<Type, List<ColumnMapping>> _columnMappings;
        private static AutoResetEvent _mappingsLock;

        static Loader()
        {
            _mappingsLock = new AutoResetEvent(true);
            _columnMappings = new Dictionary<Type, List<ColumnMapping>>();
        }

        public List<ILoaderComponent> Components { get; set; }

        /// data is the model that values will be assigned to
        public object Load(object data, IDataReader reader)
        {
            IEnumerable<ColumnMapping> columnMappings = GetColumnMappings(data);
            int ordinal;
            foreach (ColumnMapping columnMapping in columnMappings)
            {
                try
                {
                    ordinal = reader.GetOrdinal(columnMapping.MappingAttribute.ColumnName);
                }
                catch (Exception ex)
                {
                    throw new SourceColumnNotFound(columnMapping.MappingAttribute.ColumnName, ex);
                }
                if (ordinal >= 0)
                {
                    columnMapping.SetValue(data, GetValue(reader, ordinal, columnMapping));
                }
            }
            return data;
        }

        private object GetValue(IDataReader reader, int ordinal, ColumnMapping columnMapping)
        {
            bool found = false;
            IEnumerator<ILoaderComponent> enumerator;
            object value = null;
            if (Components != null)
            {
                enumerator = Components.GetEnumerator();
                while (!found && enumerator.MoveNext())
                {
                    if (enumerator.Current.IsApplicable(columnMapping))
                    {
                        found = true;
                        value = enumerator.Current.GetValue(reader, ordinal);
                    }
                }
            }
            if (!found)
                throw new LoaderComponentNotFound(columnMapping);
            return value;
        }

        private List<ColumnMapping> GetColumnMappings(object data)
        {
            Type type = data.GetType();
            if (!_columnMappings.ContainsKey(type))
            {
                _mappingsLock.WaitOne();
                try
                {
                    if (!_columnMappings.ContainsKey(type))
                        _columnMappings.Add(type, LoadColumnMappings(type));
                }
                finally
                {
                    _mappingsLock.Set();
                }
            }
            return _columnMappings[type];
        }

        private List<ColumnMapping> LoadColumnMappings(Type type)
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
    }
}