using System;
using System.ComponentModel.DataAnnotations;

namespace LeoTools.Validation
{
    /// <summary>
    /// 字符串属性, 非空验证(字符串为null/""/"  "都会成立)
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class StringEmptyAttribute : ValidationAttribute
    {
        /// <summary>
        /// 字符串属性, 非空验证(字符串为null/""/"  "都会成立)
        /// </summary>
        /// <param name="errorMsg">字符串为空的错误提示消息, 默认为 : "字符串为空"</param>
        public StringEmptyAttribute(string errorMsg = "字符串为空")
        {
            base.ErrorMessage = errorMsg;
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value is string str)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    throw new ParamsException(base.ErrorMessage);
                }
                return true;
            }
            else
            {
                throw new ParamsException(base.ErrorMessage);
            }
        }
    }
}