using System.Data;
namespace BrassLoon.DataClient
{
    public interface ISqlTransactionHandler : ITransactionHandler, ISqlSettings
    {}
}