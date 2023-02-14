using NZH.Common.Extensions;
using NZH.IService;
using NZH.IService.BaseData;
using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Service.BaseData
{
    public class RoleService : BaseDataAdapter, IRoleService
    {
        public SQLBaseData(BaseDatabaseContext context) : base(context) { }

        /// <summary>
        /// 通过角色ID获取角色名
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="roleinfo"></param>
        /// <returns></returns>
        public List<RoleInfo> GetRoleNameByRoleId(string roleid, List<RoleInfo> roleinfo)
        {
            List<RoleInfo> rolenamelist = new List<RoleInfo>();
            if (string.IsNullOrEmpty(roleid))
            {
                return rolenamelist;
            }
            if (!string.IsNullOrEmpty(roleid))
            {
                string[] rolesid = roleid.Split('|');
                for (int j = 0; j < rolesid.Count(); j++)
                {
                    RoleInfo role = new RoleInfo();
                    for (int k = 0; k < roleinfo.Count; k++)
                    {
                        if (Convert.ToInt32(rolesid[j]) == roleinfo[k].RoleID)
                        {
                            role.RoleName = roleinfo[k].RoleName;
                            rolenamelist.Add(role);
                        }
                    }
                }
                return rolenamelist;
            }
            return rolenamelist;
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="role">角色实体</param>
        /// <returns>角色信息的集合</returns>
        public List<RoleInfo> GetRoleInfo(RoleInfo role)
        {
            List<RoleInfo> list = new List<RoleInfo>();
            #region
            string sql = " Select * from T_Role ";
            StringBuilder strWhere = new StringBuilder("");
            //判断角色ID是否为空
            if (role.RoleID != 0)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" RoleID='" + role.RoleID + "'  ");
            }
            //判断角色名是否为空
            if (!string.IsNullOrEmpty(role.RoleName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" RoleName='" + role.RoleName + "'  ");
            }
            //判断角色备注是否为空
            if (!string.IsNullOrEmpty(role.RoleNode))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" RoleNode like '%" + role.RoleNode + "%'  ");
            }
            //判断角色权限是否为空
            if (!string.IsNullOrEmpty(role.AuthorityID))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" AuthorityID like '%" + role.AuthorityID + "%'  ");
            }
            //判断角色是否有效
            if (role.RoleUsable > 1)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" RoleUsable='" + role.RoleUsable + "'  ");
            }
            sql += strWhere;

            #endregion
            try
            {
                DataSet ds = new DataSet();
                using (DbConnection dbConnection = base.Context.CreateConnection())
                {
                    SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                    sqlCommand.CommandText = sql;
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    da.Fill(ds);
                    sqlCommand.Dispose();
                    dbConnection.Close();
                }
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                    list = Util.DataTableConvertList<RoleInfo>(ds.Tables[0]);
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 通过某一角色获取权限信息
        /// </summary>
        /// <param name="authorityid">权限id</param>
        /// <returns>权限信息的集合</returns>
        public List<RoleInfo> GetRoleInfoByUser(string roleid)
        {
            List<RoleInfo> list = new List<RoleInfo>();
            #region
            if (string.IsNullOrEmpty(roleid))
            {
                return list;
            }
            else
            {
                string role = roleid;
                if (roleid.IndexOf('|', 0) == 0)
                {
                    role = roleid.Substring(1, roleid.Length - 1).Replace('|', ',');
                }
                else
                {
                    role = roleid.Replace('|', ',');
                }
                string sql = " Select * from T_Role Where RoleID in (" + role + ")";

                #endregion
                try
                {
                    DataSet ds = new DataSet();
                    using (DbConnection dbConnection = base.Context.CreateConnection())
                    {
                        SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                        sqlCommand.CommandText = sql;
                        SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                        da.Fill(ds);
                        sqlCommand.Dispose();
                        dbConnection.Close();
                    }
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                        list = Util.DataTableConvertList<RoleInfo>(ds.Tables[0]);
                    return list;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role">角色实体</param>
        /// <returns>返回添加返回数</returns>
        public int AddRole(RoleInfo role)
        {
            if (role == null)
            {
                return 0;
            }
            #region sql
            string sql = @"INSERT INTO T_Role
           (RoleName, RoleNode, AuthorityID,RoleUsable)
     VALUES(@RoleName, @RoleNode, @AuthorityID,@RoleUsable)";
            SqlParameter[] parameter = {
                                new SqlParameter("@RoleName",SqlDbType.VarChar),
                                new SqlParameter("@RoleNode",SqlDbType.VarChar),
                                new SqlParameter("@AuthorityID",SqlDbType.VarChar),
                                new SqlParameter("@RoleUsable",SqlDbType.Int)
                                           };
            parameter[0].Value = role.RoleName;
            parameter[1].Value = role.RoleNode + "";
            parameter[2].Value = role.AuthorityID;
            parameter[3].Value = role.RoleUsable;
            #endregion

            try
            {
                int result = 0;
                using (DbConnection dbConnection = base.Context.CreateConnection())
                {
                    if (dbConnection == null) return result;
                    SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                    sqlCommand.CommandText = sql;
                    foreach (SqlParameter parm in parameter)
                        sqlCommand.Parameters.Add(parm);
                    sqlCommand.CommandTimeout = 5;
                    result = sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
                    dbConnection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role">角色实体</param>
        /// <returns>返回添加返回数</returns>
        public int UpdateRole(RoleInfo role)
        {
            #region sql
            string sql = " UPDATE T_Role SET ";
            StringBuilder strWhere = new StringBuilder("");
            //判断角色名是否为空
            if (!string.IsNullOrEmpty(role.RoleName))
            {
                strWhere.Append(" RoleName='" + role.RoleName + "'  ");
            }
            //判断角色备注是否为空
            if (!string.IsNullOrEmpty(role.RoleNode))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" RoleNode='" + role.RoleNode + "' ");
            }
            //判断角色权限是否为空
            if (!string.IsNullOrEmpty(role.AuthorityID))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" AuthorityID='" + role.AuthorityID + "' ");
            }
            //判断角色是否有效是否为空
            if (role.RoleUsable > 1)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" RoleUsable='" + role.RoleUsable + "' ");
            }
            //判断修改条件是否为空
            if (string.IsNullOrEmpty(strWhere.ToString()))
            {
                return 0;
            }
            else
            {
                strWhere.Append(" Where RoleID=" + role.RoleID + " ");
            }
            sql += strWhere;

            #endregion
            try
            {
                int result = 0;
                using (DbConnection dbConnection = base.Context.CreateConnection())
                {
                    if (dbConnection == null) return result;
                    SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                    sqlCommand.CommandText = sql;
                    sqlCommand.CommandTimeout = 5;
                    result = sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
                    dbConnection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="RoleID">角色实体</param>
        /// <returns>返回添加返回数</returns>
        public int DeleteRole(int RoleID)
        {
            #region sql
            string sql = " Delete from T_Role Where RoleID=" + RoleID + " ";
            #endregion
            try
            {
                int result = 0;
                using (DbConnection dbConnection = base.Context.CreateConnection())
                {
                    if (dbConnection == null) return result;
                    SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                    sqlCommand.CommandText = sql;
                    sqlCommand.CommandTimeout = 5;
                    result = sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
                    dbConnection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
