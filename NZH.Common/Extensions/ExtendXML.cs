using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NZH.Common.Extensions
{
    public static class ExtendXML
    {
        /// <summary>
        ///  将Xml内容字符串转换成DataSet对象
        ///  需要捕捉异常
        /// </summary>
        /// <param name="xmlStr">Xml内容字符串</param>
        /// <returns>DataSet对象</returns>
        public static DataSet ToDataSet(this string xmlStr)
        {
            DataSet ds = null;
            if (!string.IsNullOrEmpty(xmlStr))
            {
                StringReader StrStream = null;
                XmlTextReader Xmlrdr = null;
                try
                {
                    ds = new DataSet();
                    //读取字符串中的信息
                    StrStream = new StringReader(xmlStr);
                    //获取StrStream中的数据
                    Xmlrdr = new XmlTextReader(StrStream);
                    //ds获取Xmlrdr中的数据                
                    ds.ReadXml(Xmlrdr);
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源
                    if (Xmlrdr != null)
                    {
                        Xmlrdr.Close();
                        StrStream.Close();
                        StrStream.Dispose();
                    }
                }
            }
            return ds;
        }

        /// <summary>
        ///  根据路径读取Xml文件信息,并转换成DataSet对象,路径错误返回null
        ///  需要捕捉异常
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <returns></returns>
        public static DataSet ToDataSetForPath(this string xmlFilePath)
        {
            DataSet ds = null;
            if (!string.IsNullOrEmpty(xmlFilePath))
            {
                if (File.Exists(xmlFilePath))
                {
                    using (StreamReader sr = new StreamReader(xmlFilePath))
                    {
                        string xml = sr.ReadToEnd();
                        try
                        {

                            ds = xml.ToDataSet();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        sr.Close();
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// 将Xml字符串转换成DataTable对象，获取指定tableIndex 
        /// </summary>
        /// <param name="xmlStr">Xml字符串</param>
        /// <param name="tableIndex">Table表索引</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ToDatatTable(this string xmlStr, int tableIndex)
        {
            return xmlStr.ToDataSet().Tables[tableIndex];
        }

        /// <summary>
        /// 将Xml字符串转换成DataTable对象，获取指定tablename 
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static DataTable ToDatatTable(this string xmlStr, string tablename)
        {
            return xmlStr.ToDataSet().Tables[tablename];
        }

        /// <summary>
        /// 根据路径读取Xml文件信息,并转换成DataTable对象，获取指定tableIndex
        /// </summary>
        /// <param name="xmlFilePath">xml文件路径</param>
        /// <param name="tableIndex">Table索引</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ToDataTableForPath(this string xmlFilePath, int tableIndex)
        {
            return xmlFilePath.ToDataSetForPath().Tables[tableIndex];
        }

        /// <summary>
        /// 根据路径读取Xml文件信息,并转换成DataTable对象，获取指定tablename
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static DataTable ToDataTableForPath(this string xmlFilePath, string tablename)
        {
            return xmlFilePath.ToDataSetForPath().Tables[tablename];
        }
    }
}
