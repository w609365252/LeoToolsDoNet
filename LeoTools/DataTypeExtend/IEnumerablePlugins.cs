using System;
using System.Collections;

namespace LeoTools.DataTypeExtend
{
    /// <summary>
    /// IEnumerable扩展方法
    /// </summary>
    
    public static class IEnumerablePlugins
    {
        /// <summary>
        /// 从List里面移除多个元素
        /// </summary>
        /// <param name="data">源数据</param>
        /// <param name="array">要移除的数据</param>
        /// <returns></returns>
        public static void RemoveArray(this IList data, IList array)
        {
            if (data == null || data.Count <= 0)
            {
                throw new Exception("源数据库为空, 无意义");
            }
            if (array == null || array.Count <= 0)
            {
                return;
            }
            foreach (var item in array)
            {
                data.Remove(item);
            }
        }

        
    }
}