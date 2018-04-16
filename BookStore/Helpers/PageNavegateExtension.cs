using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Helpers;
using System.Web.Routing;
using System.Text;
using System.Web.Mvc.Html;

namespace System.Web.Mvc
{
    /// <summary>
    /// HtmlHelper类的扩展方法类
    /// </summary>
    public static class PageNavegateExtension
    {
        /// <summary>
        /// HtmlHelper类的分页导航条扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="data">数据</param>
        /// <returns>HTML元素</returns>
        public static MvcHtmlString Pager<T>(this HtmlHelper html, PagedList<T> data)
        {
            //起始数字
            int start = data.PageIndex - 5 >= 1 ? data.PageIndex - 5 : 1;
            //结束数字
            int end = data.TotalPages - start > 10 ? start + 10 : data.TotalPages;
            //路由数据
            RouteValueDictionary rvs = html.ViewContext.RouteData.Values;
            //url数据
            var querystring = html.ViewContext.HttpContext.Request.QueryString;
            //合并url数据
            foreach (string key in querystring.Keys)
            {
                if (querystring[key] != null && !string.IsNullOrEmpty(key))
                {
                    rvs[key] = querystring[key];
                }
            }
            //合并form数据
            var forms = html.ViewContext.HttpContext.Request.Form;
            foreach (string key in forms.Keys)
            {
                rvs[key] = forms[key];
            }

            //拼装分页html
            StringBuilder builder = new StringBuilder();

            //显示首页和上一页
            if (data.HasPreviousPage)
            {
                rvs["pageIndex"] = 1;
                builder.Append(LinkExtensions.ActionLink(html, "首页", rvs["action"].ToString(), rvs));
                rvs["pageIndex"] = data.PageIndex - 1;
                builder.Append(LinkExtensions.ActionLink(html, "上一页", rvs["action"].ToString(), rvs));
            }

            //显示数字页码
            for (int i = start; i <= end; i++)
            {
                rvs["pageIndex"] = i;
                if (i == data.PageIndex)
                {
                    builder.AppendFormat("<font>{0}</font>", i);
                }
                else
                {
                    builder.Append(LinkExtensions.ActionLink(html, i.ToString(), rvs["action"].ToString(), rvs));
                }
            }

            //显示下一页和尾页
            if (data.HasNextPage)
            {
                rvs["pageIndex"] = data.PageIndex + 1;
                builder.Append(LinkExtensions.ActionLink(html, "下一页", rvs["action"].ToString(), rvs));
                rvs["pageIndex"] = data.TotalPages;
                builder.Append(LinkExtensions.ActionLink(html, "尾页", rvs["action"].ToString(), rvs));
            }

            //显示页码信息
            builder.AppendFormat("第{0}页/共{1}页", data.PageIndex, data.TotalPages);
            return new MvcHtmlString(builder.ToString());
        }
    }
}