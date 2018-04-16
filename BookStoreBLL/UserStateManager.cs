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
    /// 业务逻辑层用户状态类
    /// </summary>
    public class UserStateManager
    {
        private UserStateService uss = new UserStateService();

        #region 根据Id获得UserState对象
        /// <summary>
        /// 根据Id获得UserState对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserState GetUserStateById(int id)
        {
            return uss.GetUserStateById(id);
        } 
        #endregion

        #region 根据状态名称查询用户状态对象
        /// <summary>
        /// 根据状态名称查询用户状态对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UserState GetUserStateByName(string name)
        {
            return uss.GetUserStateByName(name);
        }
        #endregion

        #region 查询所有
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IList<UserState> GetUserStatesAll()
        {
            return uss.GetUserStatesAll();
        }
        #endregion
    }
}
