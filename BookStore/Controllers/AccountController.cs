using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.BLL;
using BookStore.Models;
using BookStore.Helpers;
using System.Web.Security;

namespace BookStore.Controllers
{
    [ExceptionLogInfo]
    public class AccountController : Controller
    {
        private UserManager um = new UserManager();

        //显示登录页
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
			if (returnUrl==null)
			{
				returnUrl = Url.Action("Index","Home");	
			}
            ViewBag.ReturnUrl = returnUrl;//登录成功后要跳转的页面
            return View();
        }
		//处理登录请求
        [HttpPost]
        [LoginNoteLogInfo]
        public ActionResult Login(LoginInfoModel model,string returnUrl)
		{
            //先判断是否通过验证特性
            if (ModelState.IsValid)
            {
                UserInfo user = um.GetUserInfoByLoginId(model.LoginId.Trim());
                if (user == null)
                {
                    ModelState.AddModelError("LoginId", "用户名不存在");
                }
                else
                {
                    if (user.LoginPwd.Trim().ToLower().Equals(model.LoginPwd.Trim().ToLower()) && user.UserState.Name == "正常")
                    {
                        Session["User"] = user;
                        if (Request.Form["chk"] != null)
                        {
                            HttpCookie ck_loginId = new HttpCookie("ck_loginId", model.LoginId);
                            ck_loginId.Expires = DateTime.MaxValue;
                            Response.Cookies.Add(ck_loginId);
                        }
                        else
                        {
                            delCookie();
                        }
                        //用用户的登录名替换原来匿名的GUID
                        MigrateShoppingCart(user.LoginId);
                        //如果是管理员登录，记住凭据
                        if (user.UserRole.Name == "管理员")
                        {
                            FormsAuthentication.SetAuthCookie(user.LoginId,false);
                        }
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("LoginPwd", "您输入的密码有误");
                    }
                }
            }
            ViewBag.ReturnUrl = returnUrl;//登录成功后要跳转的页面
            return View(model);
		}
        //用户希望继续使用原来的购物车，所以，在匿名用户登录之后，我们需要维护购物车。
        //ShoppingCart类已经提供了一个方法，通过当前的用户名来获取购物车中的所有项目
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
		//退出
        [CheckUserIsNull(Order=1)]
        [CheckOutNoteLogInfo(Order=2)]
		public ActionResult CheckOut()
		{
            if (Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if ((Session["User"] as UserInfo).UserRole.Name == "管理员")
            {
                //删除凭据
                FormsAuthentication.SignOut();
            }
			//Session["User"] = null;
			//Session.Remove("User");
			Session.Abandon();
			delCookie();
            return RedirectToAction("Login", "Account");
		}
        //删除cookie
        [NonAction]//表示这只是个普通方法，不是动作方法
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
        //显示注册页
        public ActionResult Register()
        {
            return View();
        }
        //检查是否重复用户名(ajax)
        public ActionResult CheckLoginIdIsExists(string loginId)
        {
            if (um.Exists(loginId.Trim()))
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }
        //处理注册
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                bool flag = false;//是否未通过
                //验证码是否正确
                if (!model.SecurityCode.ToUpper().Equals(TempData["SecurityCode"]))
                {
                    ModelState.AddModelError("SecurityCode","验证码输入有误");
                    flag = true;
                }
                //用户名是否重复
                if (um.Exists(model.LoginId))
                {
                    ModelState.AddModelError("LoginId","用户名已存在");
                    flag = true;
                }
                if (flag)
                {
                    return View(model);
                }
                string ip = string.Empty;
                //服务端获取IP地址
                //方法一
                //ip = Request.UserHostAddress;
                //方法二
                ip = Request.ServerVariables["REMOTE_ADDR"];
                ////方法三
                //string strHostName = System.Net.Dns.GetHostName();
                //ip = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                ////方法四（无视代理）
                //ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                UserInfo user = new UserInfo
                {
                    LoginId = model.LoginId.Trim(),
                    LoginPwd = model.LoginPwd.Trim().ToLower(),
                    Name = model.Name.Trim(),
                    Phone = model.Phone.Trim(),
                    Birthday = model.Birthday,
                    Mail = model.Mail.Trim(),
                    Address = model.Address.Trim(),
                    RegisterIp = ip
                };
                int id = um.Add(user);
                //如果插入成功，跳转至登录页
                if (id>0)
                {
                    return Content("<script>alert('注册成功，现在跳转到登录页面');location.href='" + Url.Action("Login", "Account") + "';</script>");
                }
                return View(model);
            }
            else
            {
                if (model!=null)
                {
                    //验证码是否正确
                    if (model.SecurityCode!=null && !model.SecurityCode.ToUpper().Equals(TempData["SecurityCode"]))
                    {
                        ModelState.AddModelError("SecurityCode", "验证码输入有误");
                    }
                    //用户名是否重复
                    if (model.LoginId != null && um.Exists(model.LoginId))
                    {
                        ModelState.AddModelError("LoginId", "用户名已存在");
                    }
                }
                return View(model);
            }
        }
        #region  生成验证码图片
        public ActionResult SecurityCode()
        {
            SecurityCode sc = new SecurityCode();
            string oldcode = TempData["SecurityCode"] as string;
            string code = sc.CreateRandomCode(5);
            TempData["SecurityCode"] = code;
            return File(sc.CreateValidateGraphic(code), "image/Jpeg");
        }
        #endregion
    }
}
