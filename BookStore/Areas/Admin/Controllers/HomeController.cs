using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.BLL;

namespace BookStore.Areas.Admin.Controllers
{
    [Authorize]//过滤器
    [ExceptionLogInfo]
    [VisitNoteLogInfo]
    public class HomeController : Controller
    {
		//管理员首页
        public ActionResult Index()
        {
            return View();
        }
    }
}
