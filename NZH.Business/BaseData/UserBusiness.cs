using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Business.BaseData
{
    public class UserBusiness: BaseBusiness
    {
        public UserInfo Login(string UserName, string UserPassword)
        {
            UserInfo result = new UserInfo();
            string message = "";
            try
            {
                result = Context.BaseData.Login(UserName, UserPassword);

            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("Login", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int UpdatePassWord(string UserName, string userpassword)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.UpdatePassWord(UserName, userpassword);

            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("UpdatePassWord", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int AddUser(UserInfo user)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.AddUser(user);

            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("AddUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int UpdateUser(UserInfo user)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.UpdateUser(user);

            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("UpdateUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int DeleteUser(int UserID)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.DeleteUser(UserID);

            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("DeleteUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public UserInfo GetUserInfo(UserInfo role)
        {
            UserInfo retInfo = new UserInfo();
            string message = "";
            try
            {
                List<UserInfo> userinfo = Context.BaseData.GetUserInfo(role);
                retInfo.user = userinfo;
            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetUserInfo", EmergencyLevel.General, ex, out message);
            }
            return retInfo;
        }

        public UserInfo GetRoleInfo(RoleInfo role)
        {
            UserInfo retInfo = new UserInfo();
            string message = "";
            try
            {
                List<RoleInfo> roleinfo = Context.BaseData.GetRoleInfo(role);
                retInfo.role = roleinfo;
            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetRoleInfo", EmergencyLevel.General, ex, out message);
            }
            return retInfo;
        }

        public UserInfo GetRoleInfoByUser(string roleid)
        {
            UserInfo retInfo = new UserInfo();
            string message = "";
            try
            {
                List<RoleInfo> roleinfo = Context.BaseData.GetRoleInfoByUser(roleid);
                retInfo.role = roleinfo;
            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetRoleInfoByUser", EmergencyLevel.General, ex, out message);
            }
            return retInfo;
        }
    }
}
