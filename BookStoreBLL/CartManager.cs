using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DAL;
using BookStore.Models;

namespace BookStore.BLL
{
    public class CartManager
    {
        private CartService cs = new CartService();

        #region 新增购物车
        /// <summary>
        /// 新增购物车
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public int Add(Cart cart)
        {
            return cs.Add(cart);
        }
        #endregion

        #region 根据cartId和bookId获取对象
        /// <summary>
        /// 根据cartId和bookId获取对象
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public Cart GetCart(string cartId, int bookId)
        {
            return cs.GetCart(cartId, bookId);
        }
        #endregion

        #region 根据recordId获得Cart对象
        /// <summary>
        /// 根据recordId获得Cart对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cart GetCart(int recordId)
        {
            return cs.GetCart(recordId);
        }
        #endregion

        #region 更新数量为count
        /// <summary>
        /// 更新数量为count
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="count"></param>
        public void UpdateCount(int recordId, int count)
        {
            cs.UpdateCount(recordId,count);
        }
        #endregion

        #region 根据recordId删除
        /// <summary>
        /// 根据recordId删除
        /// </summary>
        /// <param name="recordId"></param>
        public void Delete(int recordId)
        {
            cs.Delete(recordId);
        }
        #endregion

        #region 根据cartId删除
        /// <summary>
        /// 根据cartId删除
        /// </summary>
        /// <param name="cartId"></param>
        public void DeleteByCartId(string cartId)
        {
            cs.DeleteByCartId(cartId);
        }
        #endregion

        #region 根据cartId查询列表
        /// <summary>
        /// 根据cartId查询列表
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public IList<Cart> GetCartsByCartId(string cartId)
        {
            return cs.GetCartsByCartId(cartId);
        }
        #endregion

        #region 获取用户购物车中商品的数量
        /// <summary>
        /// 获取用户购物车中商品的数量
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public int GetCartCount(string cartId)
        {
            return cs.GetCartCount(cartId);
        }
        #endregion

        #region 获取购物车中商品的总价
        /// <summary>
        /// 获取购物车中商品的总价
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public decimal GetAcrtAmount(string cartId)
        {
            return cs.GetAcrtAmount(cartId);
        }
        #endregion

        #region 当用户已经登录，将他们的购物车与用户名相关联
        /// <summary>
        /// 当用户已经登录，将他们的购物车与用户名相关联
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="userName"></param>
        public void UpdateCartId(string cartId, string userName)
        {
            cs.UpdateCartId(cartId,userName);
        }
        #endregion
    }
}
