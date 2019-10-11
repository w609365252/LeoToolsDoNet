using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace LeoTools.Cache
{
    public class CacheFactory
    {
        private static ICache _systemCache =null;

        public static ICache SystemCache
        {
            get
            {
                if (_systemCache == null) _systemCache = new SystemCache.SystemCache();
                return _systemCache;
            }
        }

        public static string GetEnumDescription(Enum enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs.Length == 0)    //当描述属性没有时，直接返回名称
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }


        public static bool SetCache(string key, object value)
        {
            return SystemCache.Set(key, value);
        }

        public static T GetCache<T>(string key)
        {
            return SystemCache.Get<T>(key);
        }

        public static bool RemoveCache(string key)
        {
            return SystemCache.Remove(key);
        }

    }
}
