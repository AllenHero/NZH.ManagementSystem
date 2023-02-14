using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Model.BaseData
{
    public class RoleModel
    {
        /// <summary>
        /// 角色ID
        /// </summary>		
        public int RoleID { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>		
        public string RoleName { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>		
        public bool IsCheck { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>		
        public string AuthorityID { get; set; }
    }
}
