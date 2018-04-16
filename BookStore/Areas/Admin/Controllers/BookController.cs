using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.BLL;
using BookStore.Helpers;
using System.IO;

namespace BookStore.Areas.Admin.Controllers
{
    [Authorize]
    [ExceptionLogInfo]
    public class BookController : Controller
    {
        private BookManager bm = new BookManager ();

        //显示书籍管理首页
        public ActionResult Index(string searchType, string keyword, int? category, int? publisher, int pageIndex = 1)
        {
            //绑定查询类别
            List<SelectListItem> items = new List<SelectListItem> { 
                new SelectListItem{Value="title",Text="书名"},
                new SelectListItem{Value="CategoryId",Text="类别"},
                new SelectListItem{Value="author",Text="作者"},
                new SelectListItem{Value="ContentDescription",Text="内容简介"},
                new SelectListItem{Value="PublisherId",Text="出版社"}
            };
            ViewBag.searchType = items;
            //获取全部类别、出版社，并绑定到下拉列表
            PrepareDropDownListData(null);
            string condition = string.Empty;
            if (!string.IsNullOrEmpty(searchType))
            {
                keyword = keyword.Trim();
                if (searchType.Equals("title") || searchType.Equals("ContentDescription") || searchType.Equals("author"))
                {
                    condition = " and " + searchType + " like'%" + keyword + "%'";
                }
                else if (searchType.Equals("CategoryId"))
                {
                    condition = " and " + searchType + "=" + category;
                }
                else if (searchType.Equals("PublisherId"))
                {
                    condition = " and " + searchType + "=" + publisher;
                }                     
            }
            condition += " Order By PubLishDate Desc";
            if (!string.IsNullOrEmpty(keyword))
            {
                //添加搜索词
                new SearchKeywordManager().Add(keyword);
            }
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
            var pagedBooks = new PagedList<Book>(books, pageSize, pageIndex);
            return View(pagedBooks);
        }
        //准备下拉列表数据（出版社，图书类别）
        [NonAction]
        private void PrepareDropDownListData(Book book)
        {
            if (book!=null)
            {
                ViewBag.category = new SelectList(new CategoryManager().GetCategoriesAll(), "Id", "Name", book.Categorie.Id);
                ViewBag.publisher = new SelectList(new PublisherManager().GetPublishersAll(), "Id", "Name", book.Publisher.Id);
            }
            else
            {
                ViewBag.category = new SelectList(new CategoryManager().GetCategoriesAll(), "Id", "Name");
                ViewBag.publisher = new SelectList(new PublisherManager().GetPublishersAll(), "Id", "Name");
            }
        }
        //显示添加与编辑书籍页面
        public ActionResult Edit(int? id)
        {
            Book book = null;
            if (id.HasValue)//如果id有值，表示是修改图书
            {
                book = bm.GetBookById(id.Value);
            }
            PrepareDropDownListData(book);
            return View(book);
        }
        //检查标题与ISBN是否和重复
        [NonAction]
        private bool IsExistsTitleAndISBN(Book book)
        {
            bool flag = false;//是否未通过
            if (book.Id != 0)//修改
            {
                //判断标题、ISBN是否重复
                if (book.Title!=null && bm.Exists(keyword: book.Title.Trim(), id: book.Id))
                {
                    ModelState.AddModelError("Title", "已经存在这本图书");
                    flag = true;
                }
                if (book.ISBN != null && bm.Exists(book.ISBN.Trim(), "ISBN", book.Id))
                {
                    ModelState.AddModelError("ISBN", "已经存在这个ISBN");
                    flag = true;
                }
            }
            else//新增
            {
                //判断标题、ISBN是否重复
                if (book.Title != null && bm.Exists(book.Title.Trim()))
                {
                    ModelState.AddModelError("Title", "已经存在这本图书");
                    flag = true;
                }
                if (book.ISBN != null && bm.Exists(book.ISBN.Trim(), "ISBN"))
                {
                    ModelState.AddModelError("ISBN", "已经存在这个ISBN");
                    flag = true;
                }
            }
            return flag;
        }
        //上传文件
        [NonAction]
        private bool UploadFiles(Book book,HttpPostedFileBase cover)
        {
           
            //HttpPostedFileBase cover = Request.Files["cover"];
            string path = Server.MapPath("~/Images/BookCovers/");//文件夹路径
            string oldISBN = book.Id != 0 ? bm.GetBookById(book.Id).ISBN : null;//原来的ISBN
            if (cover == null || cover.ContentLength == 0)//没有选择图片，或选择0字节文件
            {
                //如果是修改图书，且没有选择封面，则仍用原来的图片,把原图片文件名重命名为新的ISBN+jpg
                if (book.Id != 0 && !oldISBN.Equals(book.ISBN))//如果修改了ISBN
                {
                    FileInfo file = new FileInfo(path + oldISBN + ".jpg");
                    if (file.Exists && !System.IO.File.Exists(path + book.ISBN + ".jpg"))
                    {
                        //移动文件，指定新的文件路径，即实现了重命名
                        file.MoveTo(path + book.ISBN + ".jpg");
                    }
                }
                else if(book.Id == 0)//如果是添加图书，就使用默认图片
                {
                    FileInfo file = new FileInfo(Server.MapPath("~/Images/未选择图书封面.jpg"));
                    file.CopyTo(path + book.ISBN + ".jpg");
                }
                return true;
            }
            string exeName = cover.FileName.Substring(cover.FileName.LastIndexOf(".") + 1);
            //如果文件后缀名不等于.jpg
            if (!exeName.ToLower().Equals("jpg"))
            {
                ModelState.AddModelError("", "文件格式必须为jpg");
                return false;
            }
            int size = cover.ContentLength;
            //如果大于10MB
            if (size / 1024 / 1024 > 10)
            {
                ModelState.AddModelError("", "文件不能大于10MB");
                return false;
            }
            //如果是修改图书，且修改了ISBN，就删除原来的图片
            if (book.Id != 0 && !oldISBN.Equals(book.ISBN))
            {
                FileInfo file = new FileInfo(path + oldISBN + ".jpg");
                if (file.Exists)
                {
                    file.Delete();//删除图片
                }
            }
            path += book.ISBN + "." + exeName;
            //上传文件
            cover.SaveAs(path);
            return true;
        }
        //处理编辑书籍和添加书籍
        [HttpPost]
        [ValidateInput(false)]//表示忽略过滤器
        public ActionResult Edit(Book book, HttpPostedFileBase cover)
        {
            if (ModelState.IsValid)
            {
                //检查标题与ISBN是否和重复
                if (IsExistsTitleAndISBN(book))
                {
                    //重新获得下拉列表数据
                    PrepareDropDownListData(book);
                    return View(book);
                }
                if (book.Id!=0)//修改
                {
                    //如果上传文件失败
                    if (!UploadFiles(book, cover))
                    {
                        TempData["message"] = "修改图书失败";
                        //重新获得下拉列表数据
                        PrepareDropDownListData(book);
                        return View(book);
                    }
                    try
                    {
                        bm.Update(book);
                        TempData["message"] = "修改图书成功";
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        TempData["message"] = "修改图书失败";
                        //重新获得下拉列表数据
                        PrepareDropDownListData(book);
                        return View(book);
                    }
                }
                else//新增
                {
                    if (UploadFiles(book, cover) && bm.Add(book) == 1)
                    {
                        TempData["message"] = "添加图书成功";
                        string info = "<script>location.href=confirm('添加图书成功，是否继续添加图书？')?'"+Url.Action("Edit")+"':'"+Url.Action("Index")+"';</script>";
                        return Content(info);
                    }
                    else
                    {
                        TempData["message"] = "添加图书失败";
                        //重新获得下拉列表数据
                        PrepareDropDownListData(book);
                        return View(book);
                    }
                }
            }
            else
            {
                //给出版社与分类添加错误提示
                if (book.Categorie.Id == 0)
                {
                    ModelState.AddModelError("Categorie","请选择图书分类");
                }
                if (book.Publisher.Id == 0)
                {
                    ModelState.AddModelError("Publisher", "请选择出版社");
                }
                //重新获得下拉列表数据
                PrepareDropDownListData(book);
                //判断标题、ISBN是否重复
                if (book != null)
                {
                    IsExistsTitleAndISBN(book);                    
                }
                return View(book);
            }
        }
        //删除书籍
        public ActionResult Delete(int id)
        {
            try
            {
                string path = Server.MapPath("~/Images/BookCovers/");//文件夹路径
                string ISBN = bm.GetBookById(id).ISBN;
                //从数据库删除数据
                bm.Delete(id);
                //删除图片
                FileInfo file = new FileInfo(path + ISBN + ".jpg");
                if (file.Exists)
                {
                    file.Delete();
                }
                TempData["message"] = "删除图书成功";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                string message = "出现的错误可能有：1、此图书在购物车表和订单明细表已有记录，暂时不能删除。";
                message += "2、数据库在执行删除操作时，出现异常。";
                Exception ex = new Exception(message);
                new ExceptionLogInfoAttribute().OnException(new ExceptionContext(this.ControllerContext, ex));
                return View("Error", new HandleErrorInfo(ex, @"Admin\Book", "Delete"));
            }
        }
    }
}
