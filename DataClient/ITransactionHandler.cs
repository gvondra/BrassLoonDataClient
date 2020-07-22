using System.Data;
namespace BrassLoon.DataClient
{
    public interface ITransactionHandler : ISettings
    {
        IDbConnection Connection { get; set; }
        IDbTransaction Transaction { get; set; }
    }
}