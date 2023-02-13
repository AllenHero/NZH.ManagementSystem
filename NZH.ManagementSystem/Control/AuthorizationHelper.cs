using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NZH.ManagementSystem.Control
{
    /// <summary>
    /// 授权帮助类。
    /// </summary>
    public sealed class AuthorizationHelper
    {
        const string MACHINE_VALIDATION_KEY = "C50B3C89CB21F4F1422FF158A5B42D0E8DB8CB5CDA1742572A487D9401E3400267682B202B746511891C1BAF47F8D25C07F6C39A104696DB51F17C529AD3CABE";
        const string MACHINE_DECRYPTION_KEY = "8A9BE8FD67AF6979E7D20198CFEA50DD3D3799C77AF2B72F";

        private int newPasswordLength = 8;

        /// <summary>
        /// 指示成员资格提供程序是否配置为允许用户重置其密码。
        /// </summary>
        public readonly bool EnablePasswordReset = true;

        /// <summary>
        /// 指示成员资格提供程序是否配置为允许用户检索其密码。
        /// </summary>
        public readonly bool EnablePasswordRetrieval = true;

        /// <summary>
        /// 获取一个值，该值指示成员资格提供程序是否配置为要求用户在进行密码重置和检索时回答密码提示问题。
        /// </summary>
        public readonly bool RequiresQuestionAndAnswer = true;

        /// <summary>
        /// 获取一个值，指示成员资格提供程序是否配置为要求每个用户名具有唯一的电子邮件地址。
        /// </summary>
        public readonly bool RequiresUniqueEmail = true;

        /// <summary>
        /// 获取锁定成员资格用户前允许的无效密码或无效密码提示问题答案尝试次数。
        /// </summary>
        public readonly int MaxInvalidPasswordAttempts = 5;

        /// <summary>
        /// 获取在锁定成员资格用户之前允许的最大无效密码或无效密码提示问题答案尝试次数的分钟数。
        /// </summary>
        public readonly int PasswordAttemptWindow = 10;

        /// <summary>
        /// 获取有效密码中必须包含的最少特殊字符数。
        /// </summary>
        public readonly int MinRequiredNonAlphanumericCharacters = 1;

        /// <summary>
        /// 获取密码所要求的最小长度。
        /// </summary>
        public readonly int MinRequiredPasswordLength = 7;

        /// <summary>
        /// 获取用于计算密码的正则表达式。
        /// </summary>
        public readonly string PasswordStrengthRegularExpression = "";

        /// <summary>
        /// 用于生成密码的盐度混淆。
        /// </summary>
        /// <returns></returns>
        static internal string GenerateSalt()
        {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        public static string EncodePassword(string password)
        {
            string encodedPassword = password.DESEncryptString(MACHINE_VALIDATION_KEY, MACHINE_DECRYPTION_KEY);
            return encodedPassword;
        }

        public static string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;
            password = encodedPassword.DESDecryptString(MACHINE_VALIDATION_KEY, MACHINE_DECRYPTION_KEY);
            return password;
        } 
    }

    /// <summary>
    /// 随机密码生成器。
    /// </summary>
    public class PasswordGenerator
    {
        public PasswordGenerator()
        {
            this.Minimum = DefaultMinimum;
            this.Maximum = DefaultMaximum;
            this.ConsecutiveCharacters = false;
            this.RepeatCharacters = true;
            this.ExcludeSymbols = false;
            this.Exclusions = null;
            rng = new RNGCryptoServiceProvider();
        }

        protected int GetCryptographicRandomNumber(int lBound, int uBound)
        {
            // 假定 lBound >= 0 && lBound < uBound
            // 返回一个 int >= lBound and < uBound
            uint urndnum;
            byte[] rndnum = new Byte[4];
            if (lBound == uBound - 1)
            {
                // 只有iBound返回的情况  
                return lBound;
            }
            uint xcludeRndBase = (uint.MaxValue - (uint.MaxValue % (uint)(uBound - lBound)));
            do
            {
                rng.GetBytes(rndnum);
                urndnum = System.BitConverter.ToUInt32(rndnum, 0);
            } while (urndnum >= xcludeRndBase);
            return (int)(urndnum % (uBound - lBound)) + lBound;
        }

        protected char GetRandomCharacter()
        {
            int upperBound = pwdCharArray.GetUpperBound(0);
            if (true == this.ExcludeSymbols)
            {
                upperBound = PasswordGenerator.UBoundDigit;
            }

            int randomCharPosition = GetCryptographicRandomNumber(pwdCharArray.GetLowerBound(0), upperBound);
            char randomChar = pwdCharArray[randomCharPosition];
            return randomChar;
        }

        public string Generate()
        {
            // 得到minimum 和 maximum 之间随机的长度
            int pwdLength = GetCryptographicRandomNumber(this.Minimum, this.Maximum);
            StringBuilder pwdBuffer = new StringBuilder();
            pwdBuffer.Capacity = this.Maximum;
            // 产生随机字符
            char lastCharacter, nextCharacter;
            // 初始化标记
            lastCharacter = nextCharacter = 'n';

            for (int i = 0; i < pwdLength; i++)
            {
                nextCharacter = GetRandomCharacter();
                if (false == this.ConsecutiveCharacters)
                {
                    while (lastCharacter == nextCharacter)
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }

                if (false == this.RepeatCharacters)
                {
                    string temp = pwdBuffer.ToString();
                    int duplicateIndex = temp.IndexOf(nextCharacter);

                    while (-1 != duplicateIndex)
                    {
                        nextCharacter = GetRandomCharacter();
                        duplicateIndex = temp.IndexOf(nextCharacter);
                    }
                }

                if ((null != this.Exclusions))
                {
                    while (-1 != this.Exclusions.IndexOf(nextCharacter))
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }
                pwdBuffer.Append(nextCharacter);
                lastCharacter = nextCharacter;
            }


            if (null != pwdBuffer)
            {
                return pwdBuffer.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        public bool ConsecutiveCharacters
        {
            get { return this.hasConsecutive; }
            set { this.hasConsecutive = value; }
        }

        public bool ExcludeSymbols
        {
            get { return this.hasSymbols; }
            set { this.hasSymbols = value; }
        }

        public string Exclusions
        {
            get { return this.exclusionSet; }
            set { this.exclusionSet = value; }
        }

        public int Maximum
        {
            get { return this.maxSize; }
            set
            {
                this.maxSize = value;
                if (this.minSize >= this.maxSize)
                {
                    this.maxSize = PasswordGenerator.DefaultMaximum;
                }
            }
        }

        public int Minimum
        {
            get { return this.minSize; }
            set
            {
                this.minSize = value;
                if (PasswordGenerator.DefaultMinimum > this.minSize)
                {
                    this.minSize = PasswordGenerator.DefaultMinimum;
                }
            }
        }

        public bool RepeatCharacters
        {
            get { return this.hasRepeating; }
            set { this.hasRepeating = value; }
        }

        private const int DefaultMaximum = 10;
        private const int DefaultMinimum = 6;
        private const int UBoundDigit = 61;
        private string exclusionSet;
        private bool hasConsecutive;
        private bool hasRepeating;
        private bool hasSymbols;
        private int maxSize;
        private int minSize;
        private char[] pwdCharArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789`~!@#$^*()-_=+[]{}\\|;:'\",./".ToCharArray();
        private RNGCryptoServiceProvider rng;
    }

    /// <summary>
    /// 提供一组对字符串加密和解密的方法。
    /// </summary>
    public static class StringEncryptionExtension
    {
        #region DES

        /// <summary> 
        /// DES 加密字符串。
        /// </summary>
        /// <param name="pString">需要加密的字符串</param>
        /// <param name="pKey">密钥字符串</param>
        /// <param name="pIV">IV字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string DESEncryptString(this string pString, string pKey, string pIV)
        {
            return DES.DESEncryptString(pString, pKey, pIV);
        }

        /// <summary> 
        /// DES 解密字符串
        /// </summary>
        /// <param name="pString">需要解密的字符串</param>
        /// <param name="pKey">密钥字符串</param>
        /// <param name="pIV">IV字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string DESDecryptString(this string pString, string pKey, string pIV)
        {
            return DES.DESDecryptString(pString, pKey, pIV);
        }

        /// <summary> 
        /// TripleDES 加密字符串。
        /// </summary>
        /// <param name="pString">需要加密的字符串</param>
        /// <param name="pKey">密钥字符串</param>
        /// <param name="pIV">IV字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string TripleDESEncryptString(this string pString, string pKey, string pIV)
        {
            return DES.TripleDESEncryptString(pString, pKey, pIV);
        }

        /// <summary> 
        /// TripleDES 解密字符串
        /// </summary>
        /// <param name="pString">需要解密的字符串</param>
        /// <param name="pKey">密钥字符串</param>
        /// <param name="pIV">IV字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string TripleDESDecryptString(this string pString, string pKey, string pIV)
        {
            return DES.TripleDESDecryptString(pString, pKey, pIV);
        }

        #endregion

        #region MD5

        /// <summary>
        /// MD5 字符串散列加密，该加密不能逆转。
        /// </summary>
        /// <param name="pString">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5EncryptString(this string pString)
        {
            return MD5.EncryptString(pString);
        }

        #endregion

        #region SHA1

        /// <summary>
        /// SHA1 字符串散列加密，该加密不能逆转。
        /// </summary>
        /// <param name="pString">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string SHA1EncryptString(this string pString)
        {
            return SHA1.EncryptString(pString);
        }

        #endregion
    }

    internal sealed class DES
    {
        #region Private methods

        /// <summary>
        /// 根据传递的加密服务加密字符串。
        /// </summary>
        /// <param name="pCSP">加密服务提供者引用，需要在调用前设置 Key 和 IV 值。</param>
        /// <param name="pString">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        private static string _EncryptString(SymmetricAlgorithm pCSP, string pString)
        {
            //加密方法需要此接口才能在任何服务提供程序上调用 CreateEncryptor 方
            //法，服务提供程序将返回定义该接口的实际 encryptor 对象。
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            ct = pCSP.CreateEncryptor(pCSP.Key, pCSP.IV);
            //然后需要将原始字符串转换成字节数组。大多数 .NET 加密算法处理的是字
            //节数组而不是字符串。
            byt = Encoding.UTF8.GetBytes(pString);
            //现在可以执行实际的加密了。此进程需要创建一个数据流，用于将加密的字
            //节写入到其中。要使用名为 ms 的 MemoryStream 对象、ICryptoTransform 
            //对象（提供给 CryptoStream 类的构造函数）以及说明您希望在何种模式（
            //读、写等）下创建该类的枚举常数。创建 CryptoStream 对象 cs 后，现在
            //使用 CryptoStream 对象的 Write 方法将数据写入到内存数据流。这就是
            //进行实际加密的方法，加密每个数据块时，数据将被写入 MemoryStream 对
            //象。
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            //创建 MemoryStream 后，该代码将在 CryptoStream 对象上执行 
            //FlushFinalBlock 方法，以确保所有数据均被写入 MemoryStream 对象。该
            //过程将关闭 CryptoStream 对象。
            cs.FlushFinalBlock();
            cs.Close();
            //最后，该过程将内存数据流从字节数组转换回字符串，这样才可以在窗体上
            //的文本框内显示该字符串。可以使用 MemoryStream ToArray() 方法从数据
            //流中获取字节数组，然后调用 Convert.ToBase64String() 方法，该方法接
            //受字节数组输入并使用 Base64 编码方法将该字符串编码为可读内容。
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// 根据传递的解密服务解密字符串。
        /// </summary>
        /// <param name="pCSP">解密服务提供者引用，需要在调用前设置 Key 和 IV 值。</param>
        /// <param name="pString">需要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        private static string _DecryptString(SymmetricAlgorithm pCSP, string pString)
        {
            //加密方法需要此接口才能在任何服务提供程序上调用 CreateEncryptor 方
            //法，服务提供程序将返回定义该接口的实际 encryptor 对象。
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            try
            {
                ct = pCSP.CreateDecryptor(pCSP.Key, pCSP.IV);
                //然后需要将原始字符串转换成字节数组。大多数 .NET 加密算法处理的是字
                //节数组而不是字符串。此处与加密方法不同。
                byt = Convert.FromBase64String(pString);
                //现在可以执行实际的加密了。此进程需要创建一个数据流，用于将加密的字
                //节写入到其中。要使用名为 ms 的 MemoryStream 对象、ICryptoTransform 
                //对象（提供给 CryptoStream 类的构造函数）以及说明您希望在何种模式（
                //读、写等）下创建该类的枚举常数。创建 CryptoStream 对象 cs 后，现在
                //使用 CryptoStream 对象的 Write 方法将数据写入到内存数据流。这就是
                //进行实际加密的方法，加密每个数据块时，数据将被写入 MemoryStream 对
                //象。
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                //创建 MemoryStream 后，该代码将在 CryptoStream 对象上执行 
                //FlushFinalBlock 方法，以确保所有数据均被写入 MemoryStream 对象。该
                //过程将关闭 CryptoStream 对象。
                cs.FlushFinalBlock();
                cs.Close();
                //最后，该过程将内存数据流从字节数组转换回字符串，这样才可以在窗体上
                //的文本框内显示该字符串。可以使用 MemoryStream ToArray() 方法从数据
                //流中获取字节数组，然后调用 Encoding.UTF8.GetString() 方法，该方法接
                //受字节数组输入并使用 UTF8 编码方法将该字符串编码为可读内容。
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                //如果解密时使用的字符串不正确，将不能正常解码而引发异常，应该返回一
                //个空值给用户。
                return e.Message;
            }
        }

        /// <summary>
        /// 该方法用于从指定密钥的哈希创建指定长度的字节数组。
        /// </summary>
        /// <param name="pString">需要生成哈希值的字符串</param>
        /// <param name="pLength">需要生成的字节长度</param>
        /// <returns>哈希值的字节数组</returns>
        private static byte[] _TruncateHash(string pString, int pLength)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] byt = global::System.Text.Encoding.Unicode.GetBytes(pString);
                byte[] hash = sha1.ComputeHash(byt);
                //对数组进行维数下标的变更
                Array.Resize(ref hash, pLength);
                return hash;
            }
        }

        #endregion

        #region DES Encrypt and Decrypt

        /// <summary> 
        /// DES 加密字符串。
        /// </summary>
        /// <param name="pString">需要加密的字符串</param>
        /// <param name="pKey">密钥字符串</param>
        /// <param name="pIV">IV字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string DESEncryptString(string pString, string pKey, string pIV)
        {
            //它用于保存相应加密服务提供程序的引用。所有的对称算法类都是从这个基
            //类继承而来的。
            try
            {
                SymmetricAlgorithm mCSP = new DESCryptoServiceProvider();
                mCSP.Key = _TruncateHash(pKey, mCSP.KeySize / 8);
                mCSP.IV = _TruncateHash(pIV, mCSP.BlockSize / 8);
                return _EncryptString(mCSP, pString);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary> 
        /// DES 解密字符串
        /// </summary>
        /// <param name="pString">需要解密的字符串</param>
        /// <param name="pKey">密钥字符串</param>
        /// <param name="pIV">IV字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string DESDecryptString(string pString, string pKey, string pIV)
        {
            //它用于保存相应加密服务提供程序的引用。所有的对称算法类都是从这个基
            //类继承而来的。
            try
            {
                SymmetricAlgorithm mCSP = new DESCryptoServiceProvider();
                mCSP.Key = _TruncateHash(pKey, mCSP.KeySize / 8);
                mCSP.IV = _TruncateHash(pIV, mCSP.BlockSize / 8);
                return _DecryptString(mCSP, pString);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion

        #region TripleDES Encrypt and Decrypt

        /// <summary> 
        /// TripleDES 加密字符串。
        /// </summary>
        /// <param name="pString">需要加密的字符串</param>
        /// <param name="pKey">密钥字符串</param>
        /// <param name="pIV">IV字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string TripleDESEncryptString(string pString, string pKey, string pIV)
        {
            //它用于保存相应加密服务提供程序的引用。所有的对称算法类都是从这个基
            //类继承而来的。
            try
            {
                SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
                mCSP.Key = _TruncateHash(pKey, mCSP.KeySize / 8);
                mCSP.IV = _TruncateHash(pIV, mCSP.BlockSize / 8);
                return _EncryptString(mCSP, pString);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary> 
        /// TripleDES 解密字符串
        /// </summary>
        /// <param name="pString">需要解密的字符串</param>
        /// <param name="pKey">密钥字符串</param>
        /// <param name="pIV">IV字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string TripleDESDecryptString(string pString, string pKey, string pIV)
        {
            //它用于保存相应加密服务提供程序的引用。所有的对称算法类都是从这个基
            //类继承而来的。
            try
            {
                SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
                mCSP.Key = _TruncateHash(pKey, mCSP.KeySize / 8);
                mCSP.IV = _TruncateHash(pIV, mCSP.BlockSize / 8);
                return _DecryptString(mCSP, pString);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion
    }

    /// <summary>
    /// MD5字符串加密类，加密结果为固定长度字符串，该加密不能逆转。
    /// </summary>
    internal sealed class MD5
    {
        /// <summary>
        /// MD5 字符串散列加密，该加密不能逆转。
        /// </summary>
        /// <param name="pString">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptString(string pString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] byteValue;
            byte[] byteHash;
            byteValue = global::System.Text.Encoding.UTF8.GetBytes(pString);
            byteHash = md5.ComputeHash(byteValue);
            md5.Clear();
            return Convert.ToBase64String(byteHash);
        }
    }

    /// <summary>
    /// SHA1 字符串加密类，加密结果为固定长度字符串，该加密不能逆转。
    /// </summary>
    internal sealed class SHA1
    {
        /// <summary>
        /// 提供 SHA1 字符串散列加密，该加密不能逆转。
        /// </summary>
        /// <param name="pString">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptString(string pString)
        {
            SHA1CryptoServiceProvider sha1Csp = new SHA1CryptoServiceProvider();
            byte[] byteValue;
            byte[] byteHash;
            byteValue = global::System.Text.Encoding.UTF8.GetBytes(pString);
            byteHash = sha1Csp.ComputeHash(byteValue);
            sha1Csp.Clear();
            return Convert.ToBase64String(byteHash);
        }
    }
}
