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
                            List<RoleInfo> roleList = GetRoleInfo(role);
                            role = GetOneRoleInfo(roleList);
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
                            Authoritys = GetAuthorityInfoByRole(AuthorityId);
                        }
                        else
                        {
                            Authoritys = GetAuthorityInfo(new AuthorityInfo());
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
                    List<RoleInfo> Roles = GetRoleInfo(role);
                    for (int i = 0; i < UserList.Count; i++)
                    {
                        UserList[i].Roles = GetRoleNameByRoleId(UserList[i].RoleID, Roles);
                    }
                }
                return UserList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
        /// 获取角色信息
        /// </summary>
        /// <param name="Role">角色实体</param>
        /// <returns>角色信息的集合</returns>
        public List<RoleInfo> GetRoleInfo(RoleInfo Role)
        {
            List<RoleInfo> RoleList = new List<RoleInfo>();
            if (Role == null)
            {
                return RoleList;
            }
            string sql = " Select * from T_Role ";
            StringBuilder strWhere = new StringBuilder("");
            //判断角色ID是否为空
            if (Role.RoleID != 0)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" RoleID='" + Role.RoleID + "'  ");
            }
            //判断角色名是否为空
            if (!string.IsNullOrEmpty(Role.RoleName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" RoleName='" + Role.RoleName + "'  ");
            }
            //判断角色备注是否为空
            if (!string.IsNullOrEmpty(Role.RoleNode))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" RoleNode like '%" + Role.RoleNode + "%'  ");
            }
            //判断角色权限是否为空
            if (!string.IsNullOrEmpty(Role.AuthorityID))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" AuthorityID like '%" + Role.AuthorityID + "%'  ");
            }
            //判断角色是否有效
            if (Role.RoleUsable > 1)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" RoleUsable='" + Role.RoleUsable + "'  ");
            }
            sql += strWhere;
            try
            {
                DataTable dt = GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                    RoleList = Util.DataTableConvertList<RoleInfo>(dt);
                return RoleList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 通过某一角色获取权限信息
        /// </summary>
        /// <param name="RoleID">权限id</param>
        /// <returns>权限信息的集合</returns>
        public List<RoleInfo> GetRoleInfoByUser(string RoleID)
        {
            List<RoleInfo> RoleList = new List<RoleInfo>();
            if (string.IsNullOrWhiteSpace(RoleID))
            {
                return RoleList;
            }
            else
            {
                string roleId = RoleID;
                if (RoleID.IndexOf('|', 0) == 0)
                {
                    roleId = RoleID.Substring(1, RoleID.Length - 1).Replace('|', ',');
                }
                else
                {
                    roleId = RoleID.Replace('|', ',');
                }
                string sql = " Select * from T_Role Where RoleID in (" + roleId + ")";
                try
                {
                    DataTable dt = GetDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                        RoleList = Util.DataTableConvertList<RoleInfo>(dt);
                    return RoleList;
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
        /// <param name="Role">角色实体</param>
        /// <returns>返回添加返回数</returns>
        public int AddRole(RoleInfo Role)
        {
            int Result = 0;
            if (Role == null)
            {
                return Result;
            }
            string sql = @"INSERT INTO T_Role
           (RoleName, RoleNode, AuthorityID,RoleUsable)
     VALUES(@RoleName, @RoleNode, @AuthorityID,@RoleUsable)";
            SqlParameter[] parameter = {
                                new SqlParameter("@RoleName",SqlDbType.VarChar),
                                new SqlParameter("@RoleNode",SqlDbType.VarChar),
                                new SqlParameter("@AuthorityID",SqlDbType.VarChar),
                                new SqlParameter("@RoleUsable",SqlDbType.Int)
                                           };
            parameter[0].Value = Role.RoleName;
            parameter[1].Value = Role.RoleNode + "";
            parameter[2].Value = Role.AuthorityID;
            parameter[3].Value = Role.RoleUsable;
            try
            {
                Result = ExecuteNonQuery(sql, parameter);
                return Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="Role">角色实体</param>
        /// <returns>返回添加返回数</returns>
        public int UpdateRole(RoleInfo Role)
        {
            int Result = 0;
            if (Role == null)
            {
                return Result;
            }
            string sql = " UPDATE T_Role SET ";
            StringBuilder strWhere = new StringBuilder("");
            //判断角色名是否为空
            if (!string.IsNullOrEmpty(Role.RoleName))
            {
                strWhere.Append(" RoleName='" + Role.RoleName + "'  ");
            }
            //判断角色备注是否为空
            if (!string.IsNullOrEmpty(Role.RoleNode))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" RoleNode='" + Role.RoleNode + "' ");
            }
            //判断角色权限是否为空
            if (!string.IsNullOrEmpty(Role.AuthorityID))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" AuthorityID='" + Role.AuthorityID + "' ");
            }
            //判断角色是否有效是否为空
            if (Role.RoleUsable > 1)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" RoleUsable='" + Role.RoleUsable + "' ");
            }
            //判断修改条件是否为空
            if (string.IsNullOrEmpty(strWhere.ToString()))
            {
                return 0;
            }
            else
            {
                strWhere.Append(" Where RoleID=" + Role.RoleID + " ");
            }
            sql += strWhere;
            try
            {
                Result = ExecuteNonQuery(sql);
                return Result;
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
            int Result = 0;
            string sql = " Delete from T_Role Where RoleID=" + RoleID + " ";
            try
            {
                Result = ExecuteNonQuery(sql);
                return Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <param name="Authority">权限实体</param>
        /// <returns>权限信息的集合</returns>
        public List<AuthorityInfo> GetAuthorityInfo(AuthorityInfo Authority)
        {
            List<AuthorityInfo> AuthorityList = new List<AuthorityInfo>();
            if (Authority == null)
            {
                return AuthorityList;
            }
            string sql = " Select * from T_Authority ";
            StringBuilder strWhere = new StringBuilder("");
            //判断权限ID是否为空
            if (Authority.AuthorityID != 0)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" AuthorityID='" + Authority.AuthorityID + "'  ");
            }
            //判断功能码是否为空
            if (!string.IsNullOrEmpty(Authority.FunCode))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" FunCode='" + Authority.FunCode + "' ");
            }
            //判断功能名称是否为空
            if (!string.IsNullOrEmpty(Authority.FunName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" FunName='" + Authority.FunName + "' ");
            }
            //判断父节点是否为空
            if (!string.IsNullOrEmpty(Authority.ParentID))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" ParentID='" + Authority.ParentID + "' ");
            }
            //判断权限路径是否为空
            if (!string.IsNullOrEmpty(Authority.Menu))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" Menu='" + Authority.Menu + "' ");
            }
            //判断权限备注是否为空
            if (Authority.SortCode > 0)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" SortCode='" + Authority.SortCode + "' ");
            }
            strWhere.Append(" order by SortCode asc ");
            sql += strWhere;
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

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="Authority">权限实体</param>
        /// <returns>返回添加返回数</returns>
        public int AddAuthority(AuthorityInfo Authority)
        {
            int Result = 0;
            if (Authority == null)
            {
                return Result;
            }
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
            parameter[0].Value = Authority.ParentID;
            parameter[1].Value = Authority.FunName;
            parameter[2].Value = Authority.AuNode;
            parameter[3].Value = Authority.Menu;
            parameter[4].Value = Authority.FunCode;
            parameter[5].Value = Authority.SortCode;
            try
            {
                Result = ExecuteNonQuery(sql, parameter);
                return Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="Authority">权限实体</param>
        /// <returns>返回添加返回数</returns>
        public int UpdateAuthority(AuthorityInfo Authority)
        {
            int Result = 0;
            if (Authority == null)
            {
                return Result;
            }
            string sql = " UPDATE T_Authority SET ";
            StringBuilder strWhere = new StringBuilder("");
            //判断权限功能名是否为空
            if (!string.IsNullOrEmpty(Authority.FunName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" FunName='" + Authority.FunName + "' ");
            }
            //判断权限父节点是否为空
            if (!string.IsNullOrEmpty(Authority.ParentID))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" ParentID='" + Authority.ParentID + "' ");
            }
            //判断权限目录是否为空
            if (!string.IsNullOrEmpty(Authority.Menu))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" Menu='" + Authority.Menu + "' ");
            }
            //判断标识码是否为空
            if (!string.IsNullOrEmpty(Authority.AuNode))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" AuNode='" + Authority.AuNode + "' ");
            }
            //判断权限备注是否为空
            if (Authority.SortCode > 0)
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
                strWhere.Append(" SortCode='" + Authority.SortCode + "' ");
            }
            //判断修改条件是否为空
            if (string.IsNullOrEmpty(strWhere.ToString()))
            {
                return Result;
            }
            else
            {
                strWhere.Append(" Where FunCode='" + Authority.FunCode + "' ");
            }
            sql += strWhere;
            try
            {
                Result = ExecuteNonQuery(sql);
                return Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="FunCode">权限实体</param>
        /// <returns>返回添加返回数</returns>
        public int DeleteAuthority(string FunCode)
        {
            int Result = 0;
            if (string.IsNullOrWhiteSpace(FunCode))
            {
                return Result;
            }
            string sql = @" Delete from T_Authority Where FunCode='" + FunCode + "' or ParentID='" + FunCode + "'";
            try
            {
                Result = ExecuteNonQuery(sql);
                return Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取mes用户
        /// </summary>
        /// <param name="MesUser"></param>
        /// <returns></returns>
        public List<MESUser> GetMESUser(MESUser MesUser)
        {
            List<MESUser> MesUsrList = new List<MESUser>();
            if (MesUser == null)
            {
                return MesUsrList;
            }
            string sql = @" select t1.USER_NAME as UserName,t2.PERSON_NAME as PersonName,t1.ID as UserID,t1.PERSON_ID as PersonID,t1.PASSWORD as UserPassword from SYS_USER_PROFILE t1 left join SYS_PERSON t2 on t1.PERSON_ID=t2.ID ";
            StringBuilder strWhere = new StringBuilder("");
            //判断用户真实姓名是否为空
            if (!string.IsNullOrEmpty(MesUser.PersonName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" t2.PERSON_NAME like '%" + MesUser.PersonName + "%' ");
            }
            //判断用户名是否为空
            if (!string.IsNullOrEmpty(MesUser.UserName))
            {
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" t1.USER_NAME like '%" + MesUser.UserName + "%'  ");
            }
            sql += strWhere;
            try
            {
                DataTable dt = GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                    MesUsrList = Util.DataTableConvertList<MESUser>(dt);
                return MesUsrList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取mes角色
        /// </summary>
        /// <param name="MesUser"></param>
        /// <returns></returns>
        public List<MESRole> GetMESRole(MESUser MesUser)
        {
            List<MESRole> MesRoleList = new List<MESRole>();
            if (MesUser == null)
            {
                return MesRoleList;
            }
            string sql = "";
            if (MesUser.UserID != null)
            {
                sql = string.Format(@" select t1.ID as RoleID,t1.ROLE_NAME as RoleName,(case when t2.ROLE_ID is null then '0' else '1' end)  RoleIdCheck from SYS_ROLE t1 left join (select * from SYS_USER_IN_ROLES 
where USER_ID='{0}') t2 on t1.ID=t2.ROLE_ID ", MesUser.UserID);
            }
            else
            {
                sql = string.Format(@" select t1.ID as ROLE_ID,t1.ROLE_NAME,0 as ROLE_ID_Check from SYS_ROLE t1 ");
            }
            try
            {
                DataTable dt = GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                    MesRoleList = Util.DataTableConvertList<MESRole>(dt);
                foreach (var row in MesRoleList)
                {
                    if (row.RoleIdCheck + "" == "0")
                        row.IsCheck = false;
                    else
                        row.IsCheck = true;
                }
                return MesRoleList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 添加mes用户
        /// </summary>
        /// <param name="MesUser"></param>
        /// <returns></returns>
        public int AddMESUser(MESUser MesUser)
        {
            int Result = 0;
            if (MesUser == null)
            {
                return Result;
            }
            List<string> SqlList = new List<string>();
            SqlList.Add(string.Format(@" INSERT INTO SYS_USER_PROFILE 
(ID, USER_NAME,PASSWORD,PASSWORD_SALT,PERSON_ID,MOBILE_PIN,PASSWORD_QUESTION,PASSWORD_ANSWER,IS_APPROVED,
DATETIME_LAST_ACTIVITY,DATETIME_LAST_LOGIN,DATETIME_LAST_PASSWORD_CHANGED,DATETIME_CREATION,DATETIME_LAST_LOCKED_OUT,
FAILED_PASSWORD_ATTEMPT_COUNT,FAILED_PASSWORD_ATTEMPT_WINDOW_START,FAILED_PASSWORD_ANSWER_ATTEMPT_COUNT,
FAILED_PASSWORD_ANSWER_ATTEMPT_WINDOW_START) 
VALUES('{0}','{1}','{2}','51Tg5W5K9IIrKmfISaGB4Q==','{3}','','my office name','D6YL8ChxSPHtEF0hsZrvAQ==',1,
GETDATE(),GETDATE(),GETDATE(),GETDATE(),GETDATE(),
0,GETDATE(),0,GETDATE())", MesUser.UserID, MesUser.UserName, MesUser.Password, MesUser.PersonID));
            SqlList.Add(string.Format(@" INSERT INTO SYS_PERSON (ID, PERSON_NAME) VALUES ('{0}', '{1}')", MesUser.PersonID, MesUser.PersonName));
            SqlList.Add(" Delete from SYS_USER_IN_ROLES Where USER_ID='" + MesUser.UserID + "' ");
            foreach (var row in MesUser.MESRoles)
            {
                if (row.IsCheck)
                    SqlList.Add(string.Format(@" INSERT INTO SYS_USER_IN_ROLES (USER_ID, ROLE_ID) VALUES ('{0}', '{1}')", MesUser.UserID, row.RoleID));
            }
            Result = BatchExecuteNonQuery(SqlList);
            return Result;
        }

        /// <summary>
        /// 判断是否添加mes用户
        /// </summary>
        /// <param name="MesUser"></param>
        /// <returns></returns>
        public int CheckAddMESUser(MESUser MesUser)
        {
            int Result = 0;
            if (MesUser == null)
            {
                return Result;
            }
            string sql = string.Format(@"select * from SYS_USER_PROFILE where user_name='{0}' ", MesUser.UserName);
            try
            {
                DataTable dt = GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                    Result = 1;
                return Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新mes用户
        /// </summary>
        /// <param name="MesUser"></param>
        /// <returns></returns>
        public int UpdateMESUser(MESUser MesUser)
        {
            int Result = 0;
            if (MesUser == null)
            {
                return Result;
            }
            List<string> SqlList = new List<string>();
            if (MesUser.ChangePassword)
            {
                SqlList.Add(string.Format(@" UPDATE SYS_USER_PROFILE SET PASSWORD='{0}' WHERE ID='{1}'", MesUser.Password, MesUser.UserID));
            }
            SqlList.Add(string.Format(@" UPDATE SYS_PERSON SET PERSON_NAME='{0}' WHERE ID='{1}'", MesUser.PersonName, MesUser.PersonID));
            SqlList.Add(" Delete from SYS_USER_IN_ROLES Where USER_ID='" + MesUser.UserID + "' ");
            foreach (var row in MesUser.MESRoles)
            {
                if (row.IsCheck)
                    SqlList.Add(string.Format(@" INSERT INTO SYS_USER_IN_ROLES (USER_ID, ROLE_ID) VALUES ('{0}', '{1}')", MesUser.UserID, row.RoleID));
            }
            Result = BatchExecuteNonQuery(SqlList);
            return Result;
        }

        /// <summary>
        /// 删除mes用户
        /// </summary>
        /// <param name="MesUser"></param>
        /// <returns></returns>
        public int DeleteMESUser(MESUser MesUser)
        {
            int Result = 0;
            if (MesUser == null)
            {
                return Result;
            }
            List<string> SqlList = new List<string>();
            SqlList.Add(" Delete from SYS_USER_PROFILE Where ID='" + MesUser.UserID + "' ");
            SqlList.Add(" Delete from SYS_PERSON Where ID='" + MesUser.PersonID + "' ");
            SqlList.Add(" Delete from SYS_USER_IN_ROLES Where USER_ID='" + MesUser.UserID + "' ");
            Result = BatchExecuteNonQuery(SqlList);
            return Result;
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
