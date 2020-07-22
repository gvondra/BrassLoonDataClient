using System;
namespace BrassLoon.DataClient
{
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