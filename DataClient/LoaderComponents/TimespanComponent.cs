using System;
using System.Data;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class TimespanComponent : ILoaderComponent
    {
        public object GetValue(IDataReader reader, int ordinal)
        {
            object result = null;
            if (!reader.IsDBNull(ordinal))
            {                
                result = (TimeSpan)reader.GetValue(ordinal);
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(TimeSpan)) || mapping.Info.PropertyType.Equals(typeof(TimeSpan?));
        }
    }
}