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

        int UpdatePassWord(string UserName, string userpassword);

        int AddUser(UserInfo user);

        int UpdateUser(UserInfo user);

        int DeleteUser(int UserID);

        List<UserInfo> GetUserInfo(UserInfo user);

        List<RoleInfo> GetRoleInfo(RoleInfo role);

        List<RoleInfo> GetRoleInfoByUser(string roleid);

        int AddRole(RoleInfo role);

        int UpdateRole(RoleInfo role);

        int DeleteRole(int RoleID);

        List<AuthorityInfo> GetAuthorityInfo(AuthorityInfo authority);

        int AddAuthority(AuthorityInfo authority);

        int UpdateAuthority(AuthorityInfo authority);

        int DeleteAuthority(string FunCode);

        List<MESUser> GetMESUser(MESUser user);

        List<MESRole> GetMESRole(MESUser user);

        int MESAddUser(MESUser user);

        int CheckMESAddUser(MESUser user);

        int MESUpdateUser(MESUser user);

        int MESDeleteUser(MESUser user);
    }
}
