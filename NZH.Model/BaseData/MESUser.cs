using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Model.BaseData
{
    public class MESUser
    {
        public Guid UserID { get; set; }

        public Guid PersonID { get; set; }

        public string UserName { get; set; }

        public string PersonName { get; set; }

        public string Password { get; set; }

        public bool ChangePassword { get; set; }

        public List<MESRole> MESRoles { get; set; }
    }
}
