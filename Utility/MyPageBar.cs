using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class MyPageBar
    {
        //会员中心
        public static string PageSortCenter(int pageIndex, int pageCount, string urlparams)
        {
            if (pageCount == 1)
            {
                return string.Empty;
            }
            int start = pageIndex - 5;
            start = start < 1 ? 1 : start;
            int end = start + 9;
            end = end > pageCount ? pageCount : end;
            StringBuilder sb = new StringBuilder();
            if (pageIndex > 1)
            {
                sb.Append(string.Format("<a href='" + urlparams + "{0}.html' class='layui-laypage-prev'>上一页</a>", pageIndex - 1));
            }
            for (int i = start; i <= end; i++)
            {
                if (pageIndex == i)
                {
                    sb.Append(string.Format("<span class='layui-laypage-curr'><em class='layui-laypage-em'></em><em>{0}</em></span>", i));
                }
                else
                {
                    sb.Append(string.Format("<a href='" + urlparams + "{0}.html'>{0}</a>", i));
                }
            }

            if (pageIndex < pageCount)
            {
                sb.Append(string.Format("<a href='" + urlparams + "{0}.html' class='layui-laypage-next'>下一页</a>", pageIndex + 1));
            }
            return sb.ToString();
        }
    }
}
