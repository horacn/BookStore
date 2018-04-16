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
    public class SearchKeywordService
    {
        #region 根据Keyword获得SearchKeyword对象
        /// <summary>
        /// 根据Keyword获得SearchKeyword对象
		/// </summary>
        /// <param name="Keyword"></param>
		/// <returns></returns>
		public SearchKeyword GetSearchKeywordByKeyword(string keyword)
		{
			SearchKeyword searchKeyword = null;
            string sql = "select Id,Keyword,SearchCount from SearchKeywords where Keyword=@keyword";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text,new SqlParameter("@Keyword",keyword));
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					searchKeyword = LoadSearchKeyword(reader);
				}
			}
			reader.Close();
			return searchKeyword;
		} 
		#endregion

       #region 根据搜索内容模糊查询(TOP x)
       /// <summary>
       /// 根据搜索内容模糊查询(TOP x)
       /// </summary>
       /// <param name="keyword"></param>
       /// <param name="count"></param>
       /// <returns></returns>
        public IList<SearchKeyword> GetSearchKeywords(string keyword,int count=10)
        {
            List<SearchKeyword> searchKeywords = new List<SearchKeyword>();
            string sql = "select TOP "+count+" Id,Keyword,SearchCount from SearchKeywords where Keyword like '" + keyword + "%' Order by SearchCount Desc";
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    searchKeywords.Add(LoadSearchKeyword(reader));
                }
            }
            reader.Close();
            return searchKeywords;
        }
        #endregion

        #region 添加搜索词
        /// <summary>
        /// 添加搜索词
        /// </summary>
        /// <param name="keyword"></param>
        public void Add(string keyword)
        {
            string sql = "insert SearchKeywords Values(@KeyWord,1)";
            DBHelper.ExecuteNonQuery(sql, CommandType.Text, new SqlParameter("@KeyWord", keyword));
        } 
        #endregion

        #region 更新搜索次数
        /// <summary>
        /// 更新搜索次数
        /// </summary>
        /// <param name="keyword"></param>
        public void UpdateSearchCount(string keyword)
        {
            string sql = "Update SearchKeywords Set SearchCount+=1 Where Keyword=@KeyWord";
            DBHelper.ExecuteNonQuery(sql, CommandType.Text, new SqlParameter("@KeyWord", keyword));
        } 
        #endregion

		#region 从SqlDataReader中读取数据，返回SearchKeyword对象
		/// <summary>
		/// 从SqlDataReader中读取数据，返回SearchKeyword对象
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private SearchKeyword LoadSearchKeyword(SqlDataReader reader)
		{
			SearchKeyword searchKeyword = new SearchKeyword(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
			return searchKeyword;
		} 
		#endregion
    }
}
