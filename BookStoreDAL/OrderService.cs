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
    public class OrderService
    {
		#region 根据Id获得Order对象
		/// <summary>
		/// 根据Id获得Order对象
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Order GetOrderById(int id)
		{
			Order order = null;
			string sql = "select Id,OrderDate,UserId,TotalPrice from Orders where Id = @Id";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Id", id));
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					order = LoadOrder(reader);
				}
			}
			reader.Close();
			return order;
		} 
		#endregion

		#region 从SqlDataReader中读取数据，返回Order对象
		/// <summary>
		/// 从SqlDataReader中读取数据，返回Order对象
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private Order LoadOrder(SqlDataReader reader)
		{
			UserInfo user = new UserService().GetUserInfoById(reader.GetInt32(2));
            IList<OrderDetail> orderDetails = new OrderDetailService().GetOrderDetails(reader.GetInt32(0));
            Order order = new Order(reader.GetInt32(0), reader.GetDateTime(1), user, reader.GetDecimal(3),orderDetails);
			return order;
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
            string sql = "Insert Into Orders Values(@OrderDate,@UserId,@TotalPrice);select @@IDENTITY";
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@OrderDate",SqlDbType.DateTime);
            parameters[0].Value = order.OrderDate;
            parameters[1] = new SqlParameter("@UserId", SqlDbType.Int);
            parameters[1].Value = order.UserInfo.Id;
            parameters[2] = new SqlParameter("@TotalPrice", SqlDbType.Money);
            parameters[2].Value = order.TotalPrice;
            return Convert.ToInt32(DBHelper.ExecuteScalar(sql,CommandType.Text,parameters));
        } 
        #endregion

        #region 获取所有订单
        /// <summary>
        /// 获取所有订单
        /// </summary>
        /// <returns></returns>
        public IList<Order> GetOrdersAll()
        {
            IList<Order> orders = new List<Order>();
            string sql = "select Id,OrderDate,UserId,TotalPrice from Orders Order By OrderDate Desc";
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    orders.Add(LoadOrder(reader));
                }
            }
            reader.Close();
            return orders;
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
            IList<Order> orders = new List<Order>();
            string sql = "select Id,OrderDate,UserId,TotalPrice from Orders where UserId=@UserId Order By OrderDate Desc";
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text,new SqlParameter("@UserId",userId));
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    orders.Add(LoadOrder(reader));
                }
            }
            reader.Close();
            return orders;
        }
        #endregion

        #region 根据Id删除一个订单
        /// <summary>
        /// 根据Id删除一个订单
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            string sql = "Delete Orders Where Id=@Id";
            DBHelper.ExecuteNonQuery(sql, CommandType.Text, new SqlParameter("@Id", id));
        }
        #endregion

        #region 清空一个用户的所有订单
        /// <summary>
        /// 清空一个用户的所有订单
        /// </summary>
        /// <param name="orderId"></param>
        public void EmptyOrdersByUserId(int userId)
        {
            string sql = "Delete Orders Where UserId=@UserId";
            DBHelper.ExecuteNonQuery(sql, CommandType.Text, new SqlParameter("@UserId", userId));
        }
        #endregion

        #region 清空所有订单
        /// <summary>
        /// 清空所有订单
        /// </summary>
        public void EmptyAll()
        {
            string sql = "Delete Orders";
            DBHelper.ExecuteNonQuery(sql, CommandType.Text);
        }
        #endregion
    }
}
