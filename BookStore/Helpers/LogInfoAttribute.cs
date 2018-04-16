using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using BookStore.Models;

namespace System.Web.Mvc
{
    /// <summary>
    /// 自定义过滤器,记录异常日志
    /// </summary>
    public class ExceptionLogInfoAttribute : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// //在发生异常的时候记录日志
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            string path = HttpContext.Current.Server.MapPath("~/Files/Logs/RunException.log");
            StreamWriter writer = new StreamWriter(path,true); //以追加方式来写入文件
            writer.WriteLine("-----------------------------------------------------------------");
            writer.WriteLine(string.Format("发生时间:{0}", DateTime.Now.ToString()));
            writer.WriteLine(string.Format("控制器:{0}", filterContext.RouteData.Values["controller"]));
            writer.WriteLine(string.Format("动作方法:{0}", filterContext.RouteData.Values["action"]));
            writer.WriteLine(string.Format("异常描述:{0}", filterContext.Exception.Message));
            writer.WriteLine("-----------------------------------------------------------------");
            writer.Close();
        }
       
    }
    /// <summary>
    /// 自定义过滤器,记录访问日志
    /// </summary>
    public class VisitNoteLogInfoAttribute : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// 在执行动作方法前添加访问日志
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //记录每次访问的URL地址、IP、时间、用户名
            string path = HttpContext.Current.Server.MapPath("~/Files/Logs/VisitNote.log");
            //获取用户名
            string loginId = filterContext.HttpContext.Session["User"] != null ? ((UserInfo)filterContext.HttpContext.Session["User"]).LoginId.ToString() : "未登录";
            StreamWriter writer = new StreamWriter(path, true); //以追加方式来写入文件
            writer.WriteLine("-----------------------------------------------------------------");
            writer.WriteLine(string.Format("访问时间:{0}", DateTime.Now.ToString()));
            writer.WriteLine(string.Format("用户名:{0}", loginId));
            writer.WriteLine(string.Format("URL:{0}", filterContext.HttpContext.Request.Url.AbsoluteUri));
            writer.WriteLine(string.Format("IP:{0}", filterContext.HttpContext.Request.UserHostAddress));
            writer.WriteLine("-----------------------------------------------------------------");
            writer.Close();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
    /// <summary>
    /// 自定义过滤器,记录登录日志
    /// </summary>
    public class LoginNoteLogInfoAttribute : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// 在执行动作方法之后记录用户登录日志
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //记录IP、时间、用户名
            string path = HttpContext.Current.Server.MapPath("~/Files/Logs/LoginNote.log");
            //获取用户名
            string loginId = filterContext.HttpContext.Session["User"] != null ? ((UserInfo)filterContext.HttpContext.Session["User"]).LoginId.ToString() : "登录失败";
            StreamWriter writer = new StreamWriter(path, true); //以追加方式来写入文件
            writer.WriteLine("-----------------------------------------------------------------");
            writer.WriteLine(string.Format("登录时间:{0}", DateTime.Now.ToString()));
            writer.WriteLine(string.Format("用户名:{0}", loginId));
            writer.WriteLine(string.Format("IP:{0}", filterContext.HttpContext.Request.UserHostAddress));
            writer.WriteLine("-----------------------------------------------------------------");
            writer.Close();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }
    /// <summary>
    /// 自定义过滤器,记录用户退出日志
    /// </summary>
    public class CheckOutNoteLogInfoAttribute : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// 在执行动作方法之前记录用户退出日志
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["User"] != null)
	        { 
                //记录IP、时间、用户名
                string path = HttpContext.Current.Server.MapPath("~/Files/Logs/CheckOutNote.log");
                //获取用户名
                string loginId = ((UserInfo)filterContext.HttpContext.Session["User"]).LoginId.ToString();            
                StreamWriter writer = new StreamWriter(path, true); //以追加方式来写入文件
                writer.WriteLine("-----------------------------------------------------------------");
                writer.WriteLine(string.Format("退出时间:{0}", DateTime.Now.ToString()));
                writer.WriteLine(string.Format("用户名:{0}", loginId));
                writer.WriteLine(string.Format("IP:{0}", filterContext.HttpContext.Request.UserHostAddress));
                writer.WriteLine("-----------------------------------------------------------------");
                writer.Close();
	        }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
    /// <summary>
    /// 自定义过滤器,在执行动作方法前，检查Session["User"]是否为Null
    /// </summary>
    public class CheckUserIsNullAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
        //如果User为空，就回到首页
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["User"] == null)
            {
                filterContext.HttpContext.Response.Write("<script>alert('当前账户已过期，请重新登录！');window.location='" + (filterContext.Controller as Controller).Url.Action("Index", "Home") + "';</script>");
            }
        }
    }
}