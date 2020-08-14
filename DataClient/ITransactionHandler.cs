using System.Data;
using System.Data.Common;

namespace BrassLoon.DataClient
{
    public interface ITransactionHandler : ISettings
    {
        DbConnection Connection { get; set; }
        IDbTransaction Transaction { get; set; }
    }
}