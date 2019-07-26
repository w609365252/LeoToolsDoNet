using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using LeoTools.Web;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using LeoTools.Extension;
using System.Drawing;

namespace Leo.MiniprogramApiHelper
{
    public class MiniprogramApiHelper
    {
        private static string AppID = "wx35f9df740392b2ce";
        private static string AppSercet = "590ee6dafc0f3257524737c324fdc4d2";

        public static string Login(string code)
        {
            string url = $"https://api.weixin.qq.com/sns/jscode2session?appid={AppID}&secret={AppSercet}&js_code={code}&grant_type=authorization_code";

            return LeoHttpHelper.Get(url);
        }

        public static string AES_decrypt(string encryptedDataStr, string key, string iv)
        {
            RijndaelManaged rijalg = new RijndaelManaged();
            //-----------------    
            //设置 cipher 格式 AES-128-CBC    

            rijalg.KeySize = 128;

            rijalg.Padding = PaddingMode.PKCS7;
            rijalg.Mode = CipherMode.CBC;

            rijalg.Key = Convert.FromBase64String(key);
            rijalg.IV = Convert.FromBase64String(iv);


            byte[] encryptedData = Convert.FromBase64String(encryptedDataStr);
            //解密    
            ICryptoTransform decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);

            string result;

            using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        result = srDecrypt.ReadToEnd();
                    }
                }
            }
            return result;
        }

        public static Image CreateShareCode(string str,string page)
        {
            string token = GetToken();
            var pam = new {
                scene=str,
                page=page,
                width=430
            };
            string url = $"https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token={token}";
            byte[] bytes = LeoHttpHelper.PostGetByte(url, pam.ObjectToString());
            Stream stream = new MemoryStream(bytes);
            return Image.FromStream(stream);
        }

        static string GetToken()
        {
            string str = LeoHttpHelper.Get($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={AppID}&secret={AppSercet}");
            var obj = str.ToDynamic();
            return obj.access_token;
        }

    }
}