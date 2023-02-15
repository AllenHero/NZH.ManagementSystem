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
    public class RoleService : SQLBaseData, IRoleService
    {
        public RoleService(BaseDatabaseContext context) : base(context) { }

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
    }
}
