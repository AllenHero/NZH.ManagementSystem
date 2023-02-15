using NZH.Common.Extensions;
using NZH.IService;
using NZH.IService.BaseData;
using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Service.BaseData
{
    public class MESUserService : SQLBaseData, IMESUserService
    {
        public MESUserService(BaseDatabaseContext context) : base(context) { }

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
    }
}
