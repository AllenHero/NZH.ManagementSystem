using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NZH.Common.Extensions
{
    public class Util
    {
        /// <summary>
        /// MD5 16位加密
        /// </summary>
        /// <param name="ConvertString"></param>
        /// <returns></returns>
        public static string GetMd5Str(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }

        /// <summary>
        /// 验证字符串中是否包含特殊字符
        /// </summary>
        /// <param name="str">待判定字符串</param>
        /// <returns>是否为特殊字符（true：包含，false：不包含）</returns>
        public static bool FilterSpecial(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            string[] aryReg = { "'", "'delete", "?", "<", ">", "%", "\"\"", ",", ".", ">=", "=<", "_", ";", "||", "[", "]", "&", "/", "-", "|", " ", "''" };
            for (int i = 0; i < aryReg.Length; i++)
            {
                if (str.Contains(aryReg[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 生成CRC校验
        /// </summary>
        /// <param name="data">byte</param>
        /// <returns></returns>
        public static byte[] CRC16_C(byte[] data)
        {
            byte CRC16Lo;
            byte CRC16Hi;   //CRC寄存器
            byte CL; byte CH;       //多项式码&HA001
            byte SaveHi; byte SaveLo;
            byte[] tmpData;
            int I;
            int Flag;
            CRC16Lo = 0xFF;
            CRC16Hi = 0xFF;
            CL = 0x01;
            CH = 0xA0;
            tmpData = data;
            for (int i = 0; i < tmpData.Length; i++)
            {
                CRC16Lo = (byte)(CRC16Lo ^ tmpData[i]); //每一个数据与CRC寄存器进行异或
                for (Flag = 0; Flag <= 7; Flag++)
                {
                    SaveHi = CRC16Hi;
                    SaveLo = CRC16Lo;
                    CRC16Hi = (byte)(CRC16Hi >> 1);      //高位右移一位
                    CRC16Lo = (byte)(CRC16Lo >> 1);      //低位右移一位
                    if ((SaveHi & 0x01) == 0x01) //如果高位字节最后一位为1
                    {
                        CRC16Lo = (byte)(CRC16Lo | 0x80);   //则低位字节右移后前面补1
                    }             //否则自动补0
                    if ((SaveLo & 0x01) == 0x01) //如果LSB为1，则与多项式码进行异或
                    {
                        CRC16Hi = (byte)(CRC16Hi ^ CH);
                        CRC16Lo = (byte)(CRC16Lo ^ CL);
                    }
                }
            }
            byte[] ReturnData = new byte[2];
            ReturnData[0] = CRC16Lo;       //CRC低位
            ReturnData[1] = CRC16Hi;       //CRC高位
            return ReturnData;
        }

        /// <summary>
        /// 字节数组转为字符串
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string getHexStr(byte[] buffer, int count)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                if (Regex.Match(Convert.ToString(buffer[i], 16), "[a-f0-9]{2}").Value == "")
                    sb.Append("0" + Convert.ToString(buffer[i], 16) + " ");
                else
                    sb.Append(Convert.ToString(buffer[i], 16) + " ");
            }
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 把16进制字符串转换成字节数组
        /// </summary>
        /// <param name="hex">需要转换的字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] hexStringToByte(String hex)
        {
            int len = (hex.Length / 2);
            byte[] result = new byte[len];
            char[] achar = hex.ToCharArray();
            for (int i = 0; i < len; i++)
            {
                int pos = i * 2;
                result[i] = (byte)(toByte(achar[pos]) << 4 | toByte(achar[pos + 1]));
            }
            return result;
        }

        private static byte toByte(char c)
        {
            byte b = (byte)"0123456789ABCDEF".IndexOf(c);
            return b;
        }

        /// <summary>
        /// 将DataSet转化成JSON数据
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string DataSetToJson(DataSet ds)
        {
            string json = string.Empty;
            try
            {
                if (ds.Tables.Count == 0)
                    throw new Exception("DataSet中Tables为0");
                json = "{";
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    json += "T" + (i + 1) + ":[";
                    for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                    {
                        json += "{";
                        for (int k = 0; k < ds.Tables[i].Columns.Count; k++)
                        {
                            json += ds.Tables[i].Columns[k].ColumnName + ":'" + ds.Tables[i].Rows[j][k].ToString() + "'";
                            if (k != ds.Tables[i].Columns.Count - 1)
                                json += ",";
                        }
                        json += "}";
                        if (j != ds.Tables[i].Rows.Count - 1)
                            json += ",";
                    }
                    json += "]";
                    if (i != ds.Tables.Count - 1)
                        json += ",";


                }
                json += "}";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return json;
        }

        /// <summary>
        /// 实体类转换成DataTable
        /// 调用示例：DataTable dt= FillDataTable(Entitylist.ToList());
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static DataSet FillDataTable<T>(List<T> modelList)
        {
            DataSet ds = new DataSet();
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateData(modelList[0]);//创建表结构

            foreach (T model in modelList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                }
                dt.Rows.Add(dataRow);
            }
            ds.Tables.Add(dt);
            return ds;
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public static DataTable CreateData<T>(T model)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (propertyInfo.Name != "CTimestamp")//些字段为oracle中的Timesstarmp类型
                {
                    dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
                }
                else
                {
                    dataTable.Columns.Add(new DataColumn(propertyInfo.Name, typeof(DateTime)));
                }
            }
            return dataTable;
        }

        public static bool IsInherit(global::System.Type type, global::System.Type BaseType)
        {
            if (type.BaseType == null) return false;
            if (type.BaseType == BaseType) return true;
            return IsInherit(type.BaseType, BaseType);
        }

        /// <summary>
        /// DATATABLE转化成LIST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mTable"></param>
        /// <returns></returns>
        public static List<T> DataTableConvertList<T>(DataTable mTable)
        {
            try
            {
                List<T> mList = new List<T>();
                var mT = default(T);

                string mTempName = string.Empty;

                foreach (DataRow mRow in mTable.Rows)
                {
                    mT = Activator.CreateInstance<T>();
                    var mProperTypes = mT.GetType().GetProperties();
                    foreach (var mPro in mProperTypes)
                    {
                        mTempName = mPro.Name;
                        if (mTable.Columns.Contains(mTempName))
                        {
                            var mValue = mRow[mTempName];
                            if (mValue != DBNull.Value)
                                mPro.SetValue(mT, mValue, null);
                        }

                    }
                    mList.Add(mT);
                }
                return mList;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
    }
}
