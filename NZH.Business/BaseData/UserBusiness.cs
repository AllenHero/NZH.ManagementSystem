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
                result = this.Context.UserService.Login(UserName, UserPassword);
            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("Login", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int UpdatePassWord(string UserName, string UserPassword)
        {
            int result = 0;
            string message = "";
            try
            {
                result = this.Context.UserService.UpdatePassWord(UserName, UserPassword);
            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("UpdatePassWord", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int AddUser(UserInfo User)
        {
            int result = 0;
            string message = "";
            try
            {
                result = this.Context.UserService.AddUser(User);
            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("AddUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int UpdateUser(UserInfo User)
        {
            int result = 0;
            string message = "";
            try
            {
                result = this.Context.UserService.UpdateUser(User);
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
                result = this.Context.UserService.DeleteUser(UserID);
            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("DeleteUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public UserInfo GetUserInfo(UserInfo Role)
        {
            UserInfo retInfo = new UserInfo();
            string message = "";
            try
            {
                List<UserInfo> userinfo = this.Context.UserService.GetUserInfo(Role);
                retInfo.Users = userinfo;
            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetUserInfo", EmergencyLevel.General, ex, out message);
            }
            return retInfo;
        }
    }
}
