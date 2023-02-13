using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Common.Extensions
{
    public static class ExtendByte
    {
        #region 计算hash

        /// <summary>
        /// 计算hash
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hashName"></param>
        /// <returns></returns>
        public static byte[] Hash(this byte[] value, string hashName)
        {
            HashAlgorithm algorithm;
            if (string.IsNullOrEmpty(hashName))
            {
                algorithm = HashAlgorithm.Create();
            }
            else
            {
                algorithm = HashAlgorithm.Create(hashName);
            }
            return algorithm.ComputeHash(value);
        }

        /// <summary>
        ///  计算hash
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Hash(this byte[] value)
        {
            return Hash(value, null);
        }

        #endregion

        /// <summary>
        /// 转换为十六进制字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHex(this byte value)
        {
            return value.ToString("X2");
        }

        /// <summary>
        /// 创建一个新文件，在其中写入指定的字节数组，然后关闭该文件。如果目标文件已存在，则覆盖该文件。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public static void WriteFile(this byte[] value, string path)
        {
            File.WriteAllBytes(path, value);
        }

        /// <summary>
        /// 转换为内存流 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MemoryStream ToMemoryStream(this byte[] value)
        {
            if (value != null)
            {
                if (value.Length == 0)
                {
                    return null;
                }
                else
                {
                    return new MemoryStream(value);
                }
            }
            return null;
        }
    }
}
