using System;
using System.Collections.Generic;
using System.Data;
namespace BrassLoon.DataClient
{
    public class DbTransaction : IDbTransaction
    {
        private readonly List<IDbTransactionObserver> _observers = new List<IDbTransactionObserver>();
        private System.Data.Common.DbTransaction _innerTransaction;
        private bool _disposedValue;

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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _observers.Clear();
                    _innerTransaction.Dispose();
                    _innerTransaction = null;
                }
                _disposedValue = true;
            }
        }
    }
}