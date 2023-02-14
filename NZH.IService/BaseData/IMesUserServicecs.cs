using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.IService.BaseData
{
    public interface IMesUserServicecs
    {
        List<MESUser> GetMESUser(MESUser user);

        List<MESRole> GetMESRole(MESUser user);

        int MESAddUser(MESUser user);

        int CheckMESAddUser(MESUser user);

        int MESUpdateUser(MESUser user);

        int MESDeleteUser(MESUser user);
    }
}
