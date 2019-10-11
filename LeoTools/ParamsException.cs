using System;

namespace LeoTools
{
    /// <summary>
    /// 安合丰自定义异常
    /// </summary>
    public class ParamsException : ApplicationException
    {
        /// <summary>
        /// 没有参数错误说明的异常
        /// </summary>
        public ParamsException()
        {
        }

        /// <summary>
        /// 带参数失败异常信息说明
        /// </summary>
        /// <param name="message">参数失败说明</param>
        public ParamsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 带参数失败异常信息说明和异常对象
        /// </summary>
        /// <param name="message">参数失败说明</param>
        /// <param name="inner">异常对象</param>
        public ParamsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}