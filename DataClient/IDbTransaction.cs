namespace BrassLoon.DataClient
{
    public interface IDbTransaction : System.Data.IDbTransaction
    {
        System.Data.Common.DbTransaction InnerTransaction { get; }
        void AddObserver(IDbTransactionObserver observer);
    }
}