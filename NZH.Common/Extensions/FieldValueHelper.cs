using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Common.Extensions
{
    public static class FieldValueHelper
    {
        /// <summary>
        /// 转换int16
        /// 出错返回默认值
        /// 默认值默认为0
        /// </summary>
        /// <param name="value">要转换值</param>
        /// <param name="defaultValue">默认值,出错返回默认值</param>
        /// <returns></returns>
        public static Int16 ToInt16(this object value, params Int16[] defaultValue)
        {
            Int16 revalue = 0;
            if (defaultValue.Length > 0)
            {
                try
                {
                    revalue = System.Convert.ToInt16(value);//如果为null肯定转换出错
                }
                catch
                {
                    revalue = defaultValue[0];
                }
            }
            else
            {
                try
                {
                    revalue = System.Convert.ToInt16(value);
                }
                catch
                {
                    revalue = 0;
                }
            }
            return revalue;
        }

        /// <summary>
        /// 转换int32
        /// 出错返回默认值
        /// 默认值默认为0
        /// </summary>
        /// <param name="value">要转换值</param>
        /// <param name="defaultValue">默认值,出错返回默认值</param>
        /// <returns></returns>
        public static Int32 ToInt32(this object value, params Int32[] defaultValue)
        {
            Int32 revalue = 0;
            if (defaultValue.Length > 0)
            {
                try
                {
                    revalue = System.Convert.ToInt32(value);//如果为null肯定转换出错
                }
                catch
                {
                    revalue = defaultValue[0];
                }
            }
            else
            {
                try
                {
                    revalue = System.Convert.ToInt32(value);
                }
                catch
                {
                    revalue = 0;
                }
            }
            return revalue;
        }

        /// <summary>
        /// 转换Int64
        /// 出错返回默认值
        /// 默认值默认为0
        /// </summary>
        /// <param name="value">要转换值</param>
        /// <param name="defaultValue">默认值,出错返回默认值</param>
        /// <returns></returns>
        public static Int64 ToInt64(this object value, params Int64[] defaultValue)
        {
            Int64 revalue = 0;
            if (defaultValue.Length > 0)
            {
                try
                {
                    revalue = System.Convert.ToInt64(value);//如果为null肯定转换出错
                }
                catch
                {
                    revalue = defaultValue[0];
                }
            }
            else
            {
                try
                {
                    revalue = System.Convert.ToInt64(value);
                }
                catch
                {
                    revalue = 0;
                }
            }
            return revalue;
        }

        /// <summary>
        /// 转换Double
        /// 出错返回默认值
        /// 默认值默认为0
        /// </summary>
        /// <param name="value">要转换值</param>
        /// <param name="defaultValue">默认值,出错返回默认值</param>
        /// <returns></returns>
        public static Double ToDouble(this object value, params Double[] defaultValue)
        {
            Double revalue = 0;
            try
            {
                revalue = System.Convert.ToDouble(value);
            }
            catch
            {
                if (defaultValue.Length > 0)
                {
                    revalue = defaultValue[0];
                }
            }
            return revalue;
        }

        /// <summary>
        /// 转换Decimal
        /// 出错返回默认值
        /// 默认值默认为0
        /// </summary>
        /// <param name="value">要转换值</param>
        /// <param name="defaultValue">默认值,出错返回默认值</param>
        /// <returns></returns>
        public static Decimal ToDecimal(this object value, params Decimal[] defaultValue)
        {
            Decimal revalue = 0;
            try
            {
                revalue = System.Convert.ToDecimal(value);
            }
            catch
            {
                if (defaultValue.Length > 0)
                {
                    revalue = defaultValue[0];
                }
            }
            return revalue;
        }

        /// <summary>
        /// 转换Boolean
        /// 要转换值为1，结果为true ;  要转换值为0，结果为false ; 
        /// </summary>
        /// <param name="value">要转换值</param>
        /// <returns></returns>
        public static Boolean ToBoolean(this object value)
        {
            Boolean revalue = false;
            try
            {
                if (value.ToString() == "1")
                {
                    revalue = true;
                }
                else if (value.ToString() == "0")
                {
                    revalue = false;
                }
                else if (value.ToString().Trim().ToLower() == "true")
                {
                    revalue = true;
                }
                else if (value.ToString().Trim().ToLower() == "false")
                {
                    revalue = false;
                }
            }
            catch
            {
                revalue = false;
            }
            return revalue;
        }

        /// <summary>
        /// 转换Base64数字编号
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase64String(this byte[] value)
        {
            try
            {
                return System.Convert.ToBase64String(value);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
