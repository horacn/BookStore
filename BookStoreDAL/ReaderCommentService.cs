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
    public class ReaderCommentService
    {
		#region 根据Id获得ReaderComment对象
		/// <summary>
		/// 根据Id获得ReaderComment对象
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ReaderComment GetReaderCommentById(int id)
		{
			ReaderComment readerComment = null;
			string sql = "select Id,BookId,ReaderName,Title,Comment,Date from ReaderComments where Id = @Id";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Id", id));
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					readerComment = LoadReaderComment(reader);
				}
			}
			reader.Close();
			return readerComment;
		} 
		#endregion

		#region 从SqlDataReader中读取数据，返回ReaderComment对象
		/// <summary>
		/// 从SqlDataReader中读取数据，返回ReaderComment对象
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private ReaderComment LoadReaderComment(SqlDataReader reader)
		{
			Book book = new BookService().GetBookById(reader.GetInt32(1));
			ReaderComment readerComment = new ReaderComment(reader.GetInt32(0), book, reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetDateTime(5));
			return readerComment;
		} 
		#endregion
    }
}
