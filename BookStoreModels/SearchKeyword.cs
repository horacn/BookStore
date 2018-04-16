using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
	/// <summary>
	/// 搜索词
	/// </summary>
    [Serializable]
    public class SearchKeyword
    {
		/// <summary>
		/// Id
		/// </summary>
        public int Id { get; set; }
		/// <summary>
		/// 搜索词语
		/// </summary>
        public string Keyword { get; set; }
		/// <summary>
		/// 搜索次数
		/// </summary>
        public int SearchCount { get; set; }

        public SearchKeyword() { }
        public SearchKeyword(int id, string keyword, int searchCount) 
        {
            this.Id = id;
            this.Keyword = keyword;
            this.SearchCount = searchCount;
        }
    }
}
