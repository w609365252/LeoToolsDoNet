using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoTools.Web
{
    public class WebHelper
    {
        /// <summary>
        /// 服务器端获取客户端请求IP和客户端机器名称
        /// </summary>
        public static string GetOperateIP()
        {
            try
            {
                string userIP;
                userIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] == null ? System.Web.HttpContext.Current.Request.UserHostAddress : System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                return userIP;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sExpression">调用代码执行脚本 return getCode();</param>
        /// <param name="script">JS脚本 var a=10; function getCode(){return a;}; </param>
        /// <returns></returns>
        public static string ExecuteScript(string sExpression, string script)
        {
            MSScriptControl.ScriptControl scriptControl = new MSScriptControl.ScriptControl();
            scriptControl.UseSafeSubset = true;
            scriptControl.Language = "JavaScript";
            scriptControl.AddCode(script);
            try
            {
                string str = scriptControl.Eval(sExpression).ToString();
                return str;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sExpression">调用代码执行脚本 return getCode();</param>
        /// <param name="script">JS脚本 var a=10; function getCode(){return a;}; </param>
        /// <returns></returns>
        public static string ExecuteScriptByFilePath(string sExpression, string path)
        {
            string str = "";
            string jsScript = File.ReadAllText(path);
            try
            {
                str = ExecuteScript(sExpression, jsScript);
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

    }
}
