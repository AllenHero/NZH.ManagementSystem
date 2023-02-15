using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Business.BaseData
{
    public class RoleBusiness: BaseBusiness
    {
        public UserInfo GetRoleInfo(RoleInfo Role)
        {
            UserInfo retInfo = new UserInfo();
            string message = "";
            try
            {
                List<RoleInfo> roleinfo = this.Context.RoleService.GetRoleInfo(Role);
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
                List<RoleInfo> roleinfo = this.Context.RoleService.GetRoleInfoByUser(RoleID);
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
                result = this.Context.RoleService.AddRole(Role);
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
                result = this.Context.RoleService.UpdateRole(Role);
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
                result = this.Context.RoleService.DeleteRole(RoleID);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("DeleteRole", EmergencyLevel.General, ex, out message);
            }
            return result;
        }
    }
}
