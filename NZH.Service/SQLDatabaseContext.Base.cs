using NZH.IService;
using NZH.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Service
{
    public partial class SQLDatabaseContext : BaseDatabaseContext
    {
        /// <summary>
        /// 获取数据源连接字符串。
        /// </summary>
        /// <returns></returns>
        public override string ConnectionString()
        {
            return string.Empty;
        }

        /// <summary>
        /// 获取数据源连接字符串。
        /// </summary>
        public override string ConnectionString(ConnectionType connectionType)
        {
            switch (connectionType)
            {
                default:
                case ConnectionType.LeanSqlServer:
                    return ConfigurationManager.ConnectionStrings["SqlServerConnection"].ToString();
                case ConnectionType.LeanAccess:
                    return ConfigurationManager.ConnectionStrings["AccessConnString"].ToString();
            }
        }

        /// <summary>
        /// 获取一个新的数据库连接。
        /// </summary>
        /// <returns></returns>
        public override DbConnection CreateConnection(ConnectionType connectionType)
        {
            SqlConnection conn = new SqlConnection(ConnectionString(connectionType));
            conn.Open();
            return conn;
        }

        /// <summary>
        /// 获取一个新的数据库连接。
        /// </summary>
        /// <returns></returns>
        public override DbConnection CreateConnection()
        {
            SqlConnection conn = new SqlConnection(ConnectionString(ConnectionType.LeanSqlServer));
            conn.Open();
            return conn;
        }

        /// <summary>
        /// 获取指定的数据源字符串。
        /// </summary>
        /// <param name="connectionConfigurationName">数据源配置节名称。</param>
        /// <returns>数据源连接字符串</returns>
        public override string GetConnectionString(string connectionConfigurationName)
        {
            return ConfigurationManager.ConnectionStrings[connectionConfigurationName].ToString();
        }

        /// <summary>
        /// 创建 DbCommand 对象。
        /// </summary>
        /// <param name="cmdText">SQL 命令。</param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public override DbCommand CreateCommand(string cmdText, DbConnection conn)
        {
            return new SqlCommand(cmdText, (SqlConnection)conn);
        }

        /// <summary>
        /// 创建 DbCommand 对象。
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public override DbCommand CreateCommand(DbConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = (SqlConnection)conn;
            return cmd;
        }
    }
}
