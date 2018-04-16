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
    /// <summary>
    /// 购物车数据库服务类
    /// </summary>
    public class CartService
    {
        #region 新增购物车
        /// <summary>
        /// 新增购物车
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public int Add(Cart cart)
        {
            string sql = "Insert Carts Values(@CartId,@BookId,@Count,@DateCreated)";
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@CartId",SqlDbType.VarChar,50);
            parameters[0].Value = cart.CartId;
            parameters[1] = new SqlParameter("@BookId", SqlDbType.Int);
            parameters[1].Value = cart.Book.Id;
            parameters[2] = new SqlParameter("@Count", SqlDbType.Int);
            parameters[2].Value = cart.Count;
            parameters[3] = new SqlParameter("@DateCreated", SqlDbType.DateTime);
            parameters[3].Value = cart.DateCreated;
            return Convert.ToInt32(DBHelper.ExecuteScalar(sql,CommandType.Text,parameters));
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
            Cart cart = null;
            string sql = "select RecordId,CartId,BookId,Count,DateCreated from Carts where CartId = @CartId And BookId = @BookId";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@CartId", cartId),
                new SqlParameter("@BookId",bookId)
            };
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, parameters);
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    cart = LoadCart(reader);
                }
            }
            reader.Close();
            return cart;
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
			Cart cart = null;
			string sql = "select RecordId,CartId,BookId,Count,DateCreated from Carts where RecordId = @Id";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Id", recordId));
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					cart = LoadCart(reader);
				}
			}
			reader.Close();
			return cart;
		} 
		#endregion

		#region 从SqlDataReader中读取数据，返回Cart对象
		/// <summary>
		/// 从SqlDataReader中读取数据，返回Cart对象
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private Cart LoadCart(SqlDataReader reader)
		{
			Book book = new BookService().GetBookById(reader.GetInt32(2));
			Cart cart = new Cart(reader.GetInt32(0), reader.GetString(1), book, reader.GetInt32(3), reader.GetDateTime(4));
			return cart;
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
            string sql = "Update Carts Set Count=@Count Where RecordId=@RecordId";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@Count",count),
                new SqlParameter("@RecordId",recordId)
            };
            DBHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
        } 
        #endregion

        #region 根据recordId删除
        /// <summary>
        /// 根据recordId删除
        /// </summary>
        /// <param name="recordId"></param>
        public void Delete(int recordId)
        {
            string sql = "Delete Carts Where RecordId=@RecordId";
            DBHelper.ExecuteNonQuery(sql, CommandType.Text,  new SqlParameter("@RecordId",recordId));
        } 
        #endregion

        #region 根据cartId删除
        /// <summary>
        /// 根据cartId删除
        /// </summary>
        /// <param name="cartId"></param>
        public void DeleteByCartId(string cartId)
        {
            string sql = "Delete Carts Where CartId=@CartId";
            DBHelper.ExecuteNonQuery(sql, CommandType.Text, new SqlParameter("@CartId", cartId));
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
            IList<Cart> carts = new List<Cart>();
            string sql = "select RecordId,CartId,BookId,Count,DateCreated from Carts where CartId = @CartId";
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@CartId", cartId));
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    carts.Add(LoadCart(reader));
                }
            }
            reader.Close();
            return carts;
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
            string sql = "Select Sum(Count) From Carts Where CartId=@CartId";
            object result = DBHelper.ExecuteScalar(sql, CommandType.Text, new SqlParameter("@CartId", cartId));
            if (result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }
            return 0;
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
            string sql = "select sum(b.UnitPrice*c.Count) from Carts as c inner join Books as b on c.BookId=b.Id where CartId=@CartId ";
            object result =  DBHelper.ExecuteScalar(sql, CommandType.Text, new SqlParameter("@CartId", cartId));
            if (result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }
            return 0.00M;
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
            string sql = "Update Carts Set CartId=@UserName Where CartId=@CartId";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@UserName",userName),
                new SqlParameter("@CartId",cartId)
            };
            DBHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
        } 
        #endregion
    }
}
