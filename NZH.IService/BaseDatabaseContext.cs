using NZH.IService.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.IService
{
    public abstract partial class BaseDataBaseContext
    {
        public IUserService UserService { get; set; }

        public IRoleService RoleService { get; set; }

        public IAuthorityService AuthorityService { get; set; }

        public IMESUserService MESUserService { get; set; }
    }
}
