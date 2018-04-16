using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DAL;
using BookStore.Models;

namespace BookStore.BLL
{
    /// <summary>
    /// 业务逻辑层的用户角色类
    /// </summary>
    public class UserRoleManager
    {
        private UserRoleService urs = new UserRoleService();

		#region 根据Id获得UserRole对象
		/// <summary>
        /// 根据Id获得UserRole对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserRole GetUserRoleById(int id)
        {
            return urs.GetUserRoleById(id);
        }
		#endregion

        #region 根据角色名称查询用户角色对象
        /// <summary>
        /// 根据角色名称查询用户角色对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UserRole GetUserRoleByName(string name)
        {
            return urs.GetUserRoleByName(name);
        }
        #endregion

		#region 查询所有
		/// <summary>
		/// 查询所有
		/// </summary>
		/// <returns></returns>
		public IList<UserRole> GetUserRolesAll()
		{
			return urs.GetUserRolesAll();
		}
		#endregion
    }
}
