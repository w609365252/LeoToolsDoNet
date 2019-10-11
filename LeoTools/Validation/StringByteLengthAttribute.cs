using LeoTools.DataTypeExtend;
using System;
using System.ComponentModel.DataAnnotations;

namespace LeoTools.Validation
{
    /// <summary>
    /// 字符串字节长度验证, 和长度有区别, 比如一个中文是两个字节
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class StringByteLengthAttribute : ValidationAttribute
    {
        private readonly int ByteLength = 0;

        private readonly bool NullNot = true;

        private readonly bool IsTrim = false;

        /// <summary>
        /// 字符串字节长度验证, 和长度有区别, 比如一个中文是两个字节
        /// </summary>
        /// <param name="byteLength">字符串最大字节长度</param>
        /// <param name="nullNot">字符串为null/""/"  "的时候是否不进行验证, 默认跳过</param>
        /// <param name="errorMsg">验证失败的错误提示信息</param>
        /// <param name="isTrim">是否去掉两边的空格再进行验证, 默认不是</param>
        public StringByteLengthAttribute(int byteLength, bool nullNot = true, string errorMsg = "字符串过长", bool isTrim = false)
        {
            this.ByteLength = byteLength;
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
                if (str.GetBytesLength() <= this.ByteLength)
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
            if (this.NullNot)
            {
                return true;
            }
            throw new ParamsException(base.ErrorMessage);
        }
    }
}