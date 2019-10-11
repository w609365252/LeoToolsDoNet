using LeoTools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoTools.SMS
{
    public class SendSMS
    {
        static string cacheKey = "SendSMSUserMobile_";
        /// <summary>
        /// code不填默认随机6位数字验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static SMSCodeResult SendSMSCode(string phone,string code="",bool isSetCache=true)
        {
            if (!phone.IsPhone())
            {
                return new SMSCodeResult()
                {
                    status = -2,
                    code = code,
                    errorMsg = "手机号错误"
                };
            }

            string errorMsg;
            if (string.IsNullOrEmpty(code))
            {
                code = new Random().Next(100000, 999999).ToString();
            }
            bool flag = AliSMS.GetSMSCode(phone, code, out errorMsg);
            if (flag && isSetCache) SetPhoneCodeCache(phone, code);
            return new SMSCodeResult()
            {
                status = flag ? 0 : -1,
                code = code,
                errorMsg = errorMsg
            };
        }

        public static bool SetPhoneCodeCache(string phone, string code)
        {
            return LeoTools.Cache.CacheFactory.SetCache(cacheKey + phone, code);
        }

        /// <summary>
        /// 0.发送验证码成功 -1验证码无效 -2错误的手机号
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static int ValidatePhoneCode(string phone, string code)
        {
            if (string.IsNullOrEmpty(code)) return -1;
            string c = LeoTools.Cache.CacheFactory.GetCache<string>(cacheKey + phone);
            if (code != c) return -1;
            return 0;
        }

        public static int ClearCode(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return -1;
            LeoTools.Cache.CacheFactory.RemoveCache(cacheKey + phone);
            return 0;
        }

        public class SMSCodeResult
        {
            /// <summary>
            /// 0发送成功 -1.发送失败
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 验证码
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errorMsg { get; set; }
        }

    }
}
