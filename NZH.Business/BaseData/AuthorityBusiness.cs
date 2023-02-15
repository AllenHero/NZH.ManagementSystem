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
    }
}
