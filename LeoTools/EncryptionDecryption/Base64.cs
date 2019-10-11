using System;
using System.Text;

namespace LeoTools.EncryptionDecryption
{
    /// <summary>
    /// Base64加密解密
    /// </summary>
    
    public class Base64
    {
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="text">加密的字符串</param>
        /// <returns></returns>
        public static string Base64Code(string text)
        {
            byte[] encData_byte = new byte[text.Length];
            encData_byte = Encoding.UTF8.GetBytes(text);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="text">解密的字符串</param>
        /// <returns></returns>
        public static string Base64Decode(string text)
        {
            try
            {
                UTF8Encoding encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(text);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Base64解密错误" + e.Message);
            }
        }
    }
}