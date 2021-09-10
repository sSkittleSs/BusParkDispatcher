using System;
using System.Reflection;

namespace BusParkDispatcher.Infrastructure
{
    public class DbTable
    {
        public virtual bool IsSearchable(string value)
        {
            Type type = GetType();
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object val = prop.GetValue(this);
                if (!val.GetType().IsPrimitive && !(val is string))
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
