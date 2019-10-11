using MSScriptControl;
using System;
using System.IO;

namespace LeoTools.FileHelper
{
    /// <summary>
    /// JavascriptHelper
    /// </summary>
    public class JavascriptHelper
    {
        /// <summary>
        /// ScriptControl
        /// </summary>
        private ScriptControl jsControl = null;

        /// <summary>
        /// 构造方法
        /// </summary>
        public JavascriptHelper()
        {
            this.jsControl = new ScriptControl();
            this.jsControl.UseSafeSubset = true;
            this.jsControl.Language = "JScript";
        }

        /// <summary>
        /// 添加js文件
        /// </summary>
        /// <param name="filePath">js文件路径</param>
        public void AddJavaScriptFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("文件" + filePath + "不存在。");
            }

            string jsCode = File.ReadAllText(filePath);
            this.jsControl.AddCode(jsCode);
        }

        /// <summary>
        /// 添加js代码
        /// </summary>
        /// <param name="jsCode">js代码</param>
        public void AddJavascriptCode(string jsCode)
        {
            this.jsControl.AddCode(jsCode);
        }

        /// <summary>
        /// 执行js
        /// </summary>
        /// <param name="method">方法名</param>
        /// <returns>结果</returns>
        public dynamic Excecute(string method)
        {
            return this.jsControl.Eval(method);
        }
    }
}
