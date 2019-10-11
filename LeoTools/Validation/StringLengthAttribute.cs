using System;
using System.ComponentModel.DataAnnotations;

namespace LeoTools.Validation
{
    /// <summary>
    /// 字符串长度验证
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class StringLengthAttribute : ValidationAttribute
    {
        private readonly int Length = 0;

        private readonly bool NullNot = true;

        private readonly bool IsTrim = false;

        /// <summary>
        /// 字符串长度验证
        /// </summary>
        /// <param name="length">字符串最大长度</param>
        /// <param name="nullNot">字符串为null/""/"  "的时候是否不进行验证, 默认跳过</param>
        /// <param name="errorMsg">验证失败的错误提示信息</param>
        /// <param name="isTrim">是否去掉两边的空格再进行验证, 默认不去掉</param>
        public StringLengthAttribute(int length, bool nullNot = true, string errorMsg = "字符串长度过长", bool isTrim = false)
        {
            this.Length = length;
            this.NullNot = nullNot;
            base.ErrorMessage = errorMsg;
            this.IsTrim = isTrim;
        }

        public override bool IsValid(object value)
        {
            if (value is string str)
            {
                if (this.IsTrim)
                {
                    str = str.Trim();
                }
                if (string.IsNullOrWhiteSpace(str) && this.NullNot)
                {
                    return true;
                }
                if (str.Length <= this.Length)
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
            else
            {
                if (this.NullNot)
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
        }
    }
}