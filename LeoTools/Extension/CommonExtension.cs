using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoTools.Extension
{
    public static class CommonExtension
    {
        public static int ToInt32(this string i, int defaultVal = 0)
        {
            int num;
            if (Int32.TryParse(i, out num)) return num;
            else return defaultVal;
        }

        public static string ObjectToString(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

    }
}
