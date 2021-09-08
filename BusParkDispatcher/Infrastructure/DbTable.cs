using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                    Debug.WriteLine(stringVal);
                    return true;
                }
            }

            return false;
        }
    }
}
