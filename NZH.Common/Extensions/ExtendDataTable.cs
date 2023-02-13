using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Common.Extensions
{
    public static class ExtendDataTable
    {
        /// <summary>
        /// 将DataTable转换为list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }
            List<T> result = new List<T>();
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.ToLower().Equals(dt.Columns[i].ColumnName.ToLower()))
                        {
                            if (dt.Rows[j][i] != DBNull.Value)
                            {
                                if (pi.PropertyType.ToString() == "System.Int32")
                                {
                                    pi.SetValue(_t, Int32.Parse(dt.Rows[j][i].ToString()), null);
                                }
                                if (pi.PropertyType.ToString() == "System.DateTime")
                                {
                                    pi.SetValue(_t, System.Convert.ToDateTime(dt.Rows[j][i].ToString()), null);
                                }
                                if (pi.PropertyType.ToString() == "System.String")
                                {
                                    pi.SetValue(_t, dt.Rows[j][i].ToString(), null);
                                }
                                if (pi.PropertyType.ToString() == "System.Boolean")
                                {
                                    pi.SetValue(_t, System.Convert.ToBoolean(dt.Rows[j][i].ToString()), null);
                                }
                            }
                            else
                            {
                                pi.SetValue(_t, null, null);
                            }
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }
    }
}
