using System.IO;
using System.Security.Cryptography;
using System.Text;
using static System.Convert;

namespace LeoTools.EncryptionDecryption
{
    /// <summary>
    /// 自创类似MD5加密解密
    /// </summary>
    
    public class C_MD5
    {
        /// <summary> 
        ///  MD5加密
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        /// <param name="key">混淆的Key, 必须有, 否则异常, 默认为KLW</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5Encrypt(string text, string key = "Crp")
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            string sKey = "Crp";
            if (!string.IsNullOrWhiteSpace(key)) sKey = key.Trim();
            var des = DES.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
            var keyAndIV = Encoding.ASCII.GetBytes(MD5.GetMD5(sKey).Substring(0, 8));
            des.Key = keyAndIV;
            des.IV = keyAndIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            des.Dispose();
            cs.Dispose();
            ms.Dispose();
            return ret.ToString();
        }

        /// <summary> 
        ///  MD5解密
        /// </summary>
        /// <param name="text">要解密的字符串</param>
        /// <param name="key">混淆的Key, 必须有, 否则异常, 默认为KLW</param>
        /// <returns>解密后的字符串</returns>
        public static string MD5Decrypt(string text, string key = "Crp")
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            string sKey = "Crp";
            if (!string.IsNullOrWhiteSpace(key)) sKey = key.Trim();
            var des = DES.Create();
            int len;
            len = text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = ToInt32(text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            var keyAndIV = Encoding.ASCII.GetBytes(MD5.GetMD5(sKey).Substring(0, 8));
            des.Key = keyAndIV;
            des.IV = keyAndIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var result = Encoding.UTF8.GetString(ms.ToArray());
            des.Dispose();
            cs.Dispose();
            ms.Dispose();
            return result;
        }
    }
}