using System.Data;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class BytesComponent : ILoaderComponent
    {
        public object GetValue(IDataReader reader, int ordinal)
        {
            object result = null;
            if (!reader.IsDBNull(ordinal))
            {                
                result = (byte[])reader.GetValue(ordinal);
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(byte[]));
        }
    }
}