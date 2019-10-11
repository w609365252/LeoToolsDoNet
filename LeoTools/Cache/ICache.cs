using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoTools.Cache
{
    public interface ICache
    {
        //获取缓存
        T Get<T>(string key);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="Expire">到期时间秒为单位，小于0表示不设置到期时间</param>
        /// <returns></returns>
        bool Set(string key, object value, int Expire = 0);
        //移除缓存
        bool Remove(string key);
    }
}
