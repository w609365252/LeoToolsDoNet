using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LeoTools.Common
{
    public class LeoUtils
    {
        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddMilliseconds(d);
            return time;
        }

        /// <summary>
        /// 秒为单位
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static System.DateTime ConvertIntDateTime1(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddMilliseconds(d*1000);
            return time;
        }
        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static string ConvertDateTimeInt(System.DateTime? time)
        {
            if (time == null) return "";
            //double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //intResult = (time- startTime).TotalMilliseconds;
            long t = (time.Value.Ticks - startTime.Ticks) / 10000;            //除10000调整为13位
            return t == 0 ? "" : t.ToString();
        }

        /// <summary>
        /// 秒为单位
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ConvertDateTimeInt1(System.DateTime? time)
        {
            if (time == null) return "";
            //double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //intResult = (time- startTime).TotalMilliseconds;
            long t = (time.Value.Ticks - startTime.Ticks) / 10000000;            //除10000调整为13位
            return t == 0 ? "" : t.ToString();
        }

        public static string GetRandomStr(int num = 6)
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";


            Random randrom = new Random((int)DateTime.Now.Ticks);

            string str = "";
            for (int i = 0; i < num; i++)
            {
                str += chars[randrom.Next(chars.Length)];
            }
            return str;
        }

        public static bool IsMobilePhone(string str_handset)
        {
            return Regex.IsMatch(str_handset, "^(0\\d{2,3}-?\\d{7,8}(-\\d{3,5}){0,1})|(((13[0-9])|(15([0-3]|[5-9]))|(18[0-9])|(17[0-9])|(14[0-9]))\\d{8})$");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="enum">本地还是远程</param>
        /// <param name="type">转换的图片类型</param>
        /// <returns></returns>
        public static byte[] ImageToBytes(string url, CommonEnum.PathEnum @enum = CommonEnum.PathEnum.Remote)
        {
            ImageFormat format = GetImageFormat();
            if (@enum == CommonEnum.PathEnum.Remote)
            {
                try
                {
                    System.Net.WebRequest imgRequest = System.Net.WebRequest.Create(url);
                    HttpWebResponse res;
                    try
                    {
                        res = (HttpWebResponse)imgRequest.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        res = (HttpWebResponse)ex.Response;
                    }
                    
                    Stream stream = res.GetResponseStream();
                    byte[] bytes = new byte[stream.Length];
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Read(bytes, 0, bytes.Length);
                    return bytes;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                return File.ReadAllBytes(url);
            }
        }

        public static byte[] ImageToBytes(Image image, int type = 0)
        {
            ImageFormat format = GetImageFormat();
            MemoryStream ms = new MemoryStream();
            image.Save(ms, format);
            byte[] bytes = ms.ToArray();
            return bytes;
        }

        static ImageFormat GetImageFormat(int type=0)
        {
            ImageFormat format = ImageFormat.Jpeg;
            return format;
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

    }
}
