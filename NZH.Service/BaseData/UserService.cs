using NZH.Common.Extensions;
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

namespace NZH.Service.BaseData
{
    public class UserService : SQLBaseData, IUserService
    {
        public UserService(BaseDatabaseContext context) : base(context) { }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="UserPassword"></param>
        /// <returns></returns>
        public UserInfo Login(string UserName, string UserPassword)
        {
            UserInfo User = new UserInfo();
            if (Util.FilterSpecial(UserName))
                return User;
            string sql = @"select * from T_User where UserName=@UserName and UserPassword=@UserPassword and UserUsable=1";
            SqlParameter[] parameter = {
                                new SqlParameter("@UserName",SqlDbType.VarChar),
                                new SqlParameter("@UserPassword",SqlDbType.VarChar),
                                           };
            parameter[0].Value = UserName;
            parameter[1].Value = Util.GetMd5Str(UserPassword);
            try
            {
                DataTable dt = GetDataTable(sql, parameter);
                if (dt != null && dt.Rows.Count > 0)
                {
                    User.UserID = (int)dt.Rows[0]["UserID"];
                    User.UserName = dt.Rows[0]["UserName"] + "";
                    User.TrueName = dt.Rows[0]["TrueName"] + "";
                    User.UserUsable = (int)dt.Rows[0]["UserUsable"];
                    User.CreateDate = string.IsNullOrEmpty(dt.Rows[0]["CreateDate"].ToString()) ? DateTime.Now : (DateTime)dt.Rows[0]["CreateDate"];
                    User.RoleID = dt.Rows[0]["RoleID"].ToString();
                    User.UserNote = dt.Rows[0]["UserNote"].ToString();
                    List<RoleInfo> Roles = new List<RoleInfo>();
                    List<AuthorityInfo> Authoritys = new List<AuthorityInfo>();
                    string AuthorityId = "";
                    //角色信息
                    if (!string.IsNullOrWhiteSpace(User.RoleID))
                    {
                        string[] roles = User.RoleID.Split('|');
                        bool flag = true;
                        for (int j = 0; j < roles.Length; j++)
                        {
                            RoleInfo role = new RoleInfo();
                            role.RoleID = Convert.ToInt32(roles[j]);
                            List<RoleInfo> roleList = this.Context.RoleService.GetRoleInfo(role);
                            role = this.Context.RoleService.GetOneRoleInfo(roleList);
                            AuthorityId += "|" + role.AuthorityID;
                            //权限信息
                            if (roles[j].ToString() == "1")
                            {
                                flag = false;
                            }
                            Roles.Add(role);
                        }
                        //判断是否有管理员权限
                        if (flag)
                        {
                            Authoritys = this.Context.AuthorityService.GetAuthorityInfoByRole(AuthorityId);
                        }
                        else
                        {
                            Authoritys = this.Context.AuthorityService.GetAuthorityInfo(new AuthorityInfo(), false);
                        }
                        User.Roles = Roles;
                        User.Authoritys = Authoritys;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return User;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="UserPassword">用户密码</param>
        /// <returns>执行数</returns>
        public int UpdatePassWord(string UserName, string UserPassword)
        {
            int Result = 0;
            if (string.IsNullOrWhiteSpace(UserName))
            {
                return Result;
            }
            string sql = " Update T_User Set UserPassword=@UserPassword where UserName=@UserName ";
            SqlParameter[] parameter = {
                                new SqlParameter("@UserName",SqlDbType.VarChar),
                                new SqlParameter("@UserPassword",SqlDbType.VarChar)
                                       };
            parameter[0].Value = UserName;
            parameter[1].Value = Util.GetMd5Str(UserPassword);
            try
            {
                Result = ExecuteNonQuery(sql, parameter);
                return Result;
            }
            catch (Exception ex)
            {
                return Result;
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="User">用户实体</param>
        /// <returns>返回添加返回数</returns>
        public int AddUser(UserInfo User)
        {
            int Result = 0;
            if (User == null)
            {
                return Result;
            }
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
            parameter[0].Value = User.UserName;
            parameter[1].Value = User.TrueName;
            parameter[2].Value = Util.GetMd5Str(User.UserPassword);
            parameter[3].Value = User.UserUsable;
            parameter[4].Value = User.CreateDate + "" == "" ? DateTime.Now : User.CreateDate;
            parameter[5].Value = User.UserNote + "";
            parameter[6].Value = User.RoleID;
            try
            {
                Result = ExecuteNonQuery(sql, parameter);
                return Result;
            }
            catch (Exception ex)
            {
                return Result;
            }
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="User">用户实体</param>
        /// <returns>返回添加返回数</returns>
        public int UpdateUser(UserInfo User)
        {
            int Result = 0;
            if (User == null)
            {
                return Result;
            }
            string sql = " UPDATE T_User SET ";
            StringBuilder strWhere = new StringBuilder("");
            //判断用户真实姓名是否为空
            if (!string.IsNullOrEmpty(User.TrueName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" TrueName='" + User.TrueName + "' ");
            }
            //判断用户标志位是否为空
            if (!string.IsNullOrEmpty(User.UserUsable.ToString()))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" UserUsable='" + User.UserUsable + "' ");
            }
            //判断用户密码是否为空
            if (!string.IsNullOrEmpty(User.UserPassword))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" UserPassword='" + Util.GetMd5Str(User.UserPassword) + "' ");
            }
            //判断用户角色是否为空
            if (User.RoleID != null)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" RoleID='" + User.RoleID + "' ");
            }
            //判断用户名是否为空
            if (string.IsNullOrEmpty(strWhere.ToString()))
            {
                return Result;
            }
            else
            {
                strWhere.Append(" Where UserName='" + User.UserName + "' ");
            }
            sql += strWhere;
            try
            {
                Result = ExecuteNonQuery(sql);
                return Result;
            }
            catch (Exception ex)
            {
                return Result;
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="UserID">用户实体</param>
        /// <returns>返回添加返回数</returns>
        public int DeleteUser(int UserID)
        {
            int Result = 0;
            string sql = " Delete from T_User Where UserID=" + UserID + " ";
            try
            {
                Result = ExecuteNonQuery(sql);
                return Result;
            }
            catch (Exception ex)
            {
                return Result;
            }
        }

        /// <summary>
        /// 查询用户信息(包含角色名)
        /// </summary>
        /// <param name="User">用户实体</param>
        /// <returns>用户信息集合</returns>
        public List<UserInfo> GetUserInfo(UserInfo User)
        {
            List<UserInfo> UserList = new List<UserInfo>();
            if (User == null)
            {
                return UserList;
            }
            string sql = @" select * from T_User ";
            StringBuilder strWhere = new StringBuilder("");
            //判断用户真实姓名是否为空
            if (!string.IsNullOrEmpty(User.TrueName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" TrueName like '%" + User.TrueName + "%' ");
            }
            //判断用户名是否为空
            if (!string.IsNullOrEmpty(User.UserName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" UserName like '%" + User.UserName + "%'  ");
            }
            //判断用户标志位是否为空
            if (User.UserUsable > 1)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" UserUsable='" + User.UserUsable + "' ");
            }
            sql += strWhere;
            try
            {
                DataTable dt = GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                    UserList = Util.DataTableConvertList<UserInfo>(dt);
                if (UserList.Count > 0)
                {
                    RoleInfo role = new RoleInfo();
                    List<RoleInfo> Roles = this.Context.RoleService.GetRoleInfo(role);
                    for (int i = 0; i < UserList.Count; i++)
                    {
                        UserList[i].Roles = this.Context.RoleService.GetRoleNameByRoleId(UserList[i].RoleID, Roles);
                    }
                }
                return UserList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
