using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Model.BaseData
{
    public class TreeModle
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 代码（打开页面）
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public string FATHER_ID { get; set; }
        /// <summary>
        /// 名称（界面名称）
        /// </summary>
        public string NMAE { get; set; }

        public string AuNode { get; set; }
    }
}
