using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoTools.MiniprogramApiHelper
{
    /// <summary>
    /// 解密手机号返回的实体
    /// </summary>
    public class MiniAESDecryptResult
    {
        /// <summary>
        /// 用户绑定的手机号（国外手机号会有区号）
        /// </summary>
        public string phoneNumber { get; set; }
        /// <summary>
        /// 没有区号的手机号
        /// </summary>
        public string purePhoneNumber { get; set; }
        /// <summary>
        /// 区号
        /// </summary>
        public string countryCode { get; set; }
        public watermark watermark { get; set; }
    }

    public class watermark
    {
        public string appid { get; set; }
        public string timestamp { get; set; }
    }

}