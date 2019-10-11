using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using LeoTools.Web;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using LeoTools.Extension;
using System.Drawing;

namespace LeoTools.MiniprogramApiHelper
{
    public partial class MiniprogramApiHelper
    {
        private static string AppID = ConfigurationManager.AppSettings["AppID"];
        private static string AppSercet = ConfigurationManager.AppSettings["AppSercet"];
        static string SHSercret = ConfigurationManager.AppSettings["SHSercret"];
        static string MCHID = ConfigurationManager.AppSettings["MCHID"];
        static string WeChatPayCallBack = ConfigurationManager.AppSettings["WeChatPayCallBack"];

        static string GetToken()
        {
            string str = LeoHttpHelper.Get($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={AppID}&secret={AppSercet}");
            var obj = str.ToDynamic();
            return obj.access_token;
        }

    }
}