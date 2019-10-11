using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static System.Convert;

namespace LeoTools.EncryptionDecryption
{
    /// <summary>
    /// TripleDES加密解密
    /// </summary>
    
    public class TripleDES
    {
        /// <summary>
        /// TripleDES加密的密钥
        /// </summary>
        private readonly static byte[] TripleKey = { 42, 16, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 20, 155, 112, 2, 94, 11, 204, 119, 35, 184, 197 };

        /// <summary>
        /// TripleDES加密的偏移量
        /// </summary>
        private readonly static byte[] TripleIV = { 55, 103, 246, 79, 36, 99, 167, 3 };

        /// <summary>
        /// TripleDES加密
        /// </summary>
        /// <param name="strSource">要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string TripleDESEncrypting(string strSource)
        {
            try
            {
                byte[] bytIn = Encoding.Default.GetBytes(strSource);
                TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider() { IV = TripleIV, Key = TripleKey };
                ICryptoTransform encrypto = TripleDES.CreateEncryptor();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                byte[] bytOut = ms.ToArray();
                return ToBase64String(bytOut);
            }
            catch (Exception ex)
            {
                throw new Exception("加密时候出现错误!错误提示:\n" + ex.Message);
            }
        }

        /// <summary>
        /// TripleDES解密
        /// </summary>
        /// <param name="Source">要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string TripleDESDecrypting(string Source)
        {
            try
            {
                byte[] bytIn = FromBase64String(Source);
                TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider() { IV = TripleIV, Key = TripleKey };
                ICryptoTransform encrypto = TripleDES.CreateDecryptor();
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader strd = new StreamReader(cs, Encoding.Default);
                return strd.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("解密时候出现错误!错误提示:\n" + ex.Message);
            }
        }
    }
}