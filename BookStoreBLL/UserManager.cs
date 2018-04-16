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
    /// 业务逻辑层的用户管理类
    /// </summary>
    public class UserManager
    {
        private UserService us = new UserService();

        #region 新增用户
		/// <summary>
		/// 新增用户
		/// </summary>
		/// <param name="userInfo"></param>
		/// <returns></returns>
		public int Add(UserInfo userInfo)
		{
			return us.Add(userInfo);
		} 
		#endregion

		#region 修改用户
		/// <summary>
		/// 修改用户
		/// </summary>
		/// <param name="userInfo"></param>
		public void Update(UserInfo userInfo)
		{
			us.Update(userInfo);
		}
		#endregion

		#region 删除
		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="id"></param>
		public void Delete(int id) 
		{
			us.Delete(id);
		}
		#endregion

		#region 根据登录名查询用户对象
		/// <summary>
		/// 根据登录名查询用户对象
		/// </summary>
		/// <param name="loginId"></param>
		/// <returns></returns>
		public UserInfo GetUserInfoByLoginId(string loginId)
		{
			return us.GetUserInfoByLoginId(loginId);
		} 
		#endregion

		#region 根据用户Id查询用户对象
		/// <summary>
		/// 根据用户Id查询用户对象
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public UserInfo GetUserInfoById(int id)
		{
			return us.GetUserInfoById(id);
		} 
		#endregion

        #region 查询所有(除管理员)
        /// <summary>
        /// 查询所有(除管理员)
		/// </summary>
		/// <returns></returns>
		public IList<UserInfo> GetUserInfosAll()
		{
			return us.GetUserInfosAll();
		}
		#endregion

		#region 根据状态查询用户信息
		/// <summary>
		/// 根据状态查询用户信息
		/// </summary>
		/// <param name="stateId"></param>
		/// <returns></returns>
		public IList<UserInfo> GetUsersByState(int stateId)
		{
			return us.GetUsersByState(stateId);
		}
		#endregion

		#region 更新用户状态
		/// <summary>
		/// 更新用户状态
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="stateId"></param>
		public void UpdateUserState(int userId, int stateId)
		{
			us.UpdateUserState(userId,stateId);
		} 
		#endregion

        #region 查询是否存在这个用户名
        /// <summary>
        ///  查询是否存在这个用户名
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public bool Exists(string loginId)
        {
            return us.Exists(loginId);
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="newLoginPwd">新密码</param>
        public void UpdatePassword(int id, string newLoginPwd)
        {
            us.UpdatePassword(id, newLoginPwd);
        }
        #endregion
    }
}
