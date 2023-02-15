using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.IService
{
    public abstract class BaseDataAdapter
    {
        /// <summary>
        /// 数据库上下文。
        /// </summary>
        public BaseDataBaseContext Context { get; private set; }

        public BaseDataAdapter(BaseDataBaseContext context)
        {
            this.Context = context;
        }
    }
}
