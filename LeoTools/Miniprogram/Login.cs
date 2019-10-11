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

namespace LeoTools.MiniprogramApiHelper
{
    public partial class MiniprogramApiHelper
    {

        public static MiniLoginResult Login(string code)
        {
            string url = $"https://api.weixin.qq.com/sns/jscode2session?appid={AppID}&secret={AppSercet}&js_code={code}&grant_type=authorization_code";
            string result = LeoHttpHelper.Get(url);
            MiniLoginResult loginResult = JsonConvert.DeserializeObject<MiniLoginResult>(result);
            return loginResult;
        }

        public static MiniAESDecryptResult AES_decrypt(string encryptedDataStr, string key, string iv)
        {
            MiniAESDecryptResult miniAESDecryptResult = null;
            try
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
                miniAESDecryptResult = JsonConvert.DeserializeObject<MiniAESDecryptResult>(result);
            }
            catch (Exception ex)
            {

            }

            miniAESDecryptResult = miniAESDecryptResult ?? new MiniAESDecryptResult();
            return miniAESDecryptResult;
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

    }
}