using System.Data;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class ByteComponent : ILoaderComponent
    {
        public object GetValue(IDataReader reader, int ordinal)
        {
            object result = null;
            if (!reader.IsDBNull(ordinal))
            {                
                result = reader.GetByte(ordinal);
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(byte)) || mapping.Info.PropertyType.Equals(typeof(byte?));
        }
    }
}