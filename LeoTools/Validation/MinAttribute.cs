using System;
using System.ComponentModel.DataAnnotations;

namespace LeoTools.Validation
{
    /// <summary>
    /// 最小值验证, 支持short/int/long
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class MinAttribute : ValidationAttribute
    {
        private readonly long MinValue;

        /// <summary>
        /// 最小值验证, 支持short/int/long
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="errorMsg">验证失败的提示</param>
        public MinAttribute(long minValue, string errorMsg)
        {
            this.MinValue = minValue;
            base.ErrorMessage = errorMsg;
        }

        public override bool IsValid(object value)
        {
            if (value == null) throw new ParamsException(base.ErrorMessage);
            if (value is short number)
            {
                if (number > this.MinValue)
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
            else if (value is int number1)
            {
                if (number1 > this.MinValue)
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
            else if (value is long number2)
            {
                if (number2 > this.MinValue)
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
            throw new ParamsException("请传人整型类型的值");
        }
    }
}