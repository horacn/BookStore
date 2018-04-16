using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.BLL;

namespace BookStore.Controllers
{
    [ExceptionLogInfo]
    public class ShoppingCartController : Controller
    {
       //显示购物车列表
        [VisitNoteLogInfo]
        public ActionResult Index()
        {
            //根据请求上下文来获取购物车业务对象
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),//取出该用户的商品列表
                CartTotal = cart.GetTotal()//商品总金额
            };
            return View(viewModel);
        }
        //添加
        public ActionResult Add(int id)
        {
            Book book = new BookManager().GetBookById(id);
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(book);
            return RedirectToAction("Index", "ShoppingCart");
        }
        //根据记录Id删除
        public ActionResult Delete(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.RemoveFromCart(id);
            return RedirectToAction("Index", "ShoppingCart");
        }
        //提交订单
        public ActionResult Submit()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            if (cart.GetCount() == 0)
            {
                return Content("<script>alert('您的购物车为空，快去挑选您喜欢的图书吧！');location.href='"+Url.Action("Index","Home")+"';</script>");
            }
            else if(Session["User"] == null)
            {
                return Content("<script>alert('请您先登录！');location.href='" + Url.Action("Login", "Account", new { returnUrl = Url.Action("Index", "ShoppingCart") }) + "';</script>");
            }
            UserInfo user = Session["User"] as UserInfo;
            Order order = new Order
            {
                OrderDate = DateTime.Now,
                UserInfo = new UserInfo { Id=user.Id},
                TotalPrice = cart.GetTotal()
            };
            order.Id = new OrderManager().Add(order);
            cart.CreateOrder(order);
            Session.Remove(ShoppingCart.CartSessionKey);
            return Content("<script>alert('您的订单已结算成功，请耐心等待发货！');location.href='" + Url.Action("Index", "Home") + "';</script>");
        }
        //改变购物车商品数量，ajax调用
        [HttpPost]
        public ActionResult ChangeCount(int recordId,int count)
        { 
             var cart = ShoppingCart.GetCart(this.HttpContext);
             int result = cart.ChangeCount(recordId, count);
             if (result>0)
             {
                 return Content(cart.GetTotal().ToString());
             }
             else
             {
                 return Content("-1");
             }
        }
    }
}
