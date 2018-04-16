using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.BLL;
using BookStore.Models;

namespace BookStore.Controllers
{
    [ExceptionLogInfo]
    public class HomeController : Controller
    {
        private BookManager bm = new BookManager();

        //显示首页
        [OutputCache(Duration = 5)]
        [VisitNoteLogInfo]
        public ActionResult Index()
        {
            //获得首页图书数据
            //最新图书
            ViewBag.NewBooks = bm.GetBooks(9, "PublishDate DESC");
            //热销排行
            ViewBag.HotSellBooks = bm.GetBooks(12, "Clicks DESC");
            //编辑推荐
            ViewBag.RecommendBooks = bm.GetBooks(12, "UnitPrice");
            return View();
        }
        //显示全部图书分类为分部视图
        public ActionResult CategoryTree()
        {
            //如果图书类别没有缓存，则新建缓存
            if (HttpContext.Cache["trees"] == null)
	        {
                HttpContext.Cache["trees"] = new CategoryManager().GetCategoriesAll();
	        }
            IList<Categorie> categories = HttpContext.Cache["trees"] as IList<Categorie>;
            return PartialView(categories);//返回分部视图
        }
        //自动补全搜索词(ajax)
        public ActionResult AutoCompleteKeywords(string keyword)
        {
            var keywords = new SearchKeywordManager().GetSearchKeywords(keyword.Trim());
            //返回Json数据
            return Json(keywords, JsonRequestBehavior.AllowGet);
        }
    }
}
