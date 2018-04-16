using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.BLL;
using BookStore.Models;

namespace BookStore.Areas.Admin.Controllers
{
    [Authorize]
    [ExceptionLogInfo]
    public class PublisherController : Controller
    {
        private PublisherManager pm = new PublisherManager();

        //显示全部出版社
        public ActionResult Index()
        {
            var publishers = pm.GetPublishersAll();
            return View(publishers);
        }
        //添加出版社
        public ActionResult Add(string name)
        {
            if (name.Trim() == "")
            {
                TempData["message"] = "出版社名称不能为空";
                return RedirectToAction("Index", "Publisher");
            }
            if (pm.Exists(name.Trim()))
            {
                TempData["message"] = "已经存在这个出版社名称";
                return RedirectToAction("Index", "Publisher");
            }
            int result = pm.Add(name.Trim());
            if (result >0)
            {
                TempData["message"] = "添加出版社成功";
                return RedirectToAction("Index", "Publisher");
            }
            else
            {
                TempData["message"] = "添加出版社失败，可能是数据有误";
                return RedirectToAction("Index", "Publisher");
            }
        }
        //显示编辑出版社页面
        public ActionResult Edit(int id)
        {
            var publisher = pm.GetPublisherById(id);
            return View(publisher);
        }
        //编辑出版社
        [HttpPost]
        public ActionResult Edit(int id, string name)
        {
            if (name.Trim() == "")
            {
                TempData["message"] = "出版社名称不能为空";
                return RedirectToAction("Edit", "Publisher", new { Id = id });
            }
            if (pm.Exists(name.Trim(), id))
            {
                TempData["message"] = "已经存在这个出版社名称";
                return RedirectToAction("Edit", "Publisher", new { Id = id });
            }
            Publisher p = new Publisher { 
                Id = id,
                Name = name.Trim()
            };
            pm.Update(p);
            TempData["message"] = "修改出版社成功";
            return RedirectToAction("Index", "Publisher");
        }
        //删除出版社
        public ActionResult Delete(int id)
        {
            try
            {
                pm.Delete(id);
                TempData["message"] = "删除出版社成功";
                return RedirectToAction("Index", "Publisher");
            }
            catch (Exception)
            {
                string message = "出现的错误可能有：1、此出版社下还有图书，暂时不能删除。";
                message += "2、数据库在执行删除操作时，出现异常。";
                Exception ex = new Exception(message);
                new ExceptionLogInfoAttribute().OnException(new ExceptionContext(this.ControllerContext, ex));
                return View("Error", new HandleErrorInfo(ex, "Publisher", "Delete"));
            }
        }
    }
}
