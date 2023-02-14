using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Business.BaseData
{
    public class MesUserBusiness: BaseBusiness
    {
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
