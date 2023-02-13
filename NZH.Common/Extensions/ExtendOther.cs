using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Common.Extensions
{
    public static class ExtendOther
    {
        /// <summary>
        /// 确定集合中的是否存在传入值，用Equals比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsExist<T>(this T value, params T[] list)
        {
            return list.Any(item => item.Equals(value));
        }

        /// <summary>
        ///  ForEach扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}
