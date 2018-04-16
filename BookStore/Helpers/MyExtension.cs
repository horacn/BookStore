using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Text;

namespace System.Web.Mvc
{
    /// <summary>
    /// 扩展方法工具类
    /// </summary>
    public static class MyExtension
    {
        /// <summary>
        /// 扩展方法Submit
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static MvcHtmlString Submit(this HtmlHelper html, string name, string value)
        {
            TagBuilder tag = new TagBuilder("input");
            tag.MergeAttribute("type", "submit");
            tag.MergeAttribute("name", name);
            tag.MergeAttribute("value", value);
            tag.GenerateId(name);
            return new MvcHtmlString(tag.ToString());
        }
        public static MvcHtmlString Submit(this HtmlHelper html, string name, string value, Object htmlAttrutes)
        {
            TagBuilder tag = new TagBuilder("input");
            tag.MergeAttribute("type", "submit");
            tag.MergeAttribute("name", name);
            tag.MergeAttribute("value", value);
            tag.GenerateId(name);
            tag.MergeAttributes(
                new RouteValueDictionary(htmlAttrutes)
            );
            return new MvcHtmlString(tag.ToString());
        }
        /// <summary>
        /// 日期扩展（yyyy年MM月）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToMyFormatString(this DateTime dt)
        {
            return dt.ToString("yyyy年MM月");
        }
        /// <summary>
        /// 返回指定长度的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ToMySubstring(this string str, int length)
        {
            if (str.Length > length)
            {
                return str.Substring(0, length) + "...";
            }
            else
            {
                return str;
            }
        }
        /// <summary>
        /// 返回decimal类型的货币类型格式，并带删除线的html文本
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static MvcHtmlString ToMyDecimalAndLine(this decimal number)
        {
            StringBuilder sbf = new StringBuilder();
            sbf.Append("<span style='text-decoration:line-through'>");
            sbf.Append(number.ToString("c"));
            sbf.Append("</span>");
            return new MvcHtmlString(sbf.ToString());
        }
    }
}