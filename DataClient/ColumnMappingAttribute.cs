using System;
namespace BrassLoon.DataClient 
{
    /// This is a model property decorator. This attribute links table columns, by name, to model class properties
    public class ColumnMappingAttribute : Attribute 
    {
        public string ColumnName { get; set; }
        public bool IsPrimaryKey { get; set; } = false;

        public ColumnMappingAttribute(string columnName) 
        {
            this.ColumnName = columnName;
        }
    }
}