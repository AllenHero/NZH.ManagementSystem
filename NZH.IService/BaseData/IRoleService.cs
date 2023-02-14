using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.IService.BaseData
{
    public interface IRoleService
    {
        List<RoleInfo> GetRoleInfo(RoleInfo role);

        List<RoleInfo> GetRoleInfoByUser(string roleid);

        int AddRole(RoleInfo role);

        int UpdateRole(RoleInfo role);

        int DeleteRole(int RoleID);
    }
}
