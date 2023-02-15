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
    public class AuthorityService : SQLBaseData, IAuthorityService
    {
        public AuthorityService(BaseDatabaseContext context) : base(context) { }

        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <param name="Authority">权限实体</param>
        /// <param name="Type">权限类型</param>
        /// <returns>权限信息的集合</returns>
        public List<AuthorityInfo> GetAuthorityInfo(AuthorityInfo Authority, bool Type)
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
            if (!Type)//true-查所有权限;false-查启用权限
            {
                //启用
                strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? "Where " : " And ");
                strWhere.Append(" Enable='1' ");
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
     VALUES(@ParentID, @FunName, @AuNode, @Menu, @FunCode,@SortCode,@Enable)";
            SqlParameter[] parameter = {
                                new SqlParameter("@ParentID",SqlDbType.VarChar),
                                new SqlParameter("@FunName",SqlDbType.VarChar),
                                new SqlParameter("@AuNode",SqlDbType.VarChar),
                                new SqlParameter("@Menu",SqlDbType.VarChar),
                                new SqlParameter("@FunCode",SqlDbType.VarChar),
                                new SqlParameter("@SortCode",SqlDbType.Int),
                                new SqlParameter("@Enable",SqlDbType.Int)
                                           };
            parameter[0].Value = Authority.ParentID;
            parameter[1].Value = Authority.FunName;
            parameter[2].Value = Authority.AuNode;
            parameter[3].Value = Authority.Menu;
            parameter[4].Value = Authority.FunCode;
            parameter[5].Value = Authority.SortCode;
            parameter[6].Value = Authority.Enable;
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
            strWhere.Append(string.IsNullOrEmpty(strWhere.ToString()) ? " " : ", ");
            strWhere.Append(" Enable='" + Authority.Enable + "' ");//是否启用
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
    }
}
