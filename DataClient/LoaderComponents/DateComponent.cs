using System;
using System.Data;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class DateComponent : ILoaderComponent
    {
        public object GetValue(IDataReader reader, int ordinal)
        {
            object result = null;
            if (!reader.IsDBNull(ordinal))
            {                
                result = reader.GetDateTime(ordinal);
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(DateTime)) || mapping.Info.PropertyType.Equals(typeof(DateTime?));
        }
    }
}