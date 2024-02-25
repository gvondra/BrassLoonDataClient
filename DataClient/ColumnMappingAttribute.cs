using System;
namespace BrassLoon.DataClient
{
    // This is a model property decorator. This attribute links table columns, by name, to model class properties
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnMappingAttribute : Attribute
    {
        public ColumnMappingAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }

        public ColumnMappingAttribute()
            : this(string.Empty)
        { }

        public string ColumnName { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsUtc { get; set; }
        public bool IsOptional { get; set; }
    }
}