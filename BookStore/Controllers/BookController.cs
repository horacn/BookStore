using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.BLL;
using BookStore.Helpers;

namespace BookStore.Controllers
{
    [ExceptionLogInfo]
    [VisitNoteLogInfo]
    public class BookController : Controller
    {
        private BookManager bm = new BookManager();

        //书籍详情页面
        [HandleError(Order=1,ExceptionType=typeof(Exception),View="Error")]//错误特性
        [OutputCache(Order=2,Duration = 3600)]//页面缓存（Duration：缓存时间[秒]）
        public ActionResult Detail(int id)
        {
            //int id = Convert.ToInt32(RouteData.Values["Id"]);
            Book book = bm.GetBookById(id);
            //ViewBag.Book = book;
            return View(book);
        }
        //分页显示分类图书
        public ActionResult List(int categoryId, string sort = "PublishDate DESC", int pageIndex=1)
        {
            //设置页面标题
            Categorie category = new CategoryManager().GetCategorieById(categoryId);
            ViewBag.Title = category != null ? category.Name + "_分类图书列表_第三波书店" : "分类图书列表_第三波书店";//类别名称
            //分页图书
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int pageSize = 5;
            string condition = "And CategoryId="+categoryId;
            int recordCount;
            var books = bm.GetBooks(pageSize,pageIndex,condition,sort,out recordCount);
            int totalPages = recordCount % pageSize == 0 ? recordCount / pageSize : recordCount / pageSize + 1;
            if (totalPages==0)
            {
                totalPages = 1;
            }
            if (pageIndex>totalPages)
            {
                pageIndex = totalPages;
                books = bm.GetBooks(pageSize, pageIndex, condition, sort, out recordCount);
            }
            ViewBag.TotalPages = totalPages;
            ViewBag.PageIndex = pageIndex;
            ViewBag.Sort = sort;
            ViewBag.CategoryId = categoryId;
            return View(books);
        }
        //搜索图书页面 + 自定义分页
        public ActionResult Search(string title,int? category,int pageIndex = 1)
        {
            //获取全部类别，并绑定到下拉列表
            ViewBag.category = new SelectList(new CategoryManager().GetCategoriesAll(),"Id","Name");
            string condition = string.Empty;
            string cateTitle = "";
            //拼接条件
            if (category.HasValue)
            {
                condition += string.Format(" And CategoryId={0}",category);
                //获取图书分类对象,分类名称
                Categorie tempCategory = new CategoryManager().GetCategorieById(category.Value);
                if (tempCategory != null)
                {
                    cateTitle = tempCategory.Name+"_";
                }
            }
            if (!string.IsNullOrEmpty(title) && title.Trim()!="")
            {
                condition += string.Format(" And Title Like'%{0}%'",title);
                //添加搜索词
                new SearchKeywordManager().Add(title);
            }
            condition += " Order By Clicks Desc";
            //设置页面标题
            ViewBag.Title = !string.IsNullOrEmpty(title) ? cateTitle + title + "_图书搜索页_第三波书店" : cateTitle + "图书搜索页_第三波书店";
            //根据条件获得图书集合
            var books = bm.GetBooks(condition);
            int pageSize = 10;
            //如果是post请求，则说明是通过条件查询图书，pageIndex=1
            if (Request.RequestType == "POST")
            {
                pageIndex = 1;
            }
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            //分页
            var pagedBooks = new PagedList<Book>(books,pageSize,pageIndex);
            return View(pagedBooks);
        }
    }
}
