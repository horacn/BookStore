using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    /// <summary>
    /// 订单明细
    /// </summary>
    [Serializable]
    public class OrderDetail
    {
        /// <summary>
        /// 明细编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 订单对象
        /// </summary>
        public Order Order { get; set; }
        /// <summary>
        /// 书籍
        /// </summary>
        public Book Book { get; set; }
        /// <summary>
        /// 产品数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        public OrderDetail() { }
        public OrderDetail(int id, Order order, Book book, int quantity, decimal unitPrice)
        {
            this.Id = id;
            this.Order = order;
            this.Book = book;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
        }
    }
}
