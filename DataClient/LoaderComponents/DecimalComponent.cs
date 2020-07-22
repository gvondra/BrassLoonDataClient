using System.Data;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class DecimalComponent : ILoaderComponent
    {
        public object GetValue(IDataReader reader, int ordinal)
        {
            object result = null;
            if (!reader.IsDBNull(ordinal))
            {                
                result = reader.GetDecimal(ordinal);
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(decimal)) || mapping.Info.PropertyType.Equals(typeof(decimal?));
        }
    }
}