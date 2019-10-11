using LeoTools.DataTypeExtend;
using System;
using System.ComponentModel.DataAnnotations;

namespace LeoTools.Validation
{
    /// <summary>
    /// 车牌号属性验证
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class LicensePlateAttribute : ValidationAttribute
    {
        /// <summary>
        /// 车牌号属性验证
        /// </summary>
        /// <param name="errorMsg">验证失败的消息, 默认为 "请输入有效的车牌号"</param>
        public LicensePlateAttribute(string errorMsg = "请输入有效的车牌号") => base.ErrorMessage = errorMsg;

        public override bool IsValid(object value)
        {
            if (value is string licensePlate)
            {
                if (licensePlate.IsLicensePlate())
                {
                    return true;
                }
                throw new ParamsException(base.ErrorMessage);
            }
            else
            {
                throw new ParamsException(base.ErrorMessage);
            }
        }
    }
}