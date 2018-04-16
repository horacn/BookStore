using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DAL;
using BookStore.Models;

namespace BookStore.BLL
{
    public class CategoryManager
    {
        private CategoryService cs = new CategoryService();

		#region 查询所有
		/// <summary>
		/// 查询所有
		/// </summary>
		/// <returns></returns>
		public IList<Categorie> GetCategoriesAll()
		{
			return cs.GetCategoriesAll();
		}
		#endregion

		#region 根据Id获得Categorie对象
		/// <summary>
		/// 根据Id获得Categorie对象
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Categorie GetCategorieById(int id)
		{
			return cs.GetCategorieById(id);
		}
		#endregion

        #region 查询是否存在这个分类名称
        /// <summary>
        ///  查询是否存在这个分类名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(string name, int id = -1)
        {
            return cs.Exists(name, id);
        }
        #endregion

		#region 新增
		/// <summary>
		/// 新增
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public int Add(Categorie category)
		{
			return cs.Add(category);
		}
		#endregion

		#region 更新
		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="category"></param>
		public void Update(Categorie category)
		{
			cs.Update(category);
		}
		#endregion

		#region 删除
		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="category"></param>
		public void Delete(int id)
		{
			cs.Delete(id);	
		}
		#endregion
    }
}
