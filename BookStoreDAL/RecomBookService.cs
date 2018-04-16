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
    public class RecomBookService
    {
		#region 根据Id获得RecomBook对象
		/// <summary>
		/// 根据Id获得RecomBook对象
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public RecomBook GetRecomBookById(int id)
		{
			RecomBook recomBook = null;
			string sql = "select Id,BookId,UserId from RecomBooks where Id = @Id";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Id", id));
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					recomBook = LoadRecomBook(reader);
				}
			}
			reader.Close();
			return recomBook;
		} 
		#endregion

		#region 从SqlDataReader中读取数据，返回RecomBook对象
		/// <summary>
		/// 从SqlDataReader中读取数据，返回RecomBook对象
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private RecomBook LoadRecomBook(SqlDataReader reader)
		{
			Book book = new BookService().GetBookById(reader.GetInt32(1));
			UserInfo user = new UserService().GetUserInfoById(reader.GetInt32(2));
			RecomBook recomBook = new RecomBook(reader.GetInt32(0), book, user);
			return recomBook;
		} 
		#endregion
    }
}
