using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Business.BaseData
{
    public class BllBaseData : BaseBusiness
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

        public int AddRole(RoleInfo role)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.AddRole(role);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("AddRole", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int UpdateRole(RoleInfo role)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.UpdateRole(role);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("UpdateRole", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int DeleteRole(int RoleID)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.DeleteRole(RoleID);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("DeleteRole", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public UserInfo GetAuthorityInfo(AuthorityInfo role)
        {
            UserInfo retInfo = new UserInfo();
            string message = "";
            try
            {
                List<AuthorityInfo> roleinfo = Context.BaseData.GetAuthorityInfo(role);
                retInfo.authority = roleinfo;
            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetAuthorityInfo", EmergencyLevel.General, ex, out message);
            }
            return retInfo;
        }

        public int AddAuthority(AuthorityInfo authority)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.AddAuthority(authority);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("AddAuthority", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int UpdateAuthority(AuthorityInfo authority)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.UpdateAuthority(authority);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("UpdateAuthority", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int DeleteAuthority(string FunCode)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.DeleteAuthority(FunCode);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("DeleteAuthority", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public List<MESUser> GetMESUser(MESUser user)
        {
            List<MESUser> reslut = new List<MESUser>();
            string message = "";
            try
            {
                reslut = Context.BaseData.GetMESUser(user);

            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetMESUser", EmergencyLevel.General, ex, out message);
            }
            return reslut;
        }

        public List<MESRole> GetMESRole(MESUser user)
        {
            List<MESRole> reslut = new List<MESRole>();
            string message = "";
            try
            {
                reslut = Context.BaseData.GetMESRole(user);

            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetMESRole", EmergencyLevel.General, ex, out message);
            }
            return reslut;
        }

        public int MESAddUser(MESUser user)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.MESAddUser(user);

            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("MESAddUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int CheckMESAddUser(MESUser user)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.CheckMESAddUser(user);

            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("CheckMESAddUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int MESUpdateUser(MESUser user)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.MESUpdateUser(user);

            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("MESUpdateUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int MESDeleteUser(MESUser user)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.BaseData.MESDeleteUser(user);

            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("MESDeleteUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

    }
}
