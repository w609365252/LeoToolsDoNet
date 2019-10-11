using System;
using System.ComponentModel.DataAnnotations;

namespace LeoTools.Validation
{
    /// <summary>
    /// 验证是不是int类型数字 (int/short只要满足一个就行, 大包含小原则)
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class IntAttribute : ValidationAttribute
    {
        private readonly bool U = false;

        private readonly bool Zero = false;

        /// <summary>
        /// 验证是不是int类型数字
        /// </summary>
        /// <param name="errorMsg">验证失败的提示消息</param>
        /// <param name="u">是否只验证正数, 负数一律验证不通过, 默认不是</param>
        /// <param name="zero">是否排除 0 , 当u设置为true的时候, 该参数才有效, 默认不排除</param>
        public IntAttribute(string errorMsg, bool u = false, bool zero = false)
        {
            base.ErrorMessage = errorMsg;
            this.U = u;
            this.Zero = zero;
        }

        public override bool IsValid(object value)
        {
            if (value == null) throw new ParamsException(base.ErrorMessage);
            try
            {
                long @long = Convert.ToInt32(value);
                if (this.U && this.Zero && @long <= 0)
                {
                    throw new ParamsException(base.ErrorMessage);
                }
                else if (this.U && !this.Zero && @long < 0)
                {
                    throw new ParamsException(base.ErrorMessage);
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw new ParamsException(base.ErrorMessage);
            }
        }
    }
}