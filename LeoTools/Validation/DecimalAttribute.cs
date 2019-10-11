using LeoTools.DataTypeExtend;
using System;
using System.ComponentModel.DataAnnotations;

namespace LeoTools.Validation
{
    /// <summary>
    /// 小数验证
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class DecimalAttribute : ValidationAttribute
    {
        /// <summary>
        /// 小数验证
        /// </summary>
        /// <param name="errorMsg">验证失败的提示, 默认为 "非小数"</param>
        public DecimalAttribute(string errorMsg) => base.ErrorMessage = errorMsg;

        public override bool IsValid(object value)
        {
            if (value == null) throw new ParamsException(base.ErrorMessage);
            var @decimal = value.ToString();
            if (@decimal.IsDecimal())
            {
                return true;
            }
            throw new ParamsException(base.ErrorMessage);
        }
    }
}