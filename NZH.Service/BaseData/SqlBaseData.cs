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
        /// 登录
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="UserPassword"></param>
        /// <returns></returns>
        public UserInfo Login(string UserName, string UserPassword)
        {
            UserInfo result = new UserInfo();
            if (Util.FilterSpecial(UserName))
                return result;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @"select * from T_User where UserName=@UserName and UserPassword=@UserPassword and UserUsable=1";
            SqlParameter[] parameter = {
                                new SqlParameter("@UserName",SqlDbType.VarChar),
                                new SqlParameter("@UserPassword",SqlDbType.VarChar),
                                           };

            parameter[0].Value = UserName;
            parameter[1].Value = Util.GetMd5Str(UserPassword);
            try
            {
                using (DbConnection dbConnection = base.Context.CreateConnection())
                {
                    SqlCommand sqlCommand = (SqlCommand)base.Context.CreateCommand(sql, dbConnection);
                    sqlCommand.CommandText = sql;
                    foreach (SqlParameter parm in parameter)
                        sqlCommand.Parameters.Add(parm);
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    da.Fill(ds);
                    sqlCommand.Dispose();
                    dbConnection.Close();
                }
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    result.UserID = (int)dt.Rows[0]["UserID"];
                    result.UserName = dt.Rows[0]["UserName"] + "";
                    result.TrueName = dt.Rows[0]["TrueName"] + "";
                    result.UserUsable = (int)dt.Rows[0]["UserUsable"];
                    result.CreateDate = string.IsNullOrEmpty(dt.Rows[0]["CreateDate"].ToString()) ? DateTime.Now : (DateTime)dt.Rows[0]["CreateDate"];
                    result.RoleID = dt.Rows[0]["RoleID"].ToString();
                    result.UserNote = dt.Rows[0]["UserNote"].ToString();

                    List<RoleInfo> roleinfo = new List<RoleInfo>();
                    List<AuthorityInfo> authorityinfo = new List<AuthorityInfo>();
                    string authorityid = "";
                    //角色信息
                    if (!string.IsNullOrWhiteSpace(result.RoleID))
                    {
                        string[] roles = result.RoleID.Split('|');
                        bool flag = true;
                        for (int j = 0; j < roles.Length; j++)
                        {
                            RoleInfo ri = new RoleInfo();
                            ri.RoleID = Convert.ToInt32(roles[j]);
                            List<RoleInfo> role = GetRoleInfo(ri);
                            ri = GetOneRoleInfo(role);
                            authorityid += "|" + ri.AuthorityID;
                            //权限信息
                            if (roles[j].ToString() == "1")
                            {
                                flag = false;
                            }
                            roleinfo.Add(ri);
                        }
                        //判断是否有管理员权限
                        if (flag)
                        {
                            authorityinfo = GetAuthorityInfoByRole(authorityid);
                        }
                        else
                        {
                            authorityinfo = GetAuthorityInfo(new AuthorityInfo());
                        }

                        result.role = roleinfo;
                        result.authority = authorityinfo;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return result;
        }

        /// <summary>
        /// 获取某一个角色信息
        /// </summary>
        /// <param name="dt">某一角色的集合DataTable</param>
        /// <returns>角色实体</returns>
        public RoleInfo GetOneRoleInfo(List<RoleInfo> role)
        {
            RoleInfo ri = new RoleInfo();
            if (role != null && role.Count > 0)
            {
                ri.RoleName = role[0].RoleName;
                ri.RoleNode = role[0].RoleNode;
                ri.AuthorityID = role[0].AuthorityID;
            }
            return ri;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="userpassword">用户密码</param>
        /// <returns>执行数</returns>
        public int UpdatePassWord(string UserName, string userpassword)
        {
            int result = 0;
            string sql = " Update T_User Set UserPassword=@UserPassword where UserName=@UserName ";
            SqlParameter[] parameter = {
                                new SqlParameter("@UserName",SqlDbType.VarChar),
                                new SqlParameter("@UserPassword",SqlDbType.VarChar)
                                       };
            parameter[0].Value = UserName;
            parameter[1].Value = Util.GetMd5Str(userpassword);
            try
            {
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
                return result;
            }

        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns>返回添加返回数</returns>
        public int AddUser(UserInfo user)
        {
            int result = 0;
            if (user == null)
            {
                return result;
            }
            #region sql
            string sql = @"INSERT INTO T_User
           (UserName,TrueName, UserPassword, UserUsable, CreateDate, UserNote,RoleID)
     VALUES(@UserName,@TrueName, @UserPassword, @UserUsable, @CreateDate, @UserNote,@RoleID)";
            SqlParameter[] parameter = {
                                new SqlParameter("@UserName",SqlDbType.VarChar),
                                new SqlParameter("@TrueName",SqlDbType.VarChar),
                                new SqlParameter("@UserPassword",SqlDbType.VarChar),
                                new SqlParameter("@UserUsable",SqlDbType.Int),
                                new SqlParameter("@CreateDate",SqlDbType.DateTime),
                                new SqlParameter("@UserNote",SqlDbType.VarChar),
                                new SqlParameter("@RoleID",SqlDbType.VarChar)
                                           };
            parameter[0].Value = user.UserName;
            parameter[1].Value = user.TrueName;
            parameter[2].Value = Util.GetMd5Str(user.UserPassword);
            parameter[3].Value = user.UserUsable;
            parameter[4].Value = user.CreateDate + "" == "" ? DateTime.Now : user.CreateDate;
            parameter[5].Value = user.UserNote + "";
            parameter[6].Value = user.RoleID;
            #endregion
            try
            {
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
                return result;
            }
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns>返回添加返回数</returns>
        public int UpdateUser(UserInfo user)
        {
            int result = 0;
            #region sql
            string sql = " UPDATE T_User SET ";
            StringBuilder strWhere = new StringBuilder("");
            //判断用户真实姓名是否为空
            if (!string.IsNullOrEmpty(user.TrueName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" TrueName='" + user.TrueName + "' ");
            }
            //判断用户标志位是否为空
            if (!string.IsNullOrEmpty(user.UserUsable.ToString()))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" UserUsable='" + user.UserUsable + "' ");
            }
            //判断用户密码是否为空
            if (!string.IsNullOrEmpty(user.UserPassword))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" UserPassword='" + Util.GetMd5Str(user.UserPassword) + "' ");
            }
            //判断用户角色是否为空
            if (user.RoleID != null)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" RoleID='" + user.RoleID + "' ");
            }
            //判断用户名是否为空
            if (string.IsNullOrEmpty(strWhere.ToString()))
            {
                return 0;
            }
            else
            {
                strWhere.Append(" Where UserName='" + user.UserName + "' ");
            }
            sql += strWhere;

            #endregion
            try
            {
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
                return result;
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ID_UserID">用户实体</param>
        /// <returns>返回添加返回数</returns>
        public int DeleteUser(int UserID)
        {
            int result = 0;
            #region sql
            string sql = " Delete from T_User Where UserID=" + UserID + " ";

            #endregion
            try
            {
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
                return result;
            }
        }

        /// <summary>
        /// 查询用户信息(包含角色名)
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns>用户信息集合</returns>
        public List<UserInfo> GetUserInfo(UserInfo user)
        {
            List<UserInfo> list = new List<UserInfo>();
            #region sql
            string sql = @" select * from T_User ";
            #region  查询条件
            StringBuilder strWhere = new StringBuilder("");
            //判断用户真实姓名是否为空
            if (!string.IsNullOrEmpty(user.TrueName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" TrueName like '%" + user.TrueName + "%' ");
            }
            //判断用户名是否为空
            if (!string.IsNullOrEmpty(user.UserName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" UserName like '%" + user.UserName + "%'  ");
            }
            //判断用户标志位是否为空
            if (user.UserUsable > 1)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" UserUsable='" + user.UserUsable + "' ");
            }

            #endregion
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
                    list = Util.DataTableConvertList<UserInfo>(ds.Tables[0]);
                if (list.Count > 0)
                {
                    RoleInfo role = new RoleInfo();
                    List<RoleInfo> roleinfo = GetRoleInfo(role);
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].role = GetRoleNameByRoleId(list[i].RoleID, roleinfo);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

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

        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <param name="authority">权限实体</param>
        /// <returns>权限信息的集合</returns>
        public List<AuthorityInfo> GetAuthorityInfo(AuthorityInfo authority)
        {
            List<AuthorityInfo> list = new List<AuthorityInfo>();
            DataSet ds = new DataSet();

            #region
            string sql = " Select * from T_Authority ";
            StringBuilder strWhere = new StringBuilder("");
            //判断权限ID是否为空
            if (authority.AuthorityID != 0)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" AuthorityID='" + authority.AuthorityID + "'  ");
            }
            //判断功能码是否为空
            if (!string.IsNullOrEmpty(authority.FunCode))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" FunCode='" + authority.FunCode + "' ");
            }
            //判断功能名称是否为空
            if (!string.IsNullOrEmpty(authority.FunName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" FunName='" + authority.FunName + "' ");
            }
            //判断父节点是否为空
            if (!string.IsNullOrEmpty(authority.ParentID))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" ParentID='" + authority.ParentID + "' ");
            }
            //判断权限路径是否为空
            if (!string.IsNullOrEmpty(authority.Menu))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" Menu='" + authority.Menu + "' ");
            }
            //判断权限备注是否为空
            if (authority.SortCode > 0)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" SortCode='" + authority.SortCode + "' ");
            }
            strWhere.Append(" order by SortCode asc ");
            sql += strWhere;

            #endregion
            try
            {
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
                    list = Util.DataTableConvertList<AuthorityInfo>(ds.Tables[0]);
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
        public List<AuthorityInfo> GetAuthorityInfoByRole(string authorityid)
        {
            List<AuthorityInfo> list = new List<AuthorityInfo>();
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(authorityid))
            {
                return list;
            }
            #region
            string authority = authorityid;
            if (authorityid.IndexOf('|', 0) == 0)
            {
                authority = authorityid.Substring(1, authorityid.Length - 1).Replace('|', ',');
            }
            else
            {
                authority = authorityid.Replace('|', ',');
            }
            string sql = " Select * from T_Authority Where AuthorityID in (" + authority + ")  order by sortcode asc ";

            #endregion
            try
            {
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
                    list = Util.DataTableConvertList<AuthorityInfo>(ds.Tables[0]);
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="role">权限实体</param>
        /// <returns>返回添加返回数</returns>
        public int AddAuthority(AuthorityInfo authority)
        {
            if (authority == null)
            {
                return 0;
            }
            #region sql
            string sql = @"INSERT INTO T_Authority
           (ParentID, FunName, AuNode, Menu, FunCode,SortCode)
     VALUES(@ParentID, @FunName, @AuNode, @Menu, @FunCode,@SortCode)";
            SqlParameter[] parameter = {
                                new SqlParameter("@ParentID",SqlDbType.VarChar),
                                new SqlParameter("@FunName",SqlDbType.VarChar),
                                new SqlParameter("@AuNode",SqlDbType.VarChar),
                                new SqlParameter("@Menu",SqlDbType.VarChar),
                                new SqlParameter("@FunCode",SqlDbType.VarChar),
                                new SqlParameter("@SortCode",SqlDbType.Int)

                                           };
            parameter[0].Value = authority.ParentID;
            parameter[1].Value = authority.FunName;
            parameter[2].Value = authority.AuNode;
            parameter[3].Value = authority.Menu;
            parameter[4].Value = authority.FunCode;
            parameter[5].Value = authority.SortCode;

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
        /// 修改权限
        /// </summary>
        /// <param name="role">权限实体</param>
        /// <returns>返回添加返回数</returns>
        public int UpdateAuthority(AuthorityInfo authority)
        {
            #region sql
            string sql = " UPDATE T_Authority SET ";
            StringBuilder strWhere = new StringBuilder("");
            //判断权限功能名是否为空
            if (!string.IsNullOrEmpty(authority.FunName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" FunName='" + authority.FunName + "' ");
            }
            //判断权限父节点是否为空
            if (!string.IsNullOrEmpty(authority.ParentID))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" ParentID='" + authority.ParentID + "' ");
            }
            //判断权限目录是否为空
            if (!string.IsNullOrEmpty(authority.Menu))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" Menu='" + authority.Menu + "' ");
            }
            //判断标识码是否为空
            if (!string.IsNullOrEmpty(authority.AuNode))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" AuNode='" + authority.AuNode + "' ");
            }
            //判断权限备注是否为空
            if (authority.SortCode > 0)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" SortCode='" + authority.SortCode + "' ");
            }
            //判断修改条件是否为空
            if (string.IsNullOrEmpty(strWhere.ToString()))
            {
                return 0;
            }
            else
            {
                strWhere.Append(" Where FunCode='" + authority.FunCode + "' ");
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
        /// 删除权限
        /// </summary>
        /// <param name="ID_RoleID">权限实体</param>
        /// <returns>返回添加返回数</returns>
        public int DeleteAuthority(string FunCode)
        {
            #region sql
            string sql = @" Delete from T_Authority Where FunCode='" + FunCode + "' or ParentID='" + FunCode + "'";
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
        /// 获取mes用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<MESUser> GetMESUser(MESUser user)
        {
            List<MESUser> list = new List<MESUser>();
            #region sql
            string sql = @" select t1.USER_NAME,t2.PERSON_NAME,t1.ID as USER_ID,t1.PERSON_ID,t1.PASSWORD as UserPassword from SYS_USER_PROFILE t1 left join SYS_PERSON t2 on t1.PERSON_ID=t2.ID ";
            #region  查询条件
            StringBuilder strWhere = new StringBuilder("");
            //判断用户真实姓名是否为空
            if (!string.IsNullOrEmpty(user.PERSON_NAME))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" t2.PERSON_NAME like '%" + user.PERSON_NAME + "%' ");
            }
            //判断用户名是否为空
            if (!string.IsNullOrEmpty(user.USER_NAME))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" t1.USER_NAME like '%" + user.USER_NAME + "%'  ");
            }

            #endregion
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
                    list = Util.DataTableConvertList<MESUser>(ds.Tables[0]);
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取mes角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<MESRole> GetMESRole(MESUser user)
        {
            List<MESRole> list = new List<MESRole>();
            #region sql
            string sql = "";
            if (user.USER_ID != null)
            {
                sql = string.Format(@" select t1.ID as ROLE_ID,t1.ROLE_NAME,(case when t2.ROLE_ID is null then '0' else '1' end)  ROLE_ID_Check from SYS_ROLE t1 left join (select * from SYS_USER_IN_ROLES 
where USER_ID='{0}') t2 on t1.ID=t2.ROLE_ID ", user.USER_ID);
            }
            else
            {
                sql = string.Format(@" select t1.ID as ROLE_ID,t1.ROLE_NAME,0 as ROLE_ID_Check from SYS_ROLE t1 ");
            }
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
                    list = Util.DataTableConvertList<MESRole>(ds.Tables[0]);
                foreach (var row in list)
                {
                    if (row.ROLE_ID_Check + "" == "0")
                        row.isCheck = false;
                    else
                        row.isCheck = true;

                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 添加mes用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int MESAddUser(MESUser user)
        {
            int result = 0;
            List<string> sqllist = new List<string>();
            sqllist.Add(string.Format(@" INSERT INTO SYS_USER_PROFILE 
(ID, USER_NAME,PASSWORD,PASSWORD_SALT,PERSON_ID,MOBILE_PIN,PASSWORD_QUESTION,PASSWORD_ANSWER,IS_APPROVED,
DATETIME_LAST_ACTIVITY,DATETIME_LAST_LOGIN,DATETIME_LAST_PASSWORD_CHANGED,DATETIME_CREATION,DATETIME_LAST_LOCKED_OUT,
FAILED_PASSWORD_ATTEMPT_COUNT,FAILED_PASSWORD_ATTEMPT_WINDOW_START,FAILED_PASSWORD_ANSWER_ATTEMPT_COUNT,
FAILED_PASSWORD_ANSWER_ATTEMPT_WINDOW_START) 
VALUES('{0}','{1}','{2}','51Tg5W5K9IIrKmfISaGB4Q==','{3}','','my office name','D6YL8ChxSPHtEF0hsZrvAQ==',1,
GETDATE(),GETDATE(),GETDATE(),GETDATE(),GETDATE(),
0,GETDATE(),0,GETDATE())", user.USER_ID, user.USER_NAME, user.PASSWORD, user.PERSON_ID));
            sqllist.Add(string.Format(@" INSERT INTO SYS_PERSON (ID, PERSON_NAME) VALUES ('{0}', '{1}')", user.PERSON_ID, user.PERSON_NAME));
            sqllist.Add(" Delete from SYS_USER_IN_ROLES Where USER_ID='" + user.USER_ID + "' ");
            foreach (var row in user.MESRole)
            {
                if (row.isCheck)
                    sqllist.Add(string.Format(@" INSERT INTO SYS_USER_IN_ROLES (USER_ID, ROLE_ID) VALUES ('{0}', '{1}')", user.USER_ID, row.ROLE_ID));
            }

            result = ExecuteNonQuery(sqllist);
            return result;
        }

        /// <summary>
        /// 判断是否添加mes用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int CheckMESAddUser(MESUser user)
        {
            int result = 0;
            string sql = string.Format(@"select * from SYS_USER_PROFILE where user_name='{0}' ", user.USER_NAME);

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
                    result = 1;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新mes用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int MESUpdateUser(MESUser user)
        {
            int result = 0;
            List<string> sqllist = new List<string>();
            if (user.CHANGE_PASSWORD)
            {
                sqllist.Add(string.Format(@" UPDATE SYS_USER_PROFILE SET PASSWORD='{0}' WHERE ID='{1}'", user.PASSWORD, user.USER_ID));
            }
            sqllist.Add(string.Format(@" UPDATE SYS_PERSON SET PERSON_NAME='{0}' WHERE ID='{1}'", user.PERSON_NAME, user.PERSON_ID));
            sqllist.Add(" Delete from SYS_USER_IN_ROLES Where USER_ID='" + user.USER_ID + "' ");
            foreach (var row in user.MESRole)
            {
                if (row.isCheck)
                    sqllist.Add(string.Format(@" INSERT INTO SYS_USER_IN_ROLES (USER_ID, ROLE_ID) VALUES ('{0}', '{1}')", user.USER_ID, row.ROLE_ID));
            }

            result = ExecuteNonQuery(sqllist);
            return result;
        }

        /// <summary>
        /// 删除mes用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int MESDeleteUser(MESUser user)
        {
            int result = 0;
            List<string> sqllist = new List<string>();
            sqllist.Add(" Delete from SYS_USER_PROFILE Where ID='" + user.USER_ID + "' ");
            sqllist.Add(" Delete from SYS_PERSON Where ID='" + user.PERSON_ID + "' ");
            sqllist.Add(" Delete from SYS_USER_IN_ROLES Where USER_ID='" + user.USER_ID + "' ");
            result = ExecuteNonQuery(sqllist);
            return result;
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。最多1000
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>        
        public int ExecuteNonQuery(List<string> SQLStringList)
        {
            int result = 0;
            string sql = "";
            foreach (var row in SQLStringList)
            {
                sql += row + ";";
            }
            try
            {
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
                return result;
            }

        }
    }
}
