using LeoTools.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoTools.MiniprogramApiHelper
{
    public partial class MiniprogramApiHelper
    {
        public static string SendMsg(MiniProgramTemplate template)
        {
            if (string.IsNullOrEmpty(template.touser) || string.IsNullOrEmpty(template.form_id)) return "";
            string url = $"https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token={GetToken()}";
            string result=LeoHttpHelper.Post(url, JsonConvert.SerializeObject(template));
            return result;
        }
        
        public class MiniProgramTemplate
        {
            /// <summary>
            /// 接收者（用户）的 openid
            /// </summary>
            public string touser { get; set; }
            /// <summary>
            /// 所需下发的模板消息的id
            /// </summary>
            public string template_id { get; set; }
            /// <summary>
            /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）。该字段不填则模板无跳转。
            /// </summary>
            public string page { get; set; }
            /// <summary>
            /// 表单提交场景下，为 submit 事件带上的 formId；支付场景下，为本次支付的 prepay_id
            /// </summary>
            public string form_id { get; set; }
            /// <summary>
            /// 模板内容，不填则下发空模板。具体格式请参考示例。
            /// </summary>
            public Dictionary<string,object> data { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string emphasis_keyword { get; set; }
        }

    }
}