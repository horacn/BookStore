using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DAL;
using BookStore.Models;

namespace BookStore.BLL
{
    public class OrderDetailManager
    {
        private OrderDetailService ods = new OrderDetailService();

        #region 根据Id获得OrderDetail对象
        /// <summary>
        /// 根据Id获得OrderDetail对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OrderDetail GetOrderDetailById(int id)
        {
            return ods.GetOrderDetailById(id);
        } 
        #endregion

        #region 添加订单明细
        /// <summary>
        /// 添加订单明细
        /// </summary>
        /// <param name="orderDeteil"></param>
        /// <returns></returns>
        public int Add(OrderDetail orderDeteil)
        {
            return ods.Add(orderDeteil);
        }
        #endregion

        #region 根据Id删除一个详情订单
        /// <summary>
        /// 根据Id删除一个详情订单
        /// </summary>
        /// <param name="id"></param>
        public void DeleteById(int id)
        {
            ods.DeleteById(id);
        }
        #endregion

    }
}
