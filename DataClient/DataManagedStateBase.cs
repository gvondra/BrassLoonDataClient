namespace BrassLoon.DataClient 
{
    public abstract class DataManagedStateBase : IDataManagedState
    {
        public IDataStateManager Manager { get; set; } = new DataStateManager();

        public void AcceptChanges() 
        {
            if (Manager != null) { Manager.Original = Clone(); }
        }

        public void AfterCommit()
        {
            AcceptChanges();
        }

        public void BeforeCommit() {  } // nothing
        public void AfterRollback() {} // nothing
        public void BeforeRollback() {} // nothing

        public object Clone() 
        {
            return this.MemberwiseClone();
        }
    }
}