using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.IService.BaseData
{
    public interface IBaseData
    {
        UserInfo Login(string UserName, string UserPassword);

        int UpdatePassWord(string UserName, string UserPassword);

        int AddUser(UserInfo User);

        int UpdateUser(UserInfo User);

        int DeleteUser(int UserID);

        List<UserInfo> GetUserInfo(UserInfo User);

        List<RoleInfo> GetRoleInfo(RoleInfo Role);

        List<RoleInfo> GetRoleInfoByUser(string RoleID);

        int AddRole(RoleInfo Role);

        int UpdateRole(RoleInfo Role);

        int DeleteRole(int RoleID);

        List<AuthorityInfo> GetAuthorityInfo(AuthorityInfo Authority, bool Type);

        int AddAuthority(AuthorityInfo Authority);

        int UpdateAuthority(AuthorityInfo Authority);

        int DeleteAuthority(string FunCode);

        List<MESUser> GetMESUser(MESUser MesUser);

        List<MESRole> GetMESRole(MESUser MesUser);

        int AddMESUser(MESUser MesUser);

        int CheckAddMESUser(MESUser MesUser);

        int UpdateMESUser(MESUser MesUser);

        int DeleteMESUser(MESUser MesUser);
    }
}
