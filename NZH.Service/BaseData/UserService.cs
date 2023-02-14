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
    public class UserService: BaseDataAdapter, IUserService
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

    }
}
