using System.Data;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class ShortComponent : ILoaderComponent
    {
        public object GetValue(IDataReader reader, int ordinal)
        {
            object result = null;
            if (!reader.IsDBNull(ordinal))
            {                
                result = reader.GetInt16(ordinal);
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(short)) || mapping.Info.PropertyType.Equals(typeof(short?));
        }
    }
}