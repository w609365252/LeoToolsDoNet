using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoTools.Extension
{
    public static class StringExtension
    {
        public static string[] SplitExtension(this string str, string val, StringSplitOptions stringSplit = StringSplitOptions.RemoveEmptyEntries)
        {
            return str.Split(new string[] { val }, stringSplit);
        }

        public static dynamic ToDynamic(this string str)
        {
            return JsonConvert.DeserializeObject<dynamic>(str);
        }

    }
}
