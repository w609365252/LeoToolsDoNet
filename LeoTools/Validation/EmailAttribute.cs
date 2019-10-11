using LeoTools.DataTypeExtend;
using System;
using System.ComponentModel.DataAnnotations;

namespace LeoTools.Validation
{
    /// <summary>
    /// 邮箱属性验证
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class EmailAttribute : ValidationAttribute
    {
        /// <summary>
        /// 邮箱属性验证
        /// </summary>
        /// <param name="errorMsg">验证失败的信息, 默认为 "请输入有效的邮箱地址"</param>
        public EmailAttribute(string errorMsg = "请输入有效的邮箱地址") => base.ErrorMessage = errorMsg;

        public override bool IsValid(object value)
        {
            if (value is string email)
            {
                if (email.CheckEmail())
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
            throw new ParamsException(base.ErrorMessage);
        }
    }
}