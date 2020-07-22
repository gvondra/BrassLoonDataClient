using System.Data;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class DoubleComponent : ILoaderComponent
    {
        public object GetValue(IDataReader reader, int ordinal)
        {
            object result = null;
            if (!reader.IsDBNull(ordinal))
            {                
                result = reader.GetDouble(ordinal);
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(double)) || mapping.Info.PropertyType.Equals(typeof(double?));
        }
    }
}