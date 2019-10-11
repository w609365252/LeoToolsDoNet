using System;
using System.ComponentModel.DataAnnotations;

namespace LeoTools.Validation
{
    /// <summary>
    /// 最大值验证, 支持short/int/long
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class MaxAttribute : ValidationAttribute
    {
        private readonly long MaxValue;

        /// <summary>
        /// 最大值验证, 支持short/int/long
        /// </summary>
        /// <param name="errorMsg">验证失败的提示</param>
        /// <param name="maxValue">最大值</param>
        public MaxAttribute(long maxValue, string errorMsg)
        {
            this.MaxValue = maxValue;
            base.ErrorMessage = errorMsg;
        }

        public override bool IsValid(object value)
        {
            if (value == null) throw new ParamsException(base.ErrorMessage);
            if (value is short number)
            {
                if (number < this.MaxValue)
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
            else if (value is int number1)
            {
                if (number1 < this.MaxValue)
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
            else if (value is long number2)
            {
                if (number2 < this.MaxValue)
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
            throw new ParamsException("请传人整型类型的值");
        }
    }
}