using System.Data;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class IntegerComponent : ILoaderComponent
    {
        public object GetValue(IDataReader reader, int ordinal)
        {
            object result = null;
            if (!reader.IsDBNull(ordinal))
            {                
                result = reader.GetInt32(ordinal);
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(int)) || mapping.Info.PropertyType.Equals(typeof(int?));
        }
    }
}