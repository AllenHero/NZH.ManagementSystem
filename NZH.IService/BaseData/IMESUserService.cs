using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.IService.BaseData
{
    public interface IMESUserService
    {
        List<MESUser> GetMESUser(MESUser MesUser);

        List<MESRole> GetMESRole(MESUser MesUser);

        int AddMESUser(MESUser MesUser);

        int CheckAddMESUser(MESUser MesUser);

        int UpdateMESUser(MESUser MesUser);

        int DeleteMESUser(MESUser MesUser);
    }
}
