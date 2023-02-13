using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Common.Extensions
{
    /// <summary>
    /// 数据分页扩展方法.
    /// </summary>
    public static class DataPagingExtension
    {
        /// <summary>
        /// 获取数据分页后的 SQL 语句。
        /// </summary>
        /// <param name="value">需要进行分页的 SQL 查询语句。</param>
        /// <param name="pageIndex">当前页索引，页数由 1 开始。</param>
        /// <param name="pageSize">每页的记录数。</param>
        /// <returns></returns>
        public static string Paging(this string value, Int32 pageIndex = 1, Int32 pageSize = 20)
        {
            const string SQL_PAGING = @"SELECT * FROM (SELECT A.*, ROWNUM RN_ FROM ({0}) A WHERE ROWNUM <= {2}) WHERE RN_ > {1}";
            int firstRowIndex = (pageIndex - 1) * pageSize;
            int lastRowIndex = firstRowIndex + pageSize;
            return string.Format(SQL_PAGING, value, firstRowIndex, lastRowIndex);
        }

        /// <summary>
        /// 获取获得表总行数的 SQL 语句。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TotalCount(this string value)
        {
            const string SQL_TOTALCOUNT = @"SELECT MAX(ROWNUM) AS RST FROM ({0})";
            return string.Format(SQL_TOTALCOUNT, value);
        }
    }
}
