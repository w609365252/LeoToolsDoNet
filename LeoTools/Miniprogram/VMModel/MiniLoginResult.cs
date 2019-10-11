using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoTools.MiniprogramApiHelper
{
    /// <summary>
    /// https://api.weixin.qq.com/sns/jscode2session?appid=APPID&secret=SECRET&js_code=JSCODE&grant_type=authorization_code 登录返回的实体
    /// </summary>
    public class MiniLoginResult
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 会话密钥
        /// </summary>
        public string session_key { get; set; }
        /// <summary>
        /// 用户在开放平台的唯一标识符，在满足 UnionID 下发条件的情况下会返回，详见 UnionID 机制说明(https://developers.weixin.qq.com/miniprogram/dev/framework/open-ability/union-id.html)。
        /// </summary>
        public string unionid { get; set; }
        /// <summary>
        /// 错误码 0:请求成功,-1:系统繁忙，此时请开发者稍候再试,40029:code无效,45011:频率限制，每个用户每分钟100次
        /// </summary>
        public string errcode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }
}