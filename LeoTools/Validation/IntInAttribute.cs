using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LeoTools.Validation
{
    /// <summary>
    /// int类型in操作验证
    /// 只验证传入的int值, 如果值在传入的值当中, 则验证不通过
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class IntInAttribute : ValidationAttribute
    {
        private readonly int[] ValueArray;

        /// <summary>
        /// int类型in操作验证
        /// 只验证传入的int值, 如果值在传入的值当中, 则验证不通过
        /// </summary>
        /// <param name="errorMsg">验证失败的提示消息</param>
        /// <param name="ints">值数组, 如, 只验证1和2, 则传入1,2即可</param>
        public IntInAttribute(string errorMsg, params int[] ints)
        {
            this.ValueArray = ints;
            base.ErrorMessage = errorMsg;
        }

        public override bool IsValid(object value)
        {
            try
            {
                int @int = Convert.ToInt32(value);
                if (ValueArray.Any(t => t == @int))
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