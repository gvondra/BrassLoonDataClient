using System;
namespace BrassLoon.DataClient 
{
    /// This is a model property decorator. This attribute links table columns, by name, to model class properties
    public class ColumnMappingAttribute : Attribute 
    {
        public string ColumnName { get; set; } = string.Empty;
        public bool IsPrimaryKey { get; set; } = false;
        public bool IsUtc { get; set; } = false;
        public bool IsOptional { get; set; } = false;

        public ColumnMappingAttribute(string columnName) 
        {
            this.ColumnName = columnName;
        }

        public ColumnMappingAttribute()
            : this(string.Empty)
        { }
        
    }
}