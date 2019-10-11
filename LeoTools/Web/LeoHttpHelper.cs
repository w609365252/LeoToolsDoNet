using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LeoTools.Web
{
    public class LeoHttpHelper
    {
        public static string Get(string url)
        {
            HttpHelper httpHelper = new HttpHelper();
            return httpHelper.GetHtml(new HttpItem()
            {
                URL = url,
                Method = "GET"
            }).Html;
        }

        public static string Post(string url,string Params)
        {
            HttpHelper httpHelper = new HttpHelper();
            return httpHelper.GetHtml(new HttpItem()
            {
                URL = url,
                Method = "Post",
                PostEncoding=Encoding.UTF8,
                Postdata = Params
            }).Html;
        }

        public static byte[] PostGetByte(string url, string param)
        {
            string strURL = url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            string paraUrlCoded = param;
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();//返回图片数据流
            byte[] tt = StreamToBytes(s);//将数据流转为byte[]
            return tt;
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            List<byte> bytes = new List<byte>();
            int temp = stream.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = stream.ReadByte();
            }
            return bytes.ToArray();
        }

    }
}
