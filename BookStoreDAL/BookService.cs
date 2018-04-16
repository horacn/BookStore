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
    public class BookService
    {
		#region 新增图书
		/// <summary>
		/// 新增图书
		/// </summary>
		/// <param name="book"></param>
		/// <returns></returns>
		public int Add(Book book)
		{
			string sql = "insert Into Books Values(@Title,@Author,@PublisherId,@PublishDate,@ISBN,@UnitPrice,@ContentDescription,@TOC,@CategoryId,0)";
			SqlParameter[] parameters = new SqlParameter[9];
			parameters[0] = new SqlParameter("@Title", SqlDbType.NVarChar, 200);
			parameters[0].Value = book.Title;
			parameters[1] = new SqlParameter("@Author", SqlDbType.NVarChar, 200);
			parameters[1].Value = book.Author;
			parameters[2] = new SqlParameter("@PublisherId", SqlDbType.Int);
			parameters[2].Value = book.Publisher.Id;
			parameters[3] = new SqlParameter("@PublishDate", SqlDbType.DateTime);
			parameters[3].Value = book.PublishDate;
			parameters[4] = new SqlParameter("@ISBN", SqlDbType.NVarChar, 50);
			parameters[4].Value = book.ISBN;
			parameters[5] = new SqlParameter("@UnitPrice", SqlDbType.Money);
			parameters[5].Value = book.UnitPrice;
			parameters[6] = new SqlParameter("@ContentDescription", SqlDbType.NVarChar);
			if (book.ContentDescription != null)
			{
				parameters[6].Value = book.ContentDescription;
			}
			else
			{
				parameters[6].Value = DBNull.Value;
			}
			parameters[7] = new SqlParameter("@TOC", SqlDbType.NVarChar);
			if (book.TOC != null)
			{
				parameters[7].Value = book.TOC;
			}
			else
			{
				parameters[7].Value = DBNull.Value;
			}
			parameters[8] = new SqlParameter("@CategoryId", SqlDbType.Int);
			parameters[8].Value = book.Categorie.Id;
			return DBHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
		} 
		#endregion

		#region 修改图书
		/// <summary>
		/// 修改图书
		/// </summary>
		/// <param name="book"></param>
		public void Update(Book book)
		{
			string sql = "Update Books Set Title=@Title,Author=@Author,PublisherId=@PublisherId,PublishDate=@PublishDate,ISBN=@ISBN,UnitPrice=@UnitPrice,ContentDescription=@ContentDescription,TOC=@TOC,CategoryId=@CategoryId Where Id=@Id";
			SqlParameter[] parameters = new SqlParameter[10];
			parameters[0] = new SqlParameter("@Title", SqlDbType.NVarChar, 200);
			parameters[0].Value = book.Title;
			parameters[1] = new SqlParameter("@Author", SqlDbType.NVarChar, 200);
			parameters[1].Value = book.Author;
			parameters[2] = new SqlParameter("@PublisherId", SqlDbType.Int);
			parameters[2].Value = book.Publisher.Id;
			parameters[3] = new SqlParameter("@PublishDate", SqlDbType.DateTime);
			parameters[3].Value = book.PublishDate;
			parameters[4] = new SqlParameter("@ISBN", SqlDbType.NVarChar, 50);
			parameters[4].Value = book.ISBN;
			parameters[5] = new SqlParameter("@UnitPrice", SqlDbType.Money);
			parameters[5].Value = book.UnitPrice;
			parameters[6] = new SqlParameter("@ContentDescription", SqlDbType.NVarChar);
			if (book.ContentDescription != null)
			{
				parameters[6].Value = book.ContentDescription;
			}
			else
			{
				parameters[6].Value = DBNull.Value;
			}
			parameters[7] = new SqlParameter("@TOC", SqlDbType.NVarChar);
			if (book.TOC != null)
			{
				parameters[7].Value = book.TOC;
			}
			else
			{
				parameters[7].Value = DBNull.Value;
			}
			parameters[8] = new SqlParameter("@CategoryId", SqlDbType.Int);
			parameters[8].Value = book.Categorie.Id;
			parameters[9] = new SqlParameter("@Id", book.Id);
			parameters[9].Value = book.Id;
			DBHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
		} 
		#endregion

		#region 删除图书
		/// <summary>
		/// 删除图书
		/// </summary>
		/// <param name="id"></param>
		public void Delete(int id)
		{
			string sql = "Delete Books Where Id=@Id";
			DBHelper.ExecuteNonQuery(sql, CommandType.Text, new SqlParameter("@Id", id));
		} 
		#endregion

		#region 更新销售量
		/// <summary>
        /// 更新销售量
		/// </summary>
		/// <param name="id"></param>
		public void UpdateClicks(int id,int count)
		{
			string sql = "Update Books Set Clicks+=@Count Where Id=@Id";
            SqlParameter[] parameters = new SqlParameter[]{ 
                new SqlParameter("@Count",count),
                new SqlParameter("@Id", id)
            };
			DBHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
		} 
		#endregion

		#region 根据Id获得图书
		/// <summary>
		/// 根据Id获得图书
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Book GetBookById(int id)
		{
			Book book = null;
			string sql = "select Id,Title,Author,PublisherId,PublishDate,ISBN,UnitPrice,ContentDescription,TOC,CategoryId,Clicks from Books where Id = @Id";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Id", id));
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					book = LoadBook(reader);
				}
			}
			reader.Close();
			return book;
		} 
		#endregion

		#region 从SqlDataReader中读取数据，返回Book对象
		/// <summary>
		/// 从SqlDataReader中读取数据，返回Book对象
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private Book LoadBook(SqlDataReader reader)
		{
			Publisher publisher = new PublisherService().GetPublisherById(reader.GetInt32(3));
			Categorie categorie = new CategoryService().GetCategorieById(reader.GetInt32(9));
			Book book = new Book(
                reader.GetInt32(0), reader.GetString(1), reader.GetString(2), publisher,reader.GetDateTime(4), 
                reader.GetString(5), reader.GetDecimal(6), categorie, reader.GetInt32(10)
            );
			if (!(reader["ContentDescription"] is DBNull))
			{
				book.ContentDescription = reader.GetString(7);
			}
			else
			{
				book.ContentDescription = null;
			}
			if (!(reader["TOC"] is DBNull))
			{
				book.TOC = reader.GetString(8);
			}
			else
			{
				book.TOC = null;
			}
			return book;
		} 
		#endregion

		#region 查询所有图书
		/// <summary>
		/// 查询所有图书
		/// </summary>
		/// <returns>IList图书集合</returns>
		public IList<Book> GetBooks()
		{
			IList<Book> books = new List<Book>();
			string sql = "select Id,Title,Author,PublisherId,PublishDate,ISBN,UnitPrice,ContentDescription,TOC,CategoryId,Clicks from Books";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text);
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					Book book = LoadBook(reader);
					books.Add(book);
				}
			}
			reader.Close();
			return books;
		} 
		#endregion

		#region 根据排序查询指定数量的图书
		/// <summary>
		/// 根据排序查询指定数量的图书
		/// </summary>
		/// <param name="count">查询图书的数量</param>
		/// <param name="sort">排序方式</param>
		/// <returns></returns>
		public IList<Book> GetBooks(int count, string sort = null)
		{
			IList<Book> books = new List<Book>();
			string sql = "select Top "+count+" Id,Title,Author,PublisherId,PublishDate,ISBN,UnitPrice,ContentDescription,TOC,CategoryId,Clicks from Books ";
			if (sort != null)
			{
				sql += " order by "+sort;
			}
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text);
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					Book book = LoadBook(reader);
					books.Add(book);
				}
			}
			reader.Close();
			return books;
		} 
		#endregion

		#region 分页查询图书
		/// <summary>
		/// 分页查询图书
		/// </summary>
		/// <param name="pageSize">每页显示的数量</param>
		/// <param name="currentPageIndex">页码(从1开始)</param>
		/// <param name="condition">条件(and ...)</param>
		/// <param name="sort">排序方式</param>
		/// <param name="recordCount">总数量</param>
		/// <returns></returns>
		public IList<Book> GetBooks(int pageSize, int currentPageIndex, string condition, string sort, out int recordCount)
		{
			IList<Book> books = new List<Book>();
			//查询图书的sql语句
			StringBuilder sql =new StringBuilder();
			sql.Append("select Top "+pageSize+" Id,Title,Author,PublisherId,PublishDate,ISBN,UnitPrice,ContentDescription,TOC,CategoryId,Clicks from Books ");
			sql.Append(" Where Id Not In(Select Top "+pageSize*(currentPageIndex-1)+" Id From Books ");
			sql.Append(" Where 1=1 "+condition+" Order By "+sort+") ");
			sql.Append(condition+" Order By "+sort);
			SqlDataReader reader = DBHelper.ExecuteReader(sql.ToString(), CommandType.Text);
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					Book book = LoadBook(reader);
					books.Add(book);
				}
			}
			reader.Close();
			sql.Clear();
			sql.Append("select  Count(1) from Books  Where 1=1 "+condition);
			//获得总数量
			recordCount = Convert.ToInt32(DBHelper.ExecuteScalar(sql.ToString(), CommandType.Text));
			return books;
		} 
		#endregion

		#region 根据条件查询
		/// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="condition">条件(and ...)</param>
        /// <returns></returns>
        public IList<Book> GetBooks(string condition)
        {
			IList<Book> books = new List<Book>();
            //查询图书的sql语句
            string sql = "select Id,Title,Author,PublisherId,PublishDate,ISBN,UnitPrice,ContentDescription,TOC,CategoryId,Clicks from Books Where 1=1 "+condition;
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Book book = LoadBook(reader);
                    books.Add(book);
                }
            }
            reader.Close();
            return books;
        }
		#endregion

		#region 更新图书类别
		/// <summary>
		/// 更新图书类别
		/// </summary>
		/// <param name="bookId"></param>
		/// <param name="categroyId"></param>
		public void UpdateBookCategroy(int bookId, int categroyId)
		{
			string sql = "Update Books Set CategoryId=@CategoryId Where Id=@Id";
			SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@CategoryId",categroyId),
                new SqlParameter("@Id",bookId)
            };
			DBHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
		} 
		#endregion

        #region 查询是否存在这个书籍（title或ISBN）（默认为Title）
        /// <summary>
        /// 查询是否存在这个书籍（title或ISBN）（默认为Title）
        /// </summary>
        /// <param name="keyword">查询条件的值</param>
        /// <param name="searchType">查询条件（title或ISBN）</param>
        /// <param name="id">书籍编号</param>
        /// <returns></returns>
        public bool Exists(string keyword,string searchType = "Title", int id = -1)
        {
            string sql = "select Id from Books where "+searchType+" = @Keyword";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@Keyword", keyword)
            };
            if (id != -1)
            {
                sql += " And Id != @Id";
                parameters = new SqlParameter[] { 
                    new SqlParameter("@Keyword", keyword),
                    new SqlParameter("@Id",id)
                };
            }
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, parameters);
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    return true;
                }
            }
            reader.Close();
            return false;
        }
        #endregion
    }
}
