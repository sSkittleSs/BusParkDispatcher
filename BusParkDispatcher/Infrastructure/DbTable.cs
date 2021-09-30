using System;
using System.Reflection;

namespace BusParkDispatcher.Infrastructure
{
    public abstract class DbTable
    {
        public virtual bool IsSearchable(string value) => IsSearchable(this, value);

        public static bool IsSearchable(object obj, string value)
        {
            Type type = obj.GetType();
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object val = prop.GetValue(obj);
                if (!(val?.GetType().IsPrimitive ?? true) && !(val is string))
                {
                    continue;
                }
                string stringVal = Convert.ToString(val);
                if (stringVal.ToLower().Contains(value.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
