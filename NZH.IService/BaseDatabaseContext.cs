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
        public IBaseData BaseData { get; set; }
    }
}
