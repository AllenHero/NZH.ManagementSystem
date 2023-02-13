using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Model.BaseData
{
    public class MESRole
    {
        public Guid ROLE_ID { get; set; }

        public string ROLE_NAME { get; set; }

        public string ROLE_ID_Check { get; set; }

        public bool isCheck { get; set; }
    }
}
