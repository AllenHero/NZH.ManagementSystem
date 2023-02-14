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
    public class AuthorityService: BaseDataAdapter, IAuthorityService
    {
        public SQLBaseData(BaseDatabaseContext context) : base(context) { }

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
    }
}
