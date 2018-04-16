using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DAL;
using BookStore.Models;

namespace BookStore.BLL
{
    public class SearchKeywordManager
    {
        private SearchKeywordService sks = new SearchKeywordService();

        #region 根据搜索内容模糊查询(TOP x)
        /// <summary>
        /// 根据搜索内容模糊查询(TOP x)
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IList<string> GetSearchKeywords(string keyword, int count = 10)
        {
            //返回string类型的List，给Json数据使用
            var keywords =  sks.GetSearchKeywords(keyword,count);
            var jsonList = new List<string>();
            foreach (var key in keywords)
            {
                jsonList.Add(key.Keyword);
            }
            return jsonList;
        }
        #endregion

        #region 添加搜索词
        /// <summary>
        /// 添加搜索词
        /// </summary>
        /// <param name="keyword"></param>
        public void Add(string keyword)
        {
            if (sks.GetSearchKeywordByKeyword(keyword)==null)
            {
                sks.Add(keyword);
            }
            else
            {
                sks.UpdateSearchCount(keyword);
            }
        }
        #endregion
    }
}
