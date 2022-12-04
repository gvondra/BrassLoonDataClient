using System;
namespace BrassLoon.DataClient
{
    /// <summary>
    /// Thrown when the data model class includes a column that is not found in the actual data set.
    /// Ask:
    /// - Does your data model include a column it shouldn't?
    /// - Do you need to add a column to query result set?
    /// </summary>
    public class SourceColumnNotFound : ApplicationException
    {
        public SourceColumnNotFound(string columnName)
        : base(string.Format("The dataset does not include the column \"{0}\"", columnName))
        {}

        public SourceColumnNotFound(string columnName, Exception exception)
        : base(string.Format("The dataset does not include the column \"{0}\"", columnName), exception)
        {}
    }
}