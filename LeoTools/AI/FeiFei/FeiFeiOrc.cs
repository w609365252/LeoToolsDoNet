using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Newtonsoft.Json;
using System.Drawing;

namespace LeoTools.AI.FeiFeiDaMa
{
    public class FeiFeiOrc
    {
        public static FateadmRsp AiValite(byte[] bytes)
        {
            return FeiFeiFactory.AiValite(bytes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type">0服务器路径 1本地路径</param>
        /// <returns></returns>
        public static FateadmRsp AiValite(string url, CommonEnum.PathEnum @enum= CommonEnum.PathEnum.Remote)
        {
            return FeiFeiFactory.AiValite(url, @enum);
        }

        public static FateadmRsp AiValite(Image image)
        {
            return FeiFeiFactory.AiValite(image);
        }
    }

}
