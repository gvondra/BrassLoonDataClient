using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace BrassLoon.DataClient
{
    public class DataStateManager : IDataStateManager
    {
        public DataStateManager(object original)
        {
            this.Original = original;
        }

        public DataStateManager() { }

        public object Original { get; set; }

        public static bool IsByteArrayChanged(byte[] originalValue, byte[] targetValue)
        {
            bool changed = false;
            int i;
            if (originalValue != null && originalValue != targetValue)
            {
                if (originalValue.Length != targetValue.Length)
                {
                    changed = true;
                }
                else
                {
                    i = 0;
                    while (!changed && i < originalValue.Length)
                    {
                        if (originalValue[i] != targetValue[i])
                            changed = true;
                        i += 1;
                    }
                }
            }
            return changed;
        }

        public static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead && p.GetCustomAttributes<ColumnMappingAttribute>(true).Any())
                ;
        }

        public DataState GetState(object target)
        {
            DataState result = DataState.Unchanged;
            if (this.Original == null)
            {
                result = DataState.New;
            }
            else if (target != null && target != Original
                && DataStateManager.GetProperties(target.GetType()).Any((PropertyInfo p) => IsChanged(p, target)))
            {
                result = DataState.Updated;
            }
            return result;
        }

        public bool IsChanged(PropertyInfo property, object target)
            => IsChanged(property, this.Original, target);

        public bool IsChanged(PropertyInfo property, object original, object target)
        {
            bool changed = false;
            object originalValue = property.GetValue(original);
            object targetValue = property.GetValue(target);
            if (originalValue != targetValue)
            {
                if (originalValue == null || targetValue == null)
                {
                    changed = true;
                }
                else if (property.PropertyType.Name == "Nullable`1" && property.PropertyType.GenericTypeArguments.Length > 0)
                {
                    if (IsNullableChanged(property.PropertyType, originalValue, targetValue))
                        changed = true;
                }
                else
                {
                    if (property.PropertyType.Equals(typeof(byte[])))
                    {
                        if (DataStateManager.IsByteArrayChanged((byte[])originalValue, (byte[])targetValue))
                            changed = true;
                    }
                    else if (originalValue.ToString() != targetValue.ToString())
                    {
                        changed = true;
                    }
                }
            }
            return changed;
        }

        public bool IsNullableChanged(Type type, object originalValue, object targetValue)
        {
            bool changed = false;
            PropertyInfo hasValue = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .First((PropertyInfo p) => p.CanRead && p.Name == "HasValue" && p.PropertyType.Equals(typeof(bool)))
                ;
            bool originalHasValue = (bool)hasValue.GetValue(originalValue);
            bool targetHasValue = (bool)hasValue.GetValue(targetValue);
            if (originalHasValue != targetHasValue)
                changed = true;
            if (!changed && originalHasValue == targetHasValue && originalHasValue)
            {
                PropertyInfo valueInfo = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .First((PropertyInfo p) => p.CanRead && p.Name == "Value")
                    ;
                changed = IsChanged(valueInfo, originalValue, targetValue);
            }
            return changed;
        }
    }
}