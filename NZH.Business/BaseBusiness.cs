using NZH.IService;
using NZH.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Business
{
    public abstract class BaseBusiness
    {
        /// <summary>
        /// 数据库上下文。
        /// </summary>
        protected BaseDatabaseContext Context { get; private set; }

        public BaseBusiness()
        {
            ///如存在多种数据库，则需要在这里使用策略模式。
            Context = new SQLDatabaseContext();
        }

        public bool ExceptionLog(string methodName, string level, Exception ex, out string messageFromExceptionLog)
        {
            messageFromExceptionLog = "NG: Exception occur, more detail in the log";
            messageFromExceptionLog = ex.ToString();
            return true;
        }

        public bool ExceptionLog(string methodName, string level, Exception ex)
        {
            return true;
        }

        public static bool IsObjEquals(object oldObjValue, object newObjValue)
        {
            if (oldObjValue == null || string.IsNullOrEmpty(oldObjValue.ToString()))
            {
                if (newObjValue == null || string.IsNullOrEmpty(newObjValue.ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return oldObjValue.Equals(newObjValue);
            }
        }
    }

    public sealed class EmergencyLevel
    {
        public static string Low
        {
            get { return "1"; }
        }

        public static string General
        {
            get { return "2"; }
        }

        public static string High
        {
            get { return "3"; }
        }
    }
}
