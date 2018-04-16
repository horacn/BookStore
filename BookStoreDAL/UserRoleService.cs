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
    /// 用户角色的数据库服务类
    /// </summary>
    public class UserRoleService
    {
		#region 根据角色编号查询用户角色对象
		/// <summary>
		/// 根据角色编号查询用户角色对象
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public UserRole GetUserRoleById(int id)
		{
			UserRole userRole = null;
			string sql = "select Id,Name from UserRoles where id = @Id";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Id", id));
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					userRole = LoadUserRole(reader);
				}
			}
			reader.Close();
			return userRole;
		} 
		#endregion

		#region 查询所有
		/// <summary>
		/// 查询所有
		/// </summary>
		/// <returns></returns>
		public IList<UserRole> GetUserRolesAll()
		{
			IList<UserRole> userRoles = new List<UserRole>();
			string sql = "select Id,Name from UserRoles";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text);
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					userRoles.Add(LoadUserRole(reader));
				}
			}
			reader.Close();
			return userRoles;
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
            UserRole userRole = null;
            string sql = "select Id,Name from UserRoles where Name = @Name";
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Name", name));
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    userRole = LoadUserRole(reader);
                }
            }
            reader.Close();
            return userRole;
        }
        #endregion

		#region 从SqlDataReader中读取数据，返回用户角色对象
		/// <summary>
		/// 从SqlDataReader中读取数据，返回用户角色对象
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private UserRole LoadUserRole(SqlDataReader reader)
		{
			UserRole userRole = new UserRole(reader.GetInt32(0), reader.GetString(1));
			return userRole;
		} 
		#endregion
    }
}
