namespace BrassLoon.DataClient
{
    public interface IDbTransactionObserver
    {
        void BeforeCommit();
        void AfterCommit();
        void BeforeRollback();
        void AfterRollback();
    }
}