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
    [CheckUserIsNull(Order=1)]
    [ExceptionLogInfo(Order=2)]
    public class OrderController : Controller
    {

        private OrderManager om = new OrderManager();

        //显示用户订单首页
        public ActionResult Index(int pageIndex = 1)
        {
            int uid = (Session["User"] as UserInfo).Id;
            int pageSize = 5;
            var orders = om.GetOrdersByUserId(uid);
            decimal totalMoneys = 0;//所有订单总额
            foreach (var o in orders)
            {
                totalMoneys += o.TotalPrice;
            }
            ViewBag.TotalMoneys = totalMoneys;
            var pagedOrders = new PagedList<Order>(orders, pageSize, pageIndex);
            return View(pagedOrders);
        }

        //删除一个详情订单
        public ActionResult DeleteOrderDetail(int id)
        {
            try
            {
                new OrderDetailManager().DeleteById(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                string message = "数据库在执行删除详细订单操作时，出现异常。";
                Exception ex = new Exception(message);
                new ExceptionLogInfoAttribute().OnException(new ExceptionContext(this.ControllerContext, ex));
                return View("Error", new HandleErrorInfo(ex, "Order", "DeleteOrderDetail"));
            }
        }

        //删除一个订单
        public ActionResult DeleteOrder(int id)
        {
            try
            {
                om.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                string message = "数据库在执行删除订单操作时，出现异常。";
                Exception ex = new Exception(message);
                new ExceptionLogInfoAttribute().OnException(new ExceptionContext(this.ControllerContext, ex));
                return View("Error", new HandleErrorInfo(ex, "Order", "DeleteOrder"));
            }
        }

        //清空该用户所有的订单
        public ActionResult EmptyUsersOrder()
        {
            try
            {
                int uid = (Session["User"] as UserInfo).Id;
                om.EmptyOrdersByUserId(uid);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                string message = "数据库在执行清空用户所有的订单操作时，出现异常。";
                Exception ex = new Exception(message);
                new ExceptionLogInfoAttribute().OnException(new ExceptionContext(this.ControllerContext, ex));
                return View("Error", new HandleErrorInfo(ex, "Order", "EmptyUsersOrder"));
            }
        }
    }
}
