using System;

namespace LeoTools.NLog
{
    using global::NLog;
    /// <summary>
    /// 日志操作
    /// </summary>
    
    public static class LogerManager
    {
        /// <summary>
        /// NLog操作对象
        /// </summary>
        private static Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 正常日志记录
        /// </summary>
        /// <param name="msg">记录输出信息</param>
        public static void Info(string msg)
        {
            Log.Info(msg);
        }

        /// <summary>
        /// Debug调试日志输入
        /// </summary>
        /// <param name="msg">Debug输出信息</param>
        public static void Debug(string msg)
        {
            Log.Debug(msg);
        }

        /// <summary>
        /// Debug调试日志输入
        /// </summary>
        /// <param name="msg">Debug输出信息</param>
        /// <param name="debugName">Debug名称</param>
        public static void Debug(string msg, string debugName)
        {
            Log.Debug(new Exception(msg), debugName);
        }

        /// <summary>
        /// 错误日志记录
        /// </summary>
        /// <param name="msg">错误记录信息</param>
        public static void Error(string msg)
        {
            Log.Error(msg);
        }

        /// <summary>
        /// 错误日志记录
        /// </summary>
        /// <param name="ex">异常捕获的对象</param>
        public static void Error(Exception ex)
        {
            Log.Error(ex);
        }

        /// <summary>
        /// 错误日志记录
        /// </summary>
        /// <param name="errorName">异常记录名字 一般传类名或者自定义名字, 自己能根据该名字定位到错误位置就行</param>
        /// <param name="ex">异常捕获的对象</param>
        public static void Error(string errorName, Exception ex)
        {
            Log.Error(ex, errorName);
        }
    }
}