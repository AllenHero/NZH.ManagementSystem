using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Model.BaseData
{
    public class MESRole
    {
        public Guid RoleID { get; set; }

        public string RoleName { get; set; }

        public string RoleIdCheck { get; set; }

        public bool IsCheck { get; set; }
    }
}
