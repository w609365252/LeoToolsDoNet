using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LeoTools.AI.FeiFeiDaMa
{
    public class FateadmRsp
    {
        //操作返回码，0为正常，其他为异常情况，异常原因保存在err_msg中
        public int ret_code;
        // 保存异常原因
        public string err_msg;
        // 如果为识别操作，此处保存识别结果
        // ret_code 不等于0时，pred_reslt 为空串
        public string pred_reslt;
        // 订单号
        public string order_id;
        // 余额查询时，此处得到用户的余额信息
        public double cust_val;
    }

    /// <summary>
    /// 识别验证码
    /// </summary>
    internal class FeiFeiFactory
    {
        public static FateadmRsp AiValite(byte[] bytes)
        {
            FeiFeiApi api = new FeiFeiApi();
            //string app_id = "114656";
            //string app_key = "vRACSWxHBQaafiobF7+u6BHrOuQNHj1I";
            //// pd账号信息请在用户中心页获取
            //string pd_id = "114656";
            //string pd_key = "vRACSWxHBQaafiobF7+u6BHrOuQNHj1I";

            //云创账号
            string app_id = "114717";
            string app_key = "1gC6+kx2aJzDt1F88P3x2RxhwMn4hLO7";
            //云创帐号
            string pd_id = "104296";
            string pd_key = "C/yUEWUxBg8pWWXbE6OlmC9GkbdQQoiD";

            //客户帐号
            //string app_id = "312769";
            //string app_key = "PDng3sbUU0cDpFeuE2DmtFrlFWDPjqSr";
            //string pd_id = "112769";
            //string pd_key = "cPdjMDQCCOjwBYB3iQ6CHPGUkuCLyUXy";

            // api对象使用之前，需要进行初始化操作
            api.Init(app_id, app_key, pd_id, pd_key);
            FateadmRsp rsp;

            double balance = api.QueryBalanceExtend(); // 直接返回用户余额
            string pred_type = "10400";
            rsp = api.PredictFromFile(pred_type, bytes);
            return rsp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type">0服务器路径 1本地路径</param>
        /// <returns></returns>
        public static FateadmRsp AiValite(string url, CommonEnum.PathEnum @enum= CommonEnum.PathEnum.Remote)
        {
            byte[] bytes;
            if (CommonEnum.PathEnum.Remote == @enum)
            {
                bytes = Common.LeoUtils.ImageToBytes(url);
            }
            else
            {
                bytes = Common.LeoUtils.ImageToBytes(url, CommonEnum.PathEnum.Local);
            }
            return AiValite(bytes);
        }

        public static FateadmRsp AiValite(Image image)
        {
            byte[] bytes = Common.LeoUtils.ImageToBytes(image);
            return AiValite(bytes);
        }
    }

    internal class FeiFeiApi
    { 
        protected  string app_id = "";
        protected  string app_key = "";
        protected  string usr_id = "";
        protected  string usr_key = "";
        protected  bool is_init = false;
        private static string URL = "http://pred.fateadm.com";

        internal class HttpExtraInfo
        {
            public double cust_val;
            public string result;
        }
        internal class HttpRspData
        {
            public string RetCode;
            public string ErrMsg;
            public string RequestId;
            public string RspData;
            public HttpExtraInfo einfo;
        }

        internal class JsonPaserWeb
        {
            // Object->Json  
            public static string Serialize<T>(T obj)
            {
                return JsonConvert.SerializeObject(obj);
            }

            // Json->Object  
            public static T Deserialize<T>(string json)
            {

                return JsonConvert.DeserializeObject<T>(json);
            }
        }


        public void Init(string aid, string akey, string uid, string ukey)
        {
            is_init = true;
            app_id = aid;
            app_key = akey;
            usr_id = uid;
            usr_key = ukey;
        }
        public bool CheckIsInit(FateadmRsp rsp)
        {
            if (!is_init)
            {
                rsp.ret_code = -1;
                rsp.err_msg = "ERROR: 任何接口调用之前，需要先调用Init进行初始化";
            }
            return is_init;
        }

        /**
         * 查询余额
         * 参数：无
         * 返回值：
         *      rsp.ret_code：正常返回0
         *      rsp.err_msg：异常时返回异常详情
         *      rsp.cust_val：用户余额
         */
        public FateadmRsp QueryBalance()
        {
            FateadmRsp rsp = new FateadmRsp();
            if( !CheckIsInit(rsp)){
                return rsp;
            }
           QueryBalc(rsp, usr_id, usr_key);
            return rsp;
        }

        /***
         * 查询余额：直接返回余额结果
         * 参数：无
         * 返回值：用户余额:double
         */
        public double QueryBalanceExtend()
        {
            FateadmRsp rsp = QueryBalance();
            return rsp.cust_val;
        }

        /**
         * 验证码识别
         * 参数：pred_type：识别类型, img_data：图片数据
         * 返回值：
         *      rsp.ret_code：正常返回0
         *      rsp.err_msg：异常时返回异常详情
         *      rsp.pred_reslt：识别结果
         *      rsp.order_id：唯一的订单号
         */
        public FateadmRsp Predict(string pred_type, byte[] img_data)
        {
            FateadmRsp rsp = new FateadmRsp();
            if (!CheckIsInit(rsp))
            {
                return rsp;
            }
            Predict(rsp, app_id, app_key, usr_id, usr_key, pred_type, img_data);
            return rsp;
        }

        /***
         * 验证码识别：直接返回识别结果
         * 参数：pred_type：识别类型, img_data：图片数据
         * 返回值： 识别结果:string
         */
        public string PredictExtend(string pred_type, byte[] img_data)
        {
            FateadmRsp rsp = Predict(pred_type, img_data);
            return rsp.pred_reslt;
        }

        /**
         * 文件形式进行验证码识别
         * 参数：pred_type：识别类型, file_name：文件名
         * 返回值：
         *      rsp.ret_code：正常返回0
         *      rsp.err_msg：异常时返回异常详情
         *      rsp.pred_reslt：识别结果
         *      rsp.order_id：唯一的订单号    
         */
        public FateadmRsp PredictFromFile(string pred_type, string file_name)
        {
            FateadmRsp rsp = new FateadmRsp();
            if (!CheckIsInit(rsp))
            {
                return rsp;
            }
            PredictFromFile(rsp, app_id, app_key, usr_id, usr_key, pred_type,file_name);
            return rsp;
        }

        public FateadmRsp PredictFromFile(string pred_type, byte[] bytes)
        {
            FateadmRsp rsp = new FateadmRsp();
            if (!CheckIsInit(rsp))
            {
                return rsp;
            }
           PredictFromFile(rsp, app_id, app_key, usr_id, usr_key, pred_type, bytes);
            return rsp;
        }

        /***
         * 文件形式进行验证码识别：直接返回识别结果
         * 参数：pred_type：识别类型, file_name：文件名
         * 返回值： 识别结果:string
         */
        public string PredictFromFileExtend(string pred_type, string file_name)
        {
            FateadmRsp rsp = PredictFromFile(pred_type, file_name);
            return rsp.pred_reslt;
        }

        /**
         * 识别失败，进行退款请求
         * 参数：order_id：需要退款的订单号
         * 返回值：
         *      rsp.ret_code：正常返回0
         *      rsp.err_msg：异常时返回异常详情
         */
        public FateadmRsp Justice(string order_id)
        {
            FateadmRsp rsp = new FateadmRsp();
            if (!CheckIsInit(rsp))
            {
                return rsp;
            }
            Justice(rsp, usr_id, usr_key, order_id);
            return rsp;
        }

        /***
         * 调用退款：直接返回是否成功
         * 参数：order_id：需要退款的订单号
         * 返回值： 退款成功时返回数字 0
         */
        public int JusticeExtend(string order_id)
        {
            FateadmRsp rsp = Justice(order_id);
            return rsp.ret_code;
        }

        /**
         * 充值接口
         * 参数：cardid：充值卡号  cardkey：充值卡签名串
         * 返回值：
         *      rsp.ret_code：正常返回0
         *      rsp.err_msg：异常时返回异常详情
         */
        public FateadmRsp Charge(string cardid, string cardkey)
        {
            FateadmRsp rsp = new FateadmRsp();
            if (!CheckIsInit(rsp))
            {
                return rsp;
            }
            Charge(rsp, usr_id, usr_key, cardid, cardkey);
            return rsp;
        }

        /***
        * 充值接口：直接返回是否成功
        * 参数：cardid：充值卡号  cardkey：充值卡签名串
        * 返回值： 充值成功时返回数字 0
        */
        public int ChargeExtend(string cardid, string cardkey)
        {
            FateadmRsp rsp = Charge(cardid,cardid);
            return rsp.ret_code;
        }


        public void PredictFromFile(FateadmRsp rsp, string app_id, string app_key, string usr_id, string usr_key, string pred_type, string file_name)
        {
            byte[] img_data;
            try
            {
                FileStream fs = new FileStream(file_name, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                img_data = br.ReadBytes((int)fs.Length);
            }
            catch (Exception ex)
            {
                rsp.ret_code = -1;
                rsp.err_msg = "文件读取失败，请检查文件路径, err: " + ex.ToString();
                return;
            }
            Predict(rsp, app_id, app_key, usr_id, usr_key, pred_type, img_data);
        }

        public void PredictFromFile(FateadmRsp rsp, string app_id, string app_key, string usr_id, string usr_key, string pred_type, byte[] img_data)
        {
            Predict(rsp, app_id, app_key, usr_id, usr_key, pred_type, img_data);
        }

        public void Predict(FateadmRsp rsp, string app_id, string app_key, string usr_id, string usr_key, string pred_type, byte[] img_data)
        {
            string cur_tm = GetCurrentTimeUnix();
            string sign = CalcSign(usr_id, usr_key, cur_tm);
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("user_id", usr_id);
            param.Add("timestamp", cur_tm);
            param.Add("sign", sign);
            param.Add("up_type", "mt");
            if (!string.IsNullOrEmpty(app_id))
            {
                string asign = CalcSign(app_id, app_key, cur_tm);
                param.Add("appid", app_id);
                param.Add("asign", asign);
            }
            param.Add("predict_type", pred_type);
            string url = URL + "/api/capreg";
            HttpRspData jrsp = null;
            jrsp = PostMForm(url, param, img_data);
            rsp.ret_code = int.Parse(jrsp.RetCode);
            rsp.err_msg = jrsp.ErrMsg;
            rsp.order_id = jrsp.RequestId;
            if (rsp.ret_code == 0)
            {
                rsp.pred_reslt = jrsp.einfo.result;
            }
        }



        internal static string Md5(string src)
        {
            string str = "";
            byte[] data = Encoding.GetEncoding("utf-8").GetBytes(src);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(data);
            for (int i = 0; i < bytes.Length; i++)
            {
                str += bytes[i].ToString("x2");
            }
            return str;
        }
        internal static string CalcSign(string id, string key, string tm)
        {
            string chk = Md5(tm + key);
            string sum = Md5(id + tm + chk);
            //Console.WriteLine("calc sign, id: {0} key: {1} tm: {2} chk: {3} sum: {4}", id, key, tm, chk, sum);
            return sum;
        }
        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>   
        internal static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }
        //public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int timeout, string userAgent, CookieCollection cookies)
        internal static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, Encoding charset)
        {
            HttpWebRequest request = null;
            //HTTPSQ请求  
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            request = WebRequest.Create(url) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            //如果需要POST数据     
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = charset.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }
        internal static HttpRspData HttpPost(string url, IDictionary<string, string> param)
        {
            //string url = "";
            Encoding charset = Encoding.GetEncoding("utf-8");
            //IDictionary<string, string> param = new Dictionary<string, string>();
            // param.Add("usrid", "10000");
            HttpWebResponse resp = CreatePostHttpResponse(url, param, charset);
            Stream stream = resp.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            //Console.WriteLine(html);
            HttpRspData data = JsonPaserWeb.Deserialize<HttpRspData>(html);
            if (!string.IsNullOrEmpty(data.RspData))
            {
                // 附带附加信息
                HttpExtraInfo einfo = JsonPaserWeb.Deserialize<HttpExtraInfo>(data.RspData);
                data.einfo = einfo;
            }
            return data;
        }
        static string GetCurrentTimeUnix()
        {
            TimeSpan cha = (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)));
            long t = (long)cha.TotalSeconds;
            return t.ToString();
        }
        internal static void QueryBalc(FateadmRsp rsp, string usr_id, string usr_key)
        {
            string cur_tm = GetCurrentTimeUnix();
            string sign = CalcSign(usr_id, usr_key, cur_tm);
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("user_id", usr_id);
            param.Add("timestamp", cur_tm);
            param.Add("sign", sign);
            string url = URL + "/api/custval";
            HttpRspData jrsp = HttpPost(url, param);
            rsp.ret_code = int.Parse(jrsp.RetCode);
            rsp.err_msg = jrsp.ErrMsg;
            if (rsp.ret_code == 0)
            {
                rsp.cust_val = jrsp.einfo.cust_val;
            }
        }
        internal static void Justice(FateadmRsp rsp, string usr_id, string usr_key, string order_id)
        {
            string cur_tm = GetCurrentTimeUnix();
            string sign = CalcSign(usr_id, usr_key, cur_tm);
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("user_id", usr_id);
            param.Add("timestamp", cur_tm);
            param.Add("sign", sign);
            param.Add("request_id", order_id);
            string url = URL + "/api/capjust";
            HttpRspData jrsp = HttpPost(url, param);
            rsp.ret_code = int.Parse(jrsp.RetCode);
            rsp.err_msg = jrsp.ErrMsg;
        }
        internal static void Charge(FateadmRsp rsp, string usr_id, string usr_key, string cardid, string cardkey)
        {
            string cur_tm = GetCurrentTimeUnix();
            string sign = CalcSign(usr_id, usr_key, cur_tm);
            string csign = Md5(usr_key + cur_tm + cardid + cardkey);
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("user_id", usr_id);
            param.Add("timestamp", cur_tm);
            param.Add("sign", sign);
            param.Add("cardid", cardid);
            param.Add("csign", csign);
            string url = URL + "/api/charge";
            HttpRspData jrsp = HttpPost(url, param);
            rsp.ret_code = int.Parse(jrsp.RetCode);
            rsp.err_msg = jrsp.ErrMsg;
            rsp.order_id = jrsp.RequestId;
        }


        static byte[] GetUrlImage(string url)
        {
            byte[] img_data = null;
            try
            {
                WebClient client = new WebClient();
                img_data = client.DownloadData(url);
            }
            catch (Exception e)
            {
            }
            return img_data;
        }

        static HttpRspData PostMForm(string url, IDictionary<string, string> param, byte[] img_data)
        {
            string boundary = "-----" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.UserAgent = "RK_C# 1.2";
            wr.Method = "POST";
            Stream rs = null;
            try
            {
                rs = wr.GetRequestStream();
            }
            catch
            {
                Console.WriteLine("The Web is Disconnected!");
                return null;
            }
            string html = null;
            string item_string = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in param.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string form_item = string.Format(item_string, key, param[key]);
                //Console.WriteLine(string.Format("key:{0} val:{1}",key,param[key]));
                byte[] form_item_bytes = System.Text.Encoding.UTF8.GetBytes(form_item);
                rs.Write(form_item_bytes, 0, form_item.Length);

            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);
            string file_srtring = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n"
                , "img_data", "image", "image/jpg");
            rs.Write(System.Text.Encoding.UTF8.GetBytes(file_srtring), 0, file_srtring.Length);
            rs.Write(img_data, 0, img_data.Length);
            byte[] trail = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--");
            rs.Write(trail, 0, trail.Length);
            rs.Close();
            WebResponse wresp = null;
            HttpRspData data = null;
            try
            {
                wresp = wr.GetResponse();

                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                html = reader2.ReadToEnd();
            }
            catch { }
            finally
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                    data = JsonPaserWeb.Deserialize<HttpRspData>(html);
                    if (!string.IsNullOrEmpty(data.RspData))
                    {
                        // 附带附加信息
                        HttpExtraInfo einfo = JsonPaserWeb.Deserialize<HttpExtraInfo>(data.RspData);
                        data.einfo = einfo;
                    }
                }
                wr.Abort();
                wr = null;
            }

            return data;
        }

    }
}
