using LeoTools.DataTypeExtend;
using System;
using System.ComponentModel.DataAnnotations;

namespace LeoTools.Validation
{
    /// <summary>
    /// 手机号属性验证
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class PhoneAttribute : ValidationAttribute
    {
        private readonly bool N = false;

        /// <summary>
        /// 手机号属性验证
        /// </summary>
        /// <param name="errorMsg">验证失败的信息, 默认为 "请输入有效的手机号"</param>
        /// <param name="isNull">为null或者空字符串的时候, 是不是不进行验证, 默认不进行验证</param>
        public PhoneAttribute(string errorMsg = "请输入有效的手机号", bool isNull = false)
        {
            base.ErrorMessage = errorMsg;
            this.N = isNull;
        }

        public override bool IsValid(object value)
        {
            if (!this.N)
            {
                if (value == null)
                {
                    return true;
                }
            }
            if (value is string phone)
            {
                if (!this.N)
                {
                    if (phone.IsNullOrWhiteSpace())
                    {
                        return true;
                    }
                }
                if (phone.IsPhone())
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
            else
            {
                throw new ParamsException(base.ErrorMessage);
            }
        }
    }
}