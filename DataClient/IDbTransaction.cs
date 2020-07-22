namespace BrassLoon.DataClient
{
    public interface IDbTransaction : System.Data.IDbTransaction
    {
        System.Data.IDbTransaction InnerTransaction { get; }
        void AddObserver(IDbTransactionObserver observer);
    }
}