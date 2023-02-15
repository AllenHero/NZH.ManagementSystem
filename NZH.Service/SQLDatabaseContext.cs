using NZH.IService;
using NZH.Service.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Service
{
    public partial class SQLDatabaseContext : BaseDatabaseContext
    {
        public SQLDatabaseContext()
        {
            BaseData = new SQLBaseData(this);

            UserService = new UserService(this);

            RoleService = new RoleService(this);

            AuthorityService = new AuthorityService(this);

            MESUserService = new MESUserService(this);
        }
    }
}
