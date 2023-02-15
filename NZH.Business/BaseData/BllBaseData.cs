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
                result = Context.UserService.Login(UserName, UserPassword);
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
                result = Context.UserService.UpdatePassWord(UserName, UserPassword);
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
                result = Context.UserService.AddUser(User);
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
                result = Context.UserService.UpdateUser(User);
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
                result = Context.UserService.DeleteUser(UserID);
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
                List<UserInfo> userinfo = Context.UserService.GetUserInfo(Role);
                retInfo.Users = userinfo;
            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetUserInfo", EmergencyLevel.General, ex, out message);
            }
            return retInfo;
        }

        public UserInfo GetRoleInfo(RoleInfo Role)
        {
            UserInfo retInfo = new UserInfo();
            string message = "";
            try
            {
                List<RoleInfo> roleinfo = Context.RoleService.GetRoleInfo(Role);
                retInfo.Roles = roleinfo;
            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetRoleInfo", EmergencyLevel.General, ex, out message);
            }
            return retInfo;
        }

        public UserInfo GetRoleInfoByUser(string RoleID)
        {
            UserInfo retInfo = new UserInfo();
            string message = "";
            try
            {
                List<RoleInfo> roleinfo = Context.RoleService.GetRoleInfoByUser(RoleID);
                retInfo.Roles = roleinfo;
            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetRoleInfoByUser", EmergencyLevel.General, ex, out message);
            }
            return retInfo;
        }

        public int AddRole(RoleInfo Role)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.RoleService.AddRole(Role);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("AddRole", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int UpdateRole(RoleInfo Role)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.RoleService.UpdateRole(Role);
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
                result = Context.RoleService.DeleteRole(RoleID);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("DeleteRole", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public UserInfo GetAuthorityInfo(AuthorityInfo Authority, bool Type)
        {
            UserInfo retInfo = new UserInfo();
            string message = "";
            try
            {
                List<AuthorityInfo> roleinfo = Context.AuthorityService.GetAuthorityInfo(Authority, Type);
                retInfo.Authoritys = roleinfo;
            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetAuthorityInfo", EmergencyLevel.General, ex, out message);
            }
            return retInfo;
        }

        public int AddAuthority(AuthorityInfo Authority)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.AuthorityService.AddAuthority(Authority);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("AddAuthority", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int UpdateAuthority(AuthorityInfo Authority)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.AuthorityService.UpdateAuthority(Authority);
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
                result = Context.AuthorityService.DeleteAuthority(FunCode);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("DeleteAuthority", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public List<MESUser> GetMESUser(MESUser MesUser)
        {
            List<MESUser> reslut = new List<MESUser>();
            string message = "";
            try
            {
                reslut = Context.MESUserService.GetMESUser(MesUser);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetMESUser", EmergencyLevel.General, ex, out message);
            }
            return reslut;
        }

        public List<MESRole> GetMESRole(MESUser MesUser)
        {
            List<MESRole> reslut = new List<MESRole>();
            string message = "";
            try
            {
                reslut = Context.MESUserService.GetMESRole(MesUser);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetMESRole", EmergencyLevel.General, ex, out message);
            }
            return reslut;
        }

        public int AddMESUser(MESUser MesUser)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.MESUserService.AddMESUser(MesUser);
            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("MESAddUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int CheckAddMESUser(MESUser MesUser)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.MESUserService.CheckAddMESUser(MesUser);
            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("CheckMESAddUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int UpdateMESUser(MESUser MesUser)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.MESUserService.UpdateMESUser(MesUser);
            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("MESUpdateUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }

        public int DeleteMESUser(MESUser MesUser)
        {
            int result = 0;
            string message = "";
            try
            {
                result = Context.MESUserService.DeleteMESUser(MesUser);
            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("MESDeleteUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }
    }
}
