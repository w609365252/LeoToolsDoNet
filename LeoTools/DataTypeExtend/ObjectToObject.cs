using System;
using static Newtonsoft.Json.JsonConvert;

namespace LeoTools.DataTypeExtend
{
    /// <summary>
    /// 两个结构相同的类, 互相转换
    /// </summary>
    
    public class ObjectToObject
    {
        /// <summary>
        /// 两个结构相同的类, 互相转换
        /// </summary>
        /// <typeparam name="Class2">目标类型</typeparam>
        /// <param name="c1">源类型类对象</param>
        /// <returns>返回目标类型类对象</returns>
        
        public static Class2 To<Class2>(object c1) where Class2 : class, new()
        {
            if (c1 == null) throw new Exception("源数据类型类为空");
            return DeserializeObject<Class2>(SerializeObject(c1));
        }
    }
}