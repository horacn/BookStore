using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.BLL;

namespace BookStore.Models
{
    /// <summary>
    /// 购物车业务类
    /// </summary>
    public class ShoppingCart
    {
        /// <summary>
        /// 购物编号
        /// </summary>
        public string ShoppingCartId { get; set; }

        /// <summary>
        /// Session键名
        /// </summary>
        public const string CartSessionKey = "CartId";

        private CartManager cm = new CartManager();

        #region 根据用户的请求，获取购物车id
        /// <summary>
        /// 根据用户的请求，获取Session的信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                //对于没有登录的用户，我们需要为他们创建一个临时的唯一标识，这里使用CUID，全局唯一标识符，
                //对于已经登陆的用户，我们直接使用他们的名称，保存在Session中
                //if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                //{
                //    context.Session[CartSessionKey] = context.User.Identity.Name;
                //}
                if (context.Session["User"] != null)
                {
                    context.Session[CartSessionKey] = (context.Session["User"] as UserInfo).LoginId;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        } 
        #endregion

        #region 创建购物车对象
        /// <summary>
        /// 创建购物车对象
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        } 
        #endregion

        public static ShoppingCart GetCart(Controller control)
        {
            return GetCart(control.HttpContext);
        }

        #region 将图书添加到购物车
        /// <summary>
        /// 将书籍作为参数加入到购物车中，在Cart表中跟踪每个书辑的数量
        /// 在这个方法中，我们将会检查是在表中增加一行，还是仅仅在用户已经选择的书辑上增加数量
        /// </summary>
        /// <param name="book"></param>
        public void AddToCart(Book book)
        {
            //根据cartId和购买书籍的编号获取购物车对象
            var cartItem = cm.GetCart(ShoppingCartId,book.Id);
            //如果没有该条信息，则新增一条
            if (cartItem == null)
            {
                cartItem = new Cart 
                { 
                    Book = new Book{Id=book.Id},
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                //添加Cart
                cm.Add(cartItem);
            }
            else
            {
                //如果存在，则改变该商品购买的数量
                cartItem.Count++;
                cm.UpdateCount(cartItem.RecordId, cartItem.Count);
            }
        } 
        #endregion

        #region 根据购物车id从购物车中删除一项
        /// <summary>
        /// 根据购物车id从购物车中删除一项
        /// </summary>
        /// <param name="recordId"></param>
        public void RemoveFromCart(int recordId)
        {
            cm.Delete(recordId);
        } 
        #endregion

        #region 更新数量
        /// <summary>
        /// 更新数量（ajax修改购物车数量时调用）
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int ChangeCount(int recordId, int count)
        {
            //根据购物车编号获取购物车对象
            var cartItem = cm.GetCart(recordId);
            if (cartItem != null)
            {
                cm.UpdateCount(recordId, count);
                return 1;
            }
            else
            {
                return -1;
            }
        } 
        #endregion

        #region 删除用户购物车中所有的项目
        /// <summary>
        /// 删除用户购物车中所有的项目
        /// </summary>
        public void EmptyCart()
        {
            cm.DeleteByCartId(ShoppingCartId);
        } 
        #endregion

        #region 获取购物车项目的列表用来显示或处理
        /// <summary>
        /// 获取购物车项目的列表用来显示或处理
        /// </summary>
        /// <returns></returns>
        public IList<Cart> GetCartItems()
        {
            return cm.GetCartsByCartId(ShoppingCartId);
        } 
        #endregion

        #region 获取购物车商品数量
        /// <summary>
        /// 获取购物车商品数量
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return cm.GetCartCount(ShoppingCartId);
        } 
        #endregion

        #region 获取购物车商品总价
		/// <summary>
        /// 获取购物车商品总价
        /// </summary>
        /// <returns></returns>
        public decimal GetTotal()
        {
            return cm.GetAcrtAmount(ShoppingCartId);
        }
        #endregion

        #region 提交订单
        /// <summary>
        /// 将购物车转换为结账处理过程中的订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int CreateOrder(Order order)
        {
            //获取所有的购物车商品列表
            var cartItems = GetCartItems();
            //遍历购物车中的商品，增加订单的详细信息
            foreach (var cart in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Book = new Book { Id = cart.Book.Id },
                    Order = new Order { Id = order.Id },
                    UnitPrice = cart.Book.UnitPrice,
                    Quantity = cart.Count
                };
                //添加一项详细订单
                new OrderDetailManager().Add(orderDetail);
                //增加图书销售次数
                new BookManager().UpdateClicks(cart.Book.Id,cart.Count);
            }
            //清空购物车
            EmptyCart();
            //返回订单号
            return order.Id;
        } 
        #endregion

        #region 当用户已经登录，将他们的购物车与用户名相关联
        /// <summary>
        /// 当用户已经登录，将他们的购物车与用户名相关联
        /// </summary>
        /// <param name="userName"></param>
        public void MigrateCart(string userName)
        {
            //获取所有cartId为用户名的购物车商品列表
            var cartItems_userName = cm.GetCartsByCartId(userName);
            //获取所有cartId为Guid的购物车商品列表
            var cartItems_Guid = cm.GetCartsByCartId(ShoppingCartId);
            //遍历集合，如果存在购物车里相同的两本书，删除cartId为Guid的那本书，再修改cartId为用户名的书籍购买数量
            foreach (var cu in cartItems_userName)
            {
                foreach (var cg in cartItems_Guid)
                {
                    if (cg.Book.Id==cu.Book.Id)
                    {
                        //删除一项
                        cm.Delete(cg.RecordId);
                        //修改购买数量
                        cm.UpdateCount(cu.RecordId,cu.Count+cg.Count);
                    }
                }
            }
            cm.UpdateCartId(ShoppingCartId, userName);
        } 
        #endregion
    }
}