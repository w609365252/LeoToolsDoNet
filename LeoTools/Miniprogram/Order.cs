using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LeoTools.Web;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using LeoTools.Extension;
using CsharpHttpHelper;
using System.Text;
using LeoTools.Common;
using System.Xml.Serialization;
using System.Configuration;

namespace LeoTools.MiniprogramApiHelper
{
    public partial class MiniprogramApiHelper
    {
        public static PayOrderResult CreateOrder(WeChatPayOrder order)
        {
    
            HttpHelper httpHelper = new HttpHelper();
            SetSign(order);
            string data = CreateXmlParam(JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(order)));
            HttpResult result = httpHelper.GetHtml(new HttpItem()
            {
                URL = "https://api.mch.weixin.qq.com/pay/unifiedorder",
                Postdata = data,
                Method = "Post",
                PostEncoding = Encoding.UTF8,
                Encoding=Encoding.UTF8,
                ContentType = "text/xml;"
            });
            PayOrderResult payOrderResult = XmlHelper.DeserializeToObject<PayOrderResult>(result.Html);
            payOrderResult.timeStamp = GetTimeStamp(DateTime.Now).ToString();
            SetPaySign(payOrderResult);
            return payOrderResult;
        }


        /// <summary>
        /// MD5(appId=wxd678efh567hg6787&
        /// nonceStr=5K8264ILTKCH16CQ2502SI8ZNMTM67VS&
        /// package=prepay_id=wx2017033010242291fcfe0db70013231072&
        /// signType=MD5&
        /// timeStamp=1490840662&
        /// key=qazwsxedcrfvtgbyhnujmikolp111111) 
        /// </summary>
        /// <param name="o"></param>
        static  void SetPaySign(PayOrderResult o)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("appId",AppID);
            d.Add("nonceStr", o.nonce_str);
            d.Add("package", "prepay_id=" + o.prepay_id);
            d.Add("signType", "MD5");
            d.Add("timeStamp", o.timeStamp);
            var vDic = (from objDic in d orderby objDic.Key ascending select objDic);
            string stringA = "";
            foreach (var item in vDic)
            {
                string val = item.Value;
                string key = item.Key;
                if (!string.IsNullOrEmpty(val))
                {
                    stringA += $"{key}={val}&";
                }
            }
            stringA = stringA.Trim('&') + "&key=" + SHSercret;
            o.paySign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(stringA, "MD5").ToUpper();
        }

        //生成10位时间戳，微信要10位
        static int GetTimeStamp(DateTime dt)
        {
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            int timeStamp = Convert.ToInt32((dt - dateStart).TotalSeconds);
            return timeStamp;
        }

        /// <summary>
        /// 集合转换XML数据 (拼接成XML请求数据)
        /// </summary>
        /// <param name="strParam">参数</param>
        /// <returns></returns>
        static string CreateXmlParam(Dictionary<string, string> strParam)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (KeyValuePair<string, string> k in strParam)
            {
                if (k.Key == "attach" || k.Key == "body" || k.Key == "sign")
                {
                    //if (k.Key == "body")
                    //{
                    //    sb.Append("<" + k.Key + "><![CDATA[" +333 + "]]></" + k.Key + ">");
                    //}
                    //else
                    //{
                        sb.Append("<" + k.Key + "><![CDATA[" + k.Value + "]]></" + k.Key + ">");
                    //}
                }
                else
                {
                    sb.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取签名
        /// 第一步：对参数按照key=value的格式，并按照参数名ASCII字典序排序如下：
        ///stringA="appid=wxd930ea5d5a258f4f&body=test&device_info=1000&mch_id=10000100&nonce_str=ibuaiVcKdpRxkhJA";
        ///第二步：拼接API密钥：
        ///stringSignTemp=stringA+"&key=192006250b4c09247ec02edce69f6a2d" //注：key为商户平台设置的密钥key
        ///sign=MD5(stringSignTemp).toUpperCase()="9A0A8659F005D6984697E2CA0A9CF3B7" //注：MD5签名方式
        ///sign=hash_hmac("sha256", stringSignTemp, key).toUpperCase()="6A9AE1657590FD6257D693A078E1C3E4BB6BA4DC30B23E0EE2496E54170DACD6" //注：HMAC-SHA256签名方式
        /// </summary>
        /// <returns></returns>
        static void SetSign(WeChatPayOrder order)
        {
            var d = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(order));

            var vDic = (from objDic in d orderby objDic.Key ascending select objDic);
            string stringA = "";
            foreach (var item in vDic)
            {
                string val = item.Value;
                string key = item.Key;
                if (!string.IsNullOrEmpty(val))
                {
                    stringA += $"{key}={val}&";
                }
            }
            stringA = stringA.Trim('&') + "&key=" + SHSercret;
            order.sign= System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(stringA, "MD5").ToUpper();
        }

        public static WeChatPayOrderCallBack GetPayCallBack(string data)
        {
            return XmlHelper.DeserializeToObject<WeChatPayOrderCallBack>(data);
        }

        public static WeChatPayOrder GetWeChatPayOrder()
        {
            WeChatPayOrder weChatPayOrder = new WeChatPayOrder();
            weChatPayOrder.nonce_str = Guid.NewGuid().ToString("N");
            weChatPayOrder.notify_url = WeChatPayCallBack;
            return weChatPayOrder;
        }

        public class WeChatPayOrder
        {
            public string appid { get { return AppID; } }
            public string mch_id { get { return MCHID; } }
            /// <summary>
            /// 回调地址
            /// </summary>
            public string notify_url { get; set; } = "";
            /// <summary>
            /// 商家订单号（数据库订单号）
            /// </summary>
            public string out_trade_no { get; set; }
            /// <summary>
            /// https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=4_3
            /// </summary>
            public string nonce_str { get; set; }
            public string openid { get; set; }
            public string sign { get; set; }
            public string spbill_create_ip { get { return "129.204.181.59"; } }
            public string trade_type { get; set; } = "JSAPI";
            /// <summary>
            /// 订单总金额，单位为分
            /// </summary>
            public string total_fee { get; set; }
            public string sign_type { get; set; } = "MD5";
            /// <summary>
            /// 符合ISO 4217标准的三位字母代码，默认人民币：CNY，详细列表请参见
            /// </summary>
            public string fee_type { get; set; } = "CNY";
            /// <summary>
            /// 自定义参数，可以为终端设备号(门店号或收银设备ID)，PC网页或公众号内支付可以传"WEB"
            /// </summary>
            public string device_info { get; set; } = "013467007045764";
            /// <summary>
            /// https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=4_2
            /// </summary>
            public string body { get; set; } = "测试";
            public string attach { get; set; } = "";
        }

        [XmlRoot("xml")]
        public class PayOrderResult
        {
            public string return_code { get; set; }
            public string return_msg { get; set; }
            public string appid { get; set; } = "";
            public string mch_id { get; set; } = "";
            public string nonce_str { get; set; } = "";
            public string openid { get; set; } = "";
            public string sign { get; set; } = "";
            public string result_code { get; set; } = "";
            public string prepay_id { get; set; } = "";
            public string trade_type { get; set; } = "";
            public string timeStamp { get; set; } = "";
            //小程序唤起支付密钥
            public string paySign { get; set; } = "";

        }

        [XmlRoot("xml")]
        public class WeChatPayOrderCallBack
        {
            public string appid { get; set; }
            public string bank_type { get; set; }
            public string device_info { get; set; }
            public string cash_fee { get; set; }
            public string fee_type { get; set; }
            public string is_subscribe { get; set; }
            public string mch_id { get; set; }
            public string nonce_str { get; set; }
            public string openid { get; set; }
            //回调的订单
            public string out_trade_no { get; set; }
            //SUCCESS成功
            public string result_code { get; set; }
            //SUCCESS成功
            public string return_code { get; set; }
            //签名
            public string sign { get; set; }
            //20190907174643
            public string time_end { get; set; }
            public string total_fee { get; set; }
            public string trade_type { get; set; }
            public string transaction_id { get; set; }
            public bool IsSuccess
            {
                get { return string.Equals("success", result_code, StringComparison.OrdinalIgnoreCase); }
            }
        }

    }
}