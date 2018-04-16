using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    /// <summary>
    /// 购物车
    /// </summary>
    [Serializable]
    public class Cart
    {
        /// <summary>
        /// 购物车的记录编号，唯一标识
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string CartId { get; set; }
        /// <summary>
        /// 书籍
        /// </summary>
        public Book Book { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime DateCreated { get; set; }

        public Cart() { }
        public Cart(int recordId, string cartId, Book book, int count, DateTime dateCreated)
        {
            this.RecordId = recordId;
            this.CartId = cartId;
            this.Book = book;
            this.Count = count;
            this.DateCreated = dateCreated;
        }
    }
}
