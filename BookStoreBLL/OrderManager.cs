using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DAL;
using BookStore.Models;

namespace BookStore.BLL
{
    public class OrderManager
    {
        private OrderService os = new OrderService();

        #region 根据Id获得Order对象
        /// <summary>
        /// 根据Id获得Order对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Order GetOrderById(int id)
        {
            return os.GetOrderById(id);
        } 
        #endregion

        #region 添加订单
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int Add(Order order)
        {
            return os.Add(order);
        }
        #endregion

        #region 获取所有订单
        /// <summary>
        /// 获取所有订单
        /// </summary>
        /// <returns></returns>
        public IList<Order> GetOrdersAll()
        {
            return os.GetOrdersAll();
        }
        #endregion

        #region 获取一个用户的所有订单
        /// <summary>
        /// 获取一个用户的所有订单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Order> GetOrdersByUserId(int userId)
        {
            return os.GetOrdersByUserId(userId);
        }
        #endregion

        #region 根据Id删除一个订单
        /// <summary>
        /// 根据Id删除一个订单
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            new OrderDetailService().DeleteByOrderId(id);
            os.Delete(id);
        }
        #endregion

        #region 清空一个用户的所有订单
        /// <summary>
        /// 清空一个用户的所有订单
        /// </summary>
        /// <param name="orderId"></param>
        public void EmptyOrdersByUserId(int userId)
        {
            var orders = GetOrdersByUserId(userId);
            OrderDetailService ods = new OrderDetailService();
            foreach (var o in orders)
            {
                ods.DeleteByOrderId(o.Id);
            }
            os.EmptyOrdersByUserId(userId);
        }
        #endregion

        #region 清空所有订单
        /// <summary>
        /// 清空所有订单
        /// </summary>
        public void EmptyAll()
        {
            new OrderDetailService().EmptyAll();
            os.EmptyAll();
        }
        #endregion
    }
}
