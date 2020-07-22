using System.Data;
namespace BrassLoon.DataClient.LoaderComponents
{
    public class BooleanComponent : ILoaderComponent
    {
        public object GetValue(IDataReader reader, int ordinal)
        {
            object result = null;
            if (!reader.IsDBNull(ordinal))
            {
                if (reader.GetFieldType(ordinal) == typeof(string))
                {
                    string valueString = reader.GetString(ordinal).Trim();
                    if (!string.IsNullOrEmpty(valueString))
                        result = valueString.ToUpper().StartsWith("Y");
                }
                else
                    result = reader.GetBoolean(ordinal);
            }
            return result;
        }

        public bool IsApplicable(ColumnMapping mapping)
        {
            return mapping.Info.PropertyType.Equals(typeof(bool)) || mapping.Info.PropertyType.Equals(typeof(bool?));
        }
    }
}