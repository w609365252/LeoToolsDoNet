using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    }
}
