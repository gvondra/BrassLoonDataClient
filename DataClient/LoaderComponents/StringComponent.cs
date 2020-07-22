using System.Data;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class StringComponent : ILoaderComponent
    {
        public object GetValue(IDataReader reader, int ordinal)
        {
            object result = null;
            if (!reader.IsDBNull(ordinal))
            {                
                result = reader.GetString(ordinal).TrimEnd();
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(string));
        }
    }
}