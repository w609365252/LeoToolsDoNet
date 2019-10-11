using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeoTools.Common;
namespace LeoTools.Extension
{
    public static class PagerExtension
    {
        public static List<T> GetPage<T>(this List<T> list, PageFliter pageFliter)
        {
            pageFliter.TotalCount = list.Count();
            pageFliter.TotalPage = list.Count > 0 ? Convert.ToInt32(Math.Ceiling(Decimal.Parse(list.Count().ToString()) / Decimal.Parse(pageFliter.PageSize.ToString())))
                                    : 0;
            return list.Skip((pageFliter.PageIndex - 1) * pageFliter.PageSize).Take(pageFliter.PageSize).ToList();
        }
    }

    public class PageFliter
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string OrderBy { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
    }
}
