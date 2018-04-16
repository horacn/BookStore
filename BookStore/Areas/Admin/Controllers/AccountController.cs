using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.BLL;
using System.Web.Security;

namespace BookStore.Areas.Admin.Controllers
{
    [ExceptionLogInfo]
    public class AccountController : Controller
    {
        private UserManager um = new UserManager();

        //显示登录页
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //在用户注册登录之后用用户的登录名替换原来匿名的GUID
        [NonAction]
        private void MigrateShoppingCart(string userName)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            if (cart.ShoppingCartId == null || cart.ShoppingCartId == userName)
            {
                return;
            }
            cart.MigrateCart(userName);
            Session[ShoppingCart.CartSessionKey] = userName;
        }
        //处理登录请求
        [HttpPost]
        [LoginNoteLogInfo]
        public ActionResult Login(LoginInfoModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserInfo user = um.GetUserInfoByLoginId(model.LoginId.Trim());
                if (user == null)
                {
                    ModelState.AddModelError("LoginId", "用户名不存在");
                }
                else
                {
                    if (user.LoginPwd.Trim().ToLower().Equals(model.LoginPwd.Trim().ToLower()) && user.UserRole.Name == "管理员" && user.UserState.Name == "正常")
                    {
                        Session["User"] = user;
                        //用用户的登录名替换原来匿名的GUID
                        MigrateShoppingCart(user.LoginId);
                        //记住凭据
                        FormsAuthentication.SetAuthCookie(user.LoginId,false);
                        if (returnUrl != null)
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("LoginPwd", "您输入的密码有误");
                    }
                }
            }
            return View(model);
        }
        //退出
        [Authorize]
        [CheckOutNoteLogInfo]
        public ActionResult CheckOut()
        {
            //删除凭据
            FormsAuthentication.SignOut();
            Session.Abandon();
            delCookie();
            return RedirectToAction("Index","../Home");
        }
        //删除cookie
        [NonAction]
        private void delCookie()
        {
            //使cookie失效
            HttpCookie ck_loginId = Request.Cookies["ck_loginId"];
            if (ck_loginId != null)
            {
                ck_loginId.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ck_loginId);
            }
        }
    }
}
