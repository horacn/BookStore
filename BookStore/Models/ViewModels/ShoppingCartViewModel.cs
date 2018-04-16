    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    /// <summary>
    /// 用户的购物车显示
    /// </summary>
    public class ShoppingCartViewModel
    {
        /// <summary>
        /// 购物车集合
        /// </summary>
        public IList<Cart> CartItems { get; set; }
        /// <summary>
        /// 购物总价
        /// </summary>
        public decimal CartTotal { get; set; }
    }
}