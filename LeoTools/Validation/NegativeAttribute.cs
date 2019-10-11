using System;
using System.ComponentModel.DataAnnotations;

namespace LeoTools.Validation
{
    /// <summary>
    /// 负数验证, 只验证负数
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class NegativeAttribute : ValidationAttribute
    {
        private bool NotNull = true;

        /// <summary>
        /// 负数验证, 只验证负数
        /// </summary>
        /// <param name="errorMsg">验证失败的提示</param>
        /// <param name="isNull">为null的时候是否不进行验证, 默认不进行null验证</param>
        public NegativeAttribute(string errorMsg = "请输入一个小于0的数", bool isNull = true)
        {
            base.ErrorMessage = errorMsg;
            this.NotNull = isNull;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (value is int number)
            {
                if (number >= 0)
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