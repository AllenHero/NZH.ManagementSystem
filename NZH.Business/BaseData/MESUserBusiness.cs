using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Business.BaseData
{
    public class MESUserBusiness: BaseBusiness
    {
        public List<MESUser> GetMESUser(MESUser MesUser)
        {
            List<MESUser> MESUsers = new List<MESUser>();
            string message = "";
            try
            {
                MESUsers = this.Context.MESUserService.GetMESUser(MesUser);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetMESUser", EmergencyLevel.General, ex, out message);
            }
            return MESUsers;
        }

        public List<MESRole> GetMESRole(MESUser MesUser)
        {
            List<MESRole> MESRoles = new List<MESRole>();
            string message = "";
            try
            {
                MESRoles = this.Context.MESUserService.GetMESRole(MesUser);
            }
            catch (Exception ex)
            {
                base.ExceptionLog("GetMESRole", EmergencyLevel.General, ex, out message);
            }
            return MESRoles;
        }

        public int AddMESUser(MESUser MesUser)
        {
            int result = 0;
            string message = "";
            try
            {
                result = this.Context.MESUserService.AddMESUser(MesUser);
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
                result = this.Context.MESUserService.CheckAddMESUser(MesUser);
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
                result = this.Context.MESUserService.UpdateMESUser(MesUser);
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
                result = this.Context.MESUserService.DeleteMESUser(MesUser);
            }
            catch (System.Exception ex)
            {
                base.ExceptionLog("MESDeleteUser", EmergencyLevel.General, ex, out message);
            }
            return result;
        }
    }
}
