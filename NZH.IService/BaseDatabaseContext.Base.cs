using NZH.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.IService
{
    public abstract partial class BaseDataBaseContext
    {
        /// <summary>
        /// 获取数据源连接字符串。
        /// </summary>
        public abstract string ConnectionString();

        /// <summary>
        /// 获取数据源连接字符串。
        /// </summary>
        public abstract string ConnectionString(ConnectionType connectionType);

        /// <summary>
        /// 获取指定的数据源字符串。
        /// </summary>
        /// <param name="connectionConfigurationName">数据源配置节名称。</param>
        /// <returns>数据源连接字符串</returns>
        public abstract string GetConnectionString(string connectionConfigurationName);

        /// <summary>
        /// 获取一个新的数据库连接。
        /// </summary>
        /// <param name="connectionString">数据源连接字符串。</param>
        /// <returns></returns>
        public abstract DbConnection CreateConnection(ConnectionType connectionType);

        /// <summary>
        /// 获取一个新的数据库连接。
        /// </summary>
        /// <returns></returns>
        public abstract DbConnection CreateConnection();

        /// <summary>
        /// 创建 DbCommand 对象。
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public abstract DbCommand CreateCommand(DbConnection conn);

        /// <summary>
        /// 创建 DbCommand 对象。
        /// </summary>
        /// <param name="cmdText">SQL 命令。</param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public abstract DbCommand CreateCommand(string cmdText, DbConnection conn);
    }
}
