using System;
using System.Data;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class GuidComponent : ILoaderComponent
    {
        public object GetValue(IDataReader reader, int ordinal)
        {
            object result = null;
            if (!reader.IsDBNull(ordinal))
            {                
                if (reader.GetFieldType(ordinal).Equals(typeof(string)))
                    result = Guid.Parse(reader.GetString(ordinal).Trim());
                else if (reader.GetFieldType(ordinal).Equals(typeof(byte[])))
                    result = new Guid((byte[])reader.GetValue(ordinal));
                else
                    result = (Guid)reader.GetValue(ordinal);
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(Guid)) || mapping.Info.PropertyType.Equals(typeof(Guid?));
        }
    }
}