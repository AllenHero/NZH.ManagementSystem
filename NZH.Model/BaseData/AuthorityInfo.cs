using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Model.BaseData
{
    public class AuthorityInfo
    {
        /// <summary>
        /// 权限ID
        /// </summary>		
        public int AuthorityID { get; set; }

        /// <summary>
        /// 父节点ID
        /// </summary>		
        public string ParentID { get; set; }

        /// <summary>
        /// 功能码
        /// </summary>		
        public string FunCode { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>		
        public string FunName { get; set; }

        /// <summary>
        /// 权限备注
        /// </summary>		
        public string AuNode { get; set; }

        /// <summary>
        /// 权限菜单备注
        /// </summary>		
        public string Menu { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>		
        public int SortCode { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int Enable { get; set; }
    }
}
