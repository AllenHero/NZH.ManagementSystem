using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Business.BaseData
{
    public class AuthorityBusiness: BaseBusiness
    {
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
    }
}
