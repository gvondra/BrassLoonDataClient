using System;
using System.Globalization;

namespace BrassLoon.DataClient
{
    /// <summary>
    /// Thrown when the data model class includes a column that is not found in the actual data set.
    /// Ask:
    /// - Does your data model include a column it shouldn't?
    /// - Do you need to add a column to query result set?
    /// </summary>
    public class SourceColumnNotFoundException : ApplicationException
    {
        public SourceColumnNotFoundException(string columnName)
        : base(string.Format(CultureInfo.InvariantCulture, "The dataset does not include the column \"{0}\"", columnName))
        { }

        public SourceColumnNotFoundException(string columnName, Exception exception)
        : base(string.Format(CultureInfo.InvariantCulture, "The dataset does not include the column \"{0}\"", columnName), exception)
        { }
    }
}