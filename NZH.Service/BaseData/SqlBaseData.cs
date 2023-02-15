using NZH.IService;
using NZH.IService.BaseData;
using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NZH.Common.Extensions;
using System.Data.Common;

namespace NZH.Service.BaseData
{
    public class SQLBaseData : BaseDataAdapter, IBaseData
    {
        public SQLBaseData(BaseDatabaseContext context) : base(context) { }

        /// <summary>
        /// 获取某一个角色信息
        /// </summary>
        /// <param name="Roles">某一角色的集合DataTable</param>
        /// <returns>角色实体</returns>
        public RoleInfo GetOneRoleInfo(List<RoleInfo> Roles)
        {
            RoleInfo Role = new RoleInfo();
            if (Roles != null && Roles.Count > 0)
            {
                Role.RoleName = Roles[0].RoleName;
                Role.RoleNode = Roles[0].RoleNode;
                Role.AuthorityID = Roles[0].AuthorityID;
            }
            return Role;
        }

        /// <summary>
        /// 通过角色ID获取角色名
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="Roles"></param>
        /// <returns></returns>
        public List<RoleInfo> GetRoleNameByRoleId(string RoleID, List<RoleInfo> Roles)
        {
            List<RoleInfo> RoleList = new List<RoleInfo>();
            if (string.IsNullOrEmpty(RoleID))
            {
                return RoleList;
            }
            if (!string.IsNullOrEmpty(RoleID))
            {
                string[] RoleIds = RoleID.Split('|');
                for (int j = 0; j < RoleIds.Count(); j++)
                {
                    RoleInfo role = new RoleInfo();
                    for (int k = 0; k < Roles.Count; k++)
                    {
                        if (Convert.ToInt32(RoleIds[j]) == Roles[k].RoleID)
                        {
                            role.RoleName = Roles[k].RoleName;
                            RoleList.Add(role);
                        }
                    }
                }
                return RoleList;
            }
            return RoleList;
        }

        /// <summary>
        /// 通过某一角色获取权限信息
        /// </summary>
        /// <param name="AuthorityID">权限id</param>
        /// <returns>权限信息的集合</returns>
        public List<AuthorityInfo> GetAuthorityInfoByRole(string AuthorityID)
        {
            List<AuthorityInfo> AuthorityList = new List<AuthorityInfo>();
            if (string.IsNullOrEmpty(AuthorityID))
            {
                return AuthorityList;
            }
            string authorityId = AuthorityID;
            if (AuthorityID.IndexOf('|', 0) == 0)
            {
                authorityId = AuthorityID.Substring(1, AuthorityID.Length - 1).Replace('|', ',');
            }
            else
            {
                authorityId = AuthorityID.Replace('|', ',');
            }
            string sql = " Select * from T_Authority Where AuthorityID in (" + authorityId + ")  order by sortcode asc ";
            try
            {
                DataTable dt = GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                    AuthorityList = Util.DataTableConvertList<AuthorityInfo>(dt);
                return AuthorityList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region SqlHelper辅助类

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                using (DbConnection dbConnection = base.Context.CreateConnection())
                {
                    SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                    sqlCommand.CommandText = sql;
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    da.Fill(dt);
                    sqlCommand.Dispose();
                    dbConnection.Close();
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, params SqlParameter[] pms)
        {
            try
            {
                DataTable dt = new DataTable();
                using (DbConnection dbConnection = base.Context.CreateConnection())
                {
                    SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                    sqlCommand.CommandText = sql;
                    if (pms != null)
                    {
                        foreach (SqlParameter item in pms)
                        {
                            if (item != null)
                            {
                                sqlCommand.Parameters.Add(item);
                            }
                        }
                    }
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    da.Fill(dt);
                    sqlCommand.Dispose();
                    dbConnection.Close();
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            try
            {
                using (DbConnection dbConnection = base.Context.CreateConnection())
                {
                    SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                    int i = sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
                    dbConnection.Close();
                    return i;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params SqlParameter[] pms)
        {
            try
            {
                using (DbConnection dbConnection = base.Context.CreateConnection())
                {
                    SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                    if (pms != null)
                    {
                        foreach (SqlParameter item in pms)
                        {
                            if (item != null)
                            {
                                sqlCommand.Parameters.Add(item);
                            }
                        }
                    }
                    int i = sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
                    dbConnection.Close();
                    return i;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。最多1000
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>        
        public int BatchExecuteNonQuery(List<string> SqlList)
        {
            int Result = 0;
            string Sqls = "";
            foreach (var row in SqlList)
            {
                Sqls += row + ";";
            }
            try
            {
                Result = ExecuteNonQuery(Sqls);
                return Result;
            }
            catch (Exception ex)
            {
                return Result;
            }
        }

        /// <summary>
        /// 返回首行首列
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            try
            {
                using (DbConnection dbConnection = base.Context.CreateConnection())
                {
                    SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                    object i = sqlCommand.ExecuteScalar();
                    sqlCommand.Dispose();
                    dbConnection.Close();
                    return i;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 返回首行首列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params SqlParameter[] pms)
        {
            try
            {
                using (DbConnection dbConnection = base.Context.CreateConnection())
                {
                    SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                    if (pms != null)
                    {
                        foreach (SqlParameter item in pms)
                        {
                            if (item != null)
                            {
                                sqlCommand.Parameters.Add(item);
                            }
                        }
                    }
                    object i = sqlCommand.ExecuteScalar();
                    sqlCommand.Dispose();
                    dbConnection.Close();
                    return i;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 返回游标对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string sql)
        {
            try
            {
                using (DbConnection dbConnection = base.Context.CreateConnection())
                {
                    SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                    SqlDataReader sdr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    sdr.Close();
                    sqlCommand.Dispose();
                    dbConnection.Close();
                    return sdr;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 返回游标对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string sql, params SqlParameter[] pms)
        {
            try
            {
                using (DbConnection dbConnection = base.Context.CreateConnection())
                {
                    SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                    if (pms != null)
                    {
                        foreach (SqlParameter item in pms)
                        {
                            if (item != null)
                            {
                                sqlCommand.Parameters.Add(item);
                            }
                        }
                    }
                    SqlDataReader sdr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    sdr.Close();
                    sqlCommand.Dispose();
                    dbConnection.Close();
                    return sdr;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
