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
    /// <summary>
    /// 用户状态的数据库服务类
    /// </summary>
    public class UserStateService
    {
		#region 根据状态编号查询用户状态对象
		/// <summary>
		/// 根据状态编号查询用户状态对象
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public UserState GetUserStateById(int id)
		{
			UserState userState = null;
			string sql = "select Id,Name from UserStates where id = @Id";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Id", id));
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					userState = LoadUserState(reader);
				}
			}
			reader.Close();
			return userState;
		} 
		#endregion

        #region 查询所有
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IList<UserState> GetUserStatesAll()
        {
            IList<UserState> userStates = new List<UserState>();
            string sql = "select Id,Name from UserStates";
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    userStates.Add(LoadUserState(reader));
                }
            }
            reader.Close();
            return userStates;
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
            UserState userState = null;
            string sql = "select Id,Name from UserStates where Name = @Name";
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Name", name));
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    userState = LoadUserState(reader);
                }
            }
            reader.Close();
            return userState;
        }
        #endregion

		#region 从SqlDataReader中读取数据，返回用户状态对象
		/// <summary>
		/// 从SqlDataReader中读取数据，返回用户状态对象
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private UserState LoadUserState(SqlDataReader reader)
		{
			UserState UserState = new UserState(reader.GetInt32(0), reader.GetString(1));
			return UserState;
		} 
		#endregion
    }
}
