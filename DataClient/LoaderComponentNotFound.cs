using System;
using System.Globalization;

namespace BrassLoon.DataClient
{
    public class LoaderComponentNotFound : ApplicationException
    {
        public LoaderComponentNotFound(ColumnMapping columnMapping)
        : base(string.Format(CultureInfo.InvariantCulture, "Loader not found for property {0} on {1}", columnMapping.Info.Name, columnMapping.Info.DeclaringType.FullName))
        { }
    }
}