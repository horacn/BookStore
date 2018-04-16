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
    public class BookRatingService
    {
		#region 根据Id获得BookRating对象
		/// <summary>
		/// 根据Id获得BookRating对象
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public BookRating GetBookRatingById(int id)
		{
			BookRating bookRating = null;
			string sql = "select Id,BookId,UserId,Rating,Comment,CreatedTime from BookRatings where Id = @Id";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Id", id));
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					bookRating = LoadBookRating(reader);
				}
			}
			reader.Close();
			return bookRating;
		} 
		#endregion

		#region 从SqlDataReader中读取数据，返回BookRating对象
		/// <summary>
		/// 从SqlDataReader中读取数据，返回BookRating对象
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private BookRating LoadBookRating(SqlDataReader reader)
		{
			Book book = new BookService().GetBookById(reader.GetInt32(1));
			UserInfo user = new UserService().GetUserInfoById(reader.GetInt32(2));
			BookRating bookRating = new BookRating(reader.GetInt32(0), book, user, reader.GetDateTime(5));
			if (!(reader["Rating"] is DBNull))
			{
				bookRating.Rating = reader.GetInt32(3);
			}
			else
			{
				bookRating.Rating = 0;
			}
			if (!(reader["Comment"] is DBNull))
			{
				bookRating.Comment = reader.GetString(4);
			}
			else
			{
				bookRating.Comment = null;
			}
			return bookRating;
		} 
		#endregion
    }
}
