using NZH.IService.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.IService
{
    public abstract partial class BaseDatabaseContext
    {

        #region 系统配置部分


        #endregion

        #region  基础数据部分
        public IBaseData BaseData { get; set; }
        #endregion

    }
}
