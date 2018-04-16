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
    public class CategoryController : Controller
    {
        private CategoryManager cm = new CategoryManager();

        //显示图书分类首页
        public ActionResult Index()
        {
            return View();
        }
        //显示全部图书分类(ajax调用)
        public ActionResult DisPlay()
        {
            var categroys = cm.GetCategoriesAll();
            return PartialView(categroys);
        }
        //添加图书分类(ajax)
        public ActionResult Add(string name)
        {
            if (string.IsNullOrEmpty(name.Trim()))
            {
                return Content("1");//名称为空
            }
            if (cm.Exists(name.Trim()))
            {
                return Content("2");//重复
            }
            int result = cm.Add(new Categorie {Name=name.Trim()});
            if (result==1)
            {
                UpdateBookCategoryCache();
                return Content("3");//成功
            }
            else
            {
                return Content("4");//失败
            }
        }
        //显示修改图书分类页面
        public ActionResult Edit(int id)
        {
            var categroy = cm.GetCategorieById(id);
            return View(categroy);
        }
        //修改图书分类
        [HttpPost]
        public ActionResult Edit(int id, string name)
        {
            if (name.Trim()=="")
            {
                TempData["message"] = "名称不能为空";
                return RedirectToAction("Edit", "Category", new { Id = id });
            }
            if (cm.Exists(name.Trim(),id))
            {
                TempData["message"] = "已经存在这个分类名称";
                return RedirectToAction("Edit", "Category", new { Id = id });
            }
            Categorie c = new Categorie {Id=id, Name = name.Trim() };
            cm.Update(c);
            TempData["message"] = "修改图书分类成功";
            UpdateBookCategoryCache();
            return RedirectToAction("Index", "Category");
        }
        //删除图书分类
        public ActionResult Delete(int id)
        {
            try
            {
                cm.Delete(id);
                TempData["message"] = "删除图书分类成功";
                UpdateBookCategoryCache();
                return RedirectToAction("Index", "Category");
            }
            catch (Exception)
            {
                string message = "出现的错误可能有：1、此图书类别下还有图书，暂时不能删除。";
                message += "2、数据库在执行删除操作时，出现异常。";
                Exception ex = new Exception(message);
                new ExceptionLogInfoAttribute().OnException(new ExceptionContext(this.ControllerContext, ex));
                return View("Error", new HandleErrorInfo(ex, "Category", "Delete"));
            }
        }
        //更新图书类别缓存
        [NonAction]
        private void UpdateBookCategoryCache()
        {
            //如果缓存不为空，则删除缓存
            if (HttpContext.Cache["trees"] != null)
            {
                HttpContext.Cache.Remove("trees");
            }
            HttpContext.Cache["trees"] = new CategoryManager().GetCategoriesAll();            
        }
    }
}
