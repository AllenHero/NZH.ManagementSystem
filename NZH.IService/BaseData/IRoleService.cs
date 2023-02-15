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
        List<RoleInfo> GetRoleInfo(RoleInfo Role);

        List<RoleInfo> GetRoleInfoByUser(string RoleID);

        int AddRole(RoleInfo Role);

        int UpdateRole(RoleInfo Role);

        int DeleteRole(int RoleID);
    }
}
