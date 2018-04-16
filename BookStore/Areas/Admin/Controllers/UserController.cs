using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.BLL;
using BookStore.Helpers;

namespace BookStore.Areas.Admin.Controllers
{
    [Authorize]
    [ExceptionLogInfo]
    public class UserController : Controller
    {
        private UserManager um = new UserManager();

        //显示所有用户
        public ActionResult Index(int pageIndex = 1)
        {
            int pageSize = 10;
            var users = um.GetUserInfosAll();
            var pagedUsers = new PagedList<UserInfo>(users, pageSize, pageIndex);
            return View(pagedUsers);
        }
        //删除用户
        public ActionResult Delete(int id)
        {
            try
            {
                um.Delete(id);
                TempData["message"] = "删除用户成功";
                return RedirectToAction("Index", "User");
            }
            catch (Exception)
            {
                string message = "出现的错误可能有：1、此用户在订单表已有记录，暂时不能删除。";
                message += "2、数据库在执行删除操作时，出现异常。";
                Exception ex = new Exception (message);
                new ExceptionLogInfoAttribute().OnException(new ExceptionContext(this.ControllerContext,ex));
                return View("Error", new HandleErrorInfo(ex, @"Admin\User", "Delete"));
            }
        }
        //显示编辑用户页面
        public ActionResult Edit(int id)
        {
            var user = um.GetUserInfoById(id);
            EditUserModel model = new EditUserModel { 
                Id = user.Id,
                LoginId = user.LoginId,
                Name = user.Name,
                Address = user.Address,
                Mail = user.Mail,
                Phone = user.Phone,
                Birthday = user.Birthday
            };
            return View(model);
        }
        //编辑用户
        [HttpPost]
        public ActionResult Edit(EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserInfo { 
                    Id = model.Id,
                    Mail = model.Mail.Trim(),
                    Name = model.Name.Trim(),
                    Phone = model.Phone.Trim(),
                    Address = model.Address.Trim(),
                    Birthday = model.Birthday
                };
                um.Update(user);
                TempData["message"] = "修改用户信息成功";
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
    }
}
