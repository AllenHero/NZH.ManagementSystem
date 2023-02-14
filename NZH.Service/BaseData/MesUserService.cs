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
    public class MesUserService: BaseDataAdapter, IMesUserServicecs
    {
        public SQLBaseData(BaseDatabaseContext context) : base(context) { }

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

    }
}
