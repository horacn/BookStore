using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DAL;
using BookStore.Models;

namespace BookStore.BLL
{
    public class PublisherManager
    {
        private PublisherService ps = new PublisherService();

        #region 根据Id获得Publisher对象
        /// <summary>
        /// 根据Id获得Publisher对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Publisher GetPublisherById(int id)
        {
            return ps.GetPublisherById(id);
        } 
        #endregion

		#region 查询所有
		/// <summary>
		/// 查询所有
		/// </summary>
		/// <returns></returns>
		public IList<Publisher> GetPublishersAll()
		{
			return ps.GetPublishersAll();
		}
		#endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int Add(string name)
        {
            return ps.Add(name);
        } 
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="p"></param>
        public void Update(Publisher p)
        {
            ps.Update(p);
        } 
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ps.Delete(id);
        } 
        #endregion

        #region 查询是否存在这个出版社名称
        /// <summary>
        ///  查询是否存在这个出版社名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(string name, int id = -1)
        {
            return ps.Exists(name, id);
        }
        #endregion
    }
}
