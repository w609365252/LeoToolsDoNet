using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Postdata= Params
            }).Html;
        }

        public static byte[] PostGetByte(string url, string Params)
        {
            HttpHelper httpHelper = new HttpHelper();
           var hm= httpHelper.GetHtml(new HttpItem()
            {
                URL = url,
                Method = "Post",
                Postdata = Params,
                ContentType = "application/x-www-form-urlencoded;",
                PostDataType = CsharpHttpHelper.Enum.PostDataType.Byte
            });
            return hm.ResultByte;
        }
    }
}
