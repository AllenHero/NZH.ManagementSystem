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
        List<AuthorityInfo> GetAuthorityInfo(AuthorityInfo Authority, bool Type);

        int AddAuthority(AuthorityInfo Authority);

        int UpdateAuthority(AuthorityInfo Authority);

        int DeleteAuthority(string FunCode);

        List<AuthorityInfo> GetAuthorityInfoByRole(string AuthorityID);
    }
}
