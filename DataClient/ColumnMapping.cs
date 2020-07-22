using System.Reflection;
namespace BrassLoon.DataClient 
{
    public class ColumnMapping
    {
        public ColumnMappingAttribute ColumnMappingAttribute { get; set; }
        public PropertyInfo Info { get; set; }

        public void SetValue(object model, object value) 
        {
            Info.SetValue(model, value);
        }
    }
}