using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LeoTools.Validation
{
    /// <summary>
    /// decimal类型in操作验证
    /// 只验证传入的decimal值, 如果值在传入的值当中, 则验证不通过
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DecimalInAttribute : ValidationAttribute
    {
        private readonly double[] ValueArray;

        /// <summary>
        /// int类型in操作验证
        /// 只验证传入的int值, 如果值在传入的值当中, 则验证不通过
        /// </summary>
        /// <param name="errorMsg">验证失败的提示消息</param>
        /// <param name="decimals">值数组, 如, 只验证1和2, 则传入1,2即可</param>
        public DecimalInAttribute(string errorMsg, params double[] decimals)
        {
            this.ValueArray = decimals;
            base.ErrorMessage = errorMsg;
        }

        public override bool IsValid(object value)
        {
            try
            {
                double @decimal = Convert.ToDouble(value);
                if (ValueArray.Any(t => t == @decimal))
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
            catch (Exception)
            {
                throw new ParamsException(base.ErrorMessage);
            }
        }
    }
}