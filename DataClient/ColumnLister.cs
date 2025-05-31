using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BrassLoon.DataClient
{
    internal sealed class ColumnLister
    {
        private readonly Dictionary<string, string[]> _columns = new Dictionary<string, string[]>();

        internal static ColumnLister Instance { get; } = new ColumnLister();

        internal string[] GetColumns<T>() => GetColumns(typeof(T));

        internal string[] GetColumns(Type modelType)
        {
            if (modelType == null)
                throw new ArgumentNullException(nameof(modelType));
            string key = modelType.AssemblyQualifiedName;
            if (!_columns.ContainsKey(key))
                _columns[key] = InnerGetColumns(modelType);
            return _columns[key];
        }

        private static string[] InnerGetColumns(Type modelType)
        {
            List<string> columns = new List<string>();
            PropertyInfo[] properties = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                string columnName = GetColumnName(property);
                if (!string.IsNullOrEmpty(columnName))
                    columns.Add(columnName);
            }
            return columns.ToArray();
        }

        private static string GetColumnName(PropertyInfo property)
        {
            string name = string.Empty;
            ColumnMappingAttribute attribute = property.GetCustomAttributes<ColumnMappingAttribute>(true).SingleOrDefault();
            if (attribute != null)
            {
                ColumnMapping mapping = new ColumnMapping { Info = property, MappingAttribute = attribute };
                name = mapping.GetColumnName();
            }
            return name;
        }
    }
}
