using System;
using System.Collections.Generic;
using System.Data;
namespace BrassLoon.DataClient
{
    public class DbTransaction : IDbTransaction
    {
        private System.Data.Common.DbTransaction _innerTransaction;
        private readonly List<IDbTransactionObserver> _observers = new List<IDbTransactionObserver>();

        public DbTransaction(System.Data.Common.DbTransaction transaction)
        {
            _innerTransaction = transaction;
        }

        public System.Data.Common.DbTransaction InnerTransaction => _innerTransaction;
        public IDbConnection Connection => _innerTransaction.Connection;
        public IsolationLevel IsolationLevel => _innerTransaction.IsolationLevel;

        public void AddObserver(IDbTransactionObserver observer) => _observers.Add(observer);

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
            _observers.Clear();
            _innerTransaction.Dispose();
            _innerTransaction = null;
            GC.SuppressFinalize(this);
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