using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LeoTools.Validation
{
    /// <summary>
    /// 枚举值验证, 只验证出现的值
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EnumIntAttribute : ValidationAttribute
    {
        private readonly int[] ValueArray;

        /// <summary>
        /// 枚举值验证, 只验证出现的值
        /// </summary>
        /// <param name="errorMsg">验证失败的消息</param>
        /// <param name="ints">值数组, 如, 只验证1和2, 则传入1,2即可</param>
        public EnumIntAttribute(string errorMsg, params int[] ints)
        {
            base.ErrorMessage = errorMsg;
            this.ValueArray = ints;
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