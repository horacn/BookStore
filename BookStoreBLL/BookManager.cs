using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DAL;
using BookStore.Models;

namespace BookStore.BLL
{
    public class BookManager
    {
        private BookService bs = new BookService();

		#region 新增图书
		/// <summary>
		/// 新增图书
		/// </summary>
		/// <param name="book"></param>
		/// <returns></returns>
		public int Add(Book book)
		{
			return bs.Add(book);
		} 
		#endregion

		#region 修改图书
		/// <summary>
		/// 修改图书
		/// </summary>
		/// <param name="book"></param>
		public void Update(Book book)
		{
			bs.Update(book);
		} 
		#endregion

		#region 删除图书
		/// <summary>
		/// 删除图书
		/// </summary>
		/// <param name="id"></param>
		public void Delete(int id)
		{
			bs.Delete(id);
		} 
		#endregion

        #region 更新销售量
        /// <summary>
        /// 更新销售量
        /// </summary>
        /// <param name="id"></param>
        public void UpdateClicks(int id, int count)
        {
			bs.UpdateClicks(id,count);
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
			return bs.GetBookById(id);
		} 
		#endregion

		#region 查询所有图书
		/// <summary>
		/// 查询所有图书
		/// </summary>
		/// <returns>IList图书集合</returns>
		public IList<Book> GetBooks()
		{
			return bs.GetBooks();
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
			return bs.GetBooks(count, sort);
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
			return bs.GetBooks(pageSize, currentPageIndex, condition, sort, out recordCount);
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
			return bs.GetBooks(condition);
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
			bs.UpdateBookCategroy(bookId, categroyId);
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
            return bs.Exists(keyword, searchType, id);
        }
        #endregion
    }
}
