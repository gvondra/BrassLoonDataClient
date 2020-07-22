using System.Data;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class LongComponent : ILoaderComponent
    {
        public object GetValue(IDataReader reader, int ordinal)
        {
            object result = null;
            if (!reader.IsDBNull(ordinal))
            {                
                result = reader.GetInt64(ordinal);
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(long)) || mapping.Info.PropertyType.Equals(typeof(long?));
        }
    }
}