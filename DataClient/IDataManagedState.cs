using System;
namespace BrassLoon.DataClient
{
    public interface IDataManagedState : ICloneable, IDbTransactionObserver
    {
        IDataStateManager Manager { get; set; }
        void AcceptChanges();
    }
}