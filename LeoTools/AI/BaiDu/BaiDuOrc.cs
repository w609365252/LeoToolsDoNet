using CsharpHttpHelper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LeoTools.AI.BaiDu
{
    public class BaiDuOrc
    {
        /// <summary>
        /// 百度
        /// </summary>
        static string API_KEY = "s2McGGfxl1L80cKmWo7GDaSO";
        static string SECRET_KEY = "Mnqxk0cNdhuIskIPC59QT1yx11sbNm5P";

        /// <summary>
        /// 百度识别（限次数）
        /// </summary>
        /// <returns></returns>
        public static string BaiduTextOrc(Image image)
        {
            try
            {
                var location = "";
                HttpHelper httper = new HttpHelper();
                HttpItem item2 = new HttpItem()
                {
                    URL = "https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id=" + API_KEY + "&client_secret=" + SECRET_KEY + "&",
                };
                var res1 = httper.GetHtml(item2);
                var jobject = JObject.Parse(res1.Html);
                var jtoken = JToken.Parse(jobject.ToString());
                var token1 = jtoken["access_token"].ToString();
                System.Drawing.Imaging.ImageFormat format = image.RawFormat;
                MemoryStream mqs = new MemoryStream();

                image.Save(mqs, format);
                var d = mqs.ToArray();
                var strbaser64 = Convert.ToBase64String(d);
                string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/general?access_token=" + token1;
                Encoding encoding = Encoding.Default;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
                request.Method = "post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.KeepAlive = true;
                String str = "image=" + HttpUtility.UrlEncode(strbaser64);
                byte[] buffer = encoding.GetBytes(str);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                string result = reader.ReadToEnd();
                var job = JObject.Parse(result.ToString());
                if (!result.ToString().Contains("limit"))
                {
                    var words_result = job["words_result"];
                    var jot = JToken.Parse(words_result.ToString());
                    location = jot[0]["words"].ToString();
                }
                return location;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 百度识别（限次数）
        /// path
        /// </summary>
        /// <returns></returns>
        public static string BaiduTextOrc(string path)
        {
            try
            {
                var location = "";
                HttpHelper httper = new HttpHelper();
                HttpItem item2 = new HttpItem()
                {
                    URL = "https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id=" + API_KEY + "&client_secret=" + SECRET_KEY + "&",
                };
                var res1 = httper.GetHtml(item2);
                var jobject = JObject.Parse(res1.Html);
                var jtoken = JToken.Parse(jobject.ToString());
                var token1 = jtoken["access_token"].ToString();
                MemoryStream mqs = new MemoryStream();

                var d = mqs.ToArray();
                var strbaser64 = Convert.ToBase64String(d);
                string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/general?access_token=" + token1;
                Encoding encoding = Encoding.Default;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
                request.Method = "post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.KeepAlive = true;
                String str = "url=" + path;
                byte[] buffer = encoding.GetBytes(str);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                string result = reader.ReadToEnd();
                var job = JObject.Parse(result.ToString());
                if (!result.ToString().Contains("limit"))
                {
                    var words_result = job["words_result"];
                    var jot = JToken.Parse(words_result.ToString());
                    location = jot[0]["words"].ToString();
                }
                return location;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
