using System.Reflection;
namespace BrassLoon.DataClient
{
    public enum DataState : short
    {
        New = 0,
        Updated = 1,
        Unchanged = 2
    }

    public interface IDataStateManager
    {
        object Original { get; set; }
        DataState GetState(object target);
        bool IsChanged(PropertyInfo property, object target);
    }
}