using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace LeoTools.DataTypeExtend
{
    /// <summary>
    /// DataTable扩展方法
    /// </summary>
    
    public static class DataTablePlugins
    {
        /// <summary>
        /// DataTable转List集合, 泛型类里面的属性, 要和DataTable里面的列名一致, 否则异常
        /// DataTable里面的列必须在类里面存在, 否则异常, 类里面的属性可以不出现在DataTable的列里面
        /// </summary>
        /// <typeparam name="T">DataTable对应的类</typeparam>
        /// <param name="dt">数据源</param>
        /// <returns>List集合</returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            if (dt == null) throw new Exception("DataTable数据源为空, 转换无意义");
            var rseult = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(dt));
            dt.Clear(); dt.Dispose(); dt = null;
            return rseult;
        }

        /// <summary>
        /// 集合类型转DataTable
        /// </summary>
        /// <param name="list">集合数据源</param>
        /// <returns>DataTable</returns>
        
        public static DataTable ToDataTable(this IList list)
            => list?.Count > 0 ? JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(list)) : throw new Exception("数据源为空");


        /// <summary>
        /// 设置DataTable字段顺序
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName">列名</param>
        /// <param name="sort">排序 索引从小到大</param>
        public static void SetColumnSort(this DataTable dt, string columnName, int sort)
        {
            dt.Columns[columnName].SetOrdinal(sort);
        }

        /// <summary>
        /// 移除指定字段
        /// </summary>
        public static void RemoveColumn(this DataTable dt, string columnName)
        {
            dt.Columns.Remove(columnName);
        }


    }
}