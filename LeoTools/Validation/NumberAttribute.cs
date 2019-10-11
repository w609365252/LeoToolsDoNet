using LeoTools.DataTypeExtend;
using System;
using System.ComponentModel.DataAnnotations;

namespace LeoTools.Validation
{
    /// <summary>
    /// 是否是纯数字验证
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class NumberAttribute : ValidationAttribute
    {
        /// <summary>
        /// 是否是纯数字验证
        /// </summary>
        /// <param name="errorMsg">验证失败的提示信息</param>
        public NumberAttribute(string errorMsg)
        {
            base.ErrorMessage = errorMsg;
        }

        public override bool IsValid(object value)
        {
            if (value is string str)
            {
                if (str.IsNumber())
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
            throw new ParamsException(base.ErrorMessage);
        }
    }
}