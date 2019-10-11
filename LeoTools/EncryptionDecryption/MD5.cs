using System.Security.Cryptography;
using System.Text;

namespace LeoTools.EncryptionDecryption
{
    /// <summary>
    /// MD5加密, 不可逆
    /// </summary>
    
    public class MD5
    {
        /// <summary>
        /// 获得MD5加密字符串
        /// </summary>
        /// <param name="string"></param>
        /// <param name="isUpper">是否返回大写, 默认是</param>
        /// <returns>加密后的MD5字符串</returns>
        
        public static string GetMD5(string @string, bool isUpper = true)
        {
            byte[] data = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(@string));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            if (isUpper)
            {
                return sBuilder.ToString().ToUpper();
            }
            return sBuilder.ToString();
        }
    }
}