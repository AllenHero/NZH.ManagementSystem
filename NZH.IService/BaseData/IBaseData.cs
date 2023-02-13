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

    /// <summary>
    /// 关于美的设备信息的接口定义
    /// </summary>

    public interface IBase<T>
    {
        bool AddEntity(T entity);
        bool DelEntity(T entity);
        bool UpdateEntity(T entity);
        ICollection<T> FindEntity(T entity);
        ICollection<T> FindEntity(T entity, out int totalPage, out int totalRecord, int pageSize, int pageIndex, out string message);
        T GetByID(string ID);
    }
}
