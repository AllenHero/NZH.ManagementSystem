using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.IService.BaseData
{
    public interface IAuthorityService
    {
        List<AuthorityInfo> GetAuthorityInfo(AuthorityInfo authority);

        int AddAuthority(AuthorityInfo authority);

        int UpdateAuthority(AuthorityInfo authority);

        int DeleteAuthority(string FunCode);
    }
}
