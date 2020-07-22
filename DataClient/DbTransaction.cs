using System.Collections.Generic;
using System.Data;
namespace BrassLoon.DataClient 
{
    public class DbTransaction : IDbTransaction
    {
        private System.Data.IDbTransaction _innerTransaction;
        private List<IDbTransactionObserver> _observers = new List<IDbTransactionObserver>();

        public DbTransaction(System.Data.IDbTransaction transaction)
        {
            _innerTransaction = transaction;
        }

        public System.Data.IDbTransaction InnerTransaction => _innerTransaction;
        public IDbConnection Connection => _innerTransaction.Connection;
        public IsolationLevel IsolationLevel => _innerTransaction.IsolationLevel;

        public void AddObserver(IDbTransactionObserver observer)
        {
            _observers.Add(observer);
        }

        public void Commit() 
        {
            foreach (IDbTransactionObserver observer in _observers)
            {
                observer.BeforeCommit();
            }
            _innerTransaction.Commit();
            foreach (IDbTransactionObserver observer in _observers)
            {
                observer.AfterCommit();
            }
        }

        public void Dispose() 
        {
            _innerTransaction.Dispose();
            _innerTransaction = null;
        }

        public void Rollback()
        {
            foreach (IDbTransactionObserver observer in _observers)
            {
                observer.BeforeRollback();
            }
            _innerTransaction.Rollback();
            foreach (IDbTransactionObserver observer in _observers)
            {
                observer.AfterRollback();
            }
        }
    }
}