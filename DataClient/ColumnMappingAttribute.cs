using System;
namespace BrassLoon.DataClient
{
    /// This is a model property decorator. This attribute links table columns, by name, to model class properties
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnMappingAttribute : Attribute
    {
        public string ColumnName { get; set; } = string.Empty;
        public bool IsPrimaryKey { get; set; }
        public bool IsUtc { get; set; }
        public bool IsOptional { get; set; }

        public ColumnMappingAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }

        public ColumnMappingAttribute()
            : this(string.Empty)
        { }
    }
}