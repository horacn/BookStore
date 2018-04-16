using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BookStore.Models;

namespace BookStore.DAL
{
    public class OrderDetailService
    {
		#region 根据Id获得OrderDetail对象
		/// <summary>
		/// 根据Id获得OrderDetail对象
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public OrderDetail GetOrderDetailById(int id)
		{
			OrderDetail orderDetail = null;
			string sql = "select Id,OrderId,BookId,Quantity,UnitPrice from OrderDetails where Id = @Id";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Id", id));
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					orderDetail = LoadOrderDetail(reader);
				}
			}
			reader.Close();
			return orderDetail;
		} 
		#endregion

		#region 从SqlDataReader中读取数据，返回OrderDetail对象
		/// <summary>
		/// 从SqlDataReader中读取数据，返回OrderDetail对象
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private OrderDetail LoadOrderDetail(SqlDataReader reader)
		{
            //这里不能通过OrderService类GetOrderById()来获取Order对象，
            //因为GetOrderById()方法在获取Order对象时会调用此方法来获取订单详情集合，
            //这样一来就形成了死循环...
            Order order = new Order { Id = reader.GetInt32(1) };
            Book book = new BookService().GetBookById(reader.GetInt32(2));
            OrderDetail orderDetail = new OrderDetail(reader.GetInt32(0), order, book, reader.GetInt32(3), reader.GetDecimal(4)); 
            return orderDetail;
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
            string sql = "Insert Into OrderDetails Values(@OrderId,@BookId,@Quantity,@UnitPrice);select @@IDENTITY";
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@OrderId", SqlDbType.Int);
            parameters[0].Value = orderDeteil.Order.Id;
            parameters[1] = new SqlParameter("@BookId", SqlDbType.Int);
            parameters[1].Value = orderDeteil.Book.Id;
            parameters[2] = new SqlParameter("@Quantity", SqlDbType.Int);
            parameters[2].Value = orderDeteil.Quantity;
            parameters[3] = new SqlParameter("@UnitPrice", SqlDbType.Money);
            parameters[3].Value = orderDeteil.UnitPrice;
            return Convert.ToInt32(DBHelper.ExecuteScalar(sql, CommandType.Text, parameters));
        }
        #endregion

        #region 根据orderId获取订单详情集合
        /// <summary>
        /// 根据orderId获取订单详情集合
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IList<OrderDetail> GetOrderDetails(int orderId)
        {
            IList<OrderDetail> orderDetails = new List<OrderDetail>();
            string sql = "select Id,OrderId,BookId,Quantity,UnitPrice from OrderDetails where OrderId = @OrderId";
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@OrderId", orderId));
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    orderDetails.Add(LoadOrderDetail(reader));
                }
            }
            reader.Close();
            return orderDetails;
        } 
        #endregion

        #region 根据Id删除一个详情订单
        /// <summary>
        /// 根据Id删除一个详情订单
        /// </summary>
        /// <param name="id"></param>
        public void DeleteById(int id)
        {
            string sql = "Delete OrderDetails Where Id=@Id";
            DBHelper.ExecuteNonQuery(sql, CommandType.Text, new SqlParameter("@Id", id));
        } 
        #endregion

        #region 根据orderId删除一个订单下的所有详情订单
        /// <summary>
        /// 根据orderId删除一个订单下的所有详情订单
        /// </summary>
        /// <param name="orderId"></param>
        public void DeleteByOrderId(int orderId)
        {
            string sql = "Delete OrderDetails Where OrderId=@OrderId";
            DBHelper.ExecuteNonQuery(sql, CommandType.Text, new SqlParameter("@OrderId", orderId));
        }
        #endregion

        #region 清空所有详情订单
        /// <summary>
        /// 清空所有详情订单
        /// </summary>
        public void EmptyAll()
        {
            string sql = "Delete OrderDetails";
            DBHelper.ExecuteNonQuery(sql, CommandType.Text);
        }
        #endregion
    }
}
