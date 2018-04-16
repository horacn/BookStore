using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    /// <summary>
    /// 订单
    /// </summary>
    [Serializable]
    public class Order
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// 订单的所有者
        /// </summary>
        public UserInfo UserInfo { get; set; }
        /// <summary>
        /// 总价格
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 订单详情集合
        /// </summary>
        public IList<OrderDetail> OrderDetails { get; set; }

        public Order() { }
        public Order(int id, DateTime orderDate, UserInfo userInfo, decimal totalPrice,IList<OrderDetail> orderDetails) 
        {
            this.Id = id;
            this.OrderDate = orderDate;
            this.UserInfo = userInfo;
            this.TotalPrice = totalPrice;
            this.OrderDetails = orderDetails;
        }
    }
}
