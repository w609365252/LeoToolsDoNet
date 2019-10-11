using System.IO;
using System.Security.Cryptography;
using System.Text;
using static System.Convert;

namespace LeoTools.EncryptionDecryption
{
    /// <summary>
    /// 简单的加密解密
    /// </summary>
    
    public class Easy
    {
        private const string KEY_64 = "VavicApp";
        private const string IV_64 = "VavicApp";

        /// <summary>
        /// 简单分字符加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string EncodeByChar(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            var htext = string.Empty;
            for (int i = 0; i < str.Length; i++) htext += (char)(str[i] + 10 - 1 * 2);
            return htext;
        }

        /// <summary>
        /// 简单字符解密
        /// </summary>
        /// <param name="str">要解密的字符串</param>
        /// <returns></returns>
        public static string DecodeByChar(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            var dtext = string.Empty;
            for (int i = 0; i < str.Length; i++) dtext += (char)(str[i] - 10 + 1 * 2);
            return dtext;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns></returns>
        public static string Encode(string str)
        {
            var byKey = Encoding.ASCII.GetBytes(KEY_64);
            var byIV = Encoding.ASCII.GetBytes(IV_64);
            var cryptoProvider = new DESCryptoServiceProvider();
            var i = cryptoProvider.KeySize;
            var ms = new MemoryStream();
            var cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);
            var sw = new StreamWriter(cst);
            sw.Write(str);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            var htext = ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            sw.Close();
            cst.Close();
            ms.Close();
            return htext;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str">要解密的字符串</param>
        /// <returns>解密后的源字符串</returns>
        public static string Decode(string str)
        {
            var byKey = Encoding.ASCII.GetBytes(KEY_64);
            var byIV = Encoding.ASCII.GetBytes(IV_64);
            byte[] byEnc;
            try
            {
                byEnc = FromBase64String(str);
            }
            catch
            {
                return string.Empty;
            }
            var cryptoProvider = new DESCryptoServiceProvider();
            var ms = new MemoryStream(byEnc);
            var cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            var sr = new StreamReader(cst);
            var dtext = sr.ReadToEnd();
            sr.Close();
            cst.Close();
            ms.Close();
            return dtext;
        }
    }
}