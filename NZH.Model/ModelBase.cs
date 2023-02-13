using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Model
{
    public class ModelBase
    {
        /// <summary>
        /// 获取或设置ID。
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 获取或设置创建者。
        /// </summary>
        public string CREATOR { get; set; }

        /// <summary>
        /// 获取或设置创建时间。
        /// </summary>
        public DateTime DATETIME_CREATED { get; set; }

        /// <summary>
        /// 获取或设置修改人。
        /// </summary>
        public string MODIFIER { get; set; }

        /// <summary>
        /// 获取或设置修改时间。
        /// </summary>
        public DateTime DATETIME_MODIFIED { get; set; }

        /// <summary>
        /// 获取或设置标识。
        /// </summary>
        public int FLAG { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 登录主机号。
        /// </summary>
        public string HOST_NAME { get; set; }

        /// <summary>
        /// 获取或设置标识。
        /// </summary>
        public string STATE { get; set; }

        /// <summary>
        /// A返回True
        /// D返回False
        /// </summary>
        public bool BoolState
        {
            get { return STATE == "A" ? true : false; }
            set
            {
                if (value)
                    STATE = "A";
                else
                    STATE = "D";
            }
        }

        /// <summary>
        /// 是否生效
        /// </summary>
        public string StateString { get { return BoolState ? "生效" : "失效"; } }
    }
}
