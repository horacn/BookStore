using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Helpers
{
    /// <summary>
    /// 分页类，通过提供的数据，页大小，当前页码，返回分页后的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T>:List<T>,IEnumerable<T>
    {
        /// <summary>
        /// 给定要分页的数据，完成初始化
        /// </summary>
        /// <param name="route">数据</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">当前页码</param>
        public PagedList(IEnumerable<T> route,int pageSize,int pageIndex)
        {
	        if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.TotalCount = route.Count();
            this.TotalPages = (this.TotalCount + pageSize - 1) / pageSize;
            if (this.TotalPages == 0)
            {
                this.TotalPages = 1;
            }
            if (pageIndex>this.TotalPages)
            {
                pageIndex = this.TotalPages;
                this.PageIndex = this.TotalPages;
            }
            this.AddRange(route.Skip(pageSize*(pageIndex-1)).Take(pageSize));
        }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage
        {
            get{ return this.PageIndex >1 ? true : false; }
        }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage
        {
            get{ return this.PageIndex < this.TotalPages ? true : false; }
        }
    }
}