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
    /// 用户表的数据库服务类
    /// </summary>
    public class UserService
    {
		#region 新增用户
		/// <summary>
		/// 新增用户
		/// </summary>
		/// <param name="userInfo"></param>
		/// <returns></returns>
		public int Add(UserInfo userInfo)
		{
            string sql = "insert users values(@LoginId,@LoginPwd,@Name,@Address,@Phone,@Mail,@Birthday,(select Id from UserRoles where Name='普通用户'),(select Id from UserStates where Name='正常'),@RegisterIp,GetDate());select @@IDENTITY";
			SqlParameter[] parameters = new SqlParameter[8];
			parameters[0] = new SqlParameter("@LoginId",SqlDbType.NVarChar,50);
			parameters[0].Value = userInfo.LoginId;
			parameters[1] = new SqlParameter("@LoginPwd", SqlDbType.NVarChar, 50);
			parameters[1].Value = userInfo.LoginPwd;
			parameters[2] = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
			parameters[2].Value = userInfo.Name;
			parameters[3] = new SqlParameter("@Address", SqlDbType.NVarChar, 200);
			parameters[3].Value = userInfo.Address;
			parameters[4] = new SqlParameter("@Phone", SqlDbType.NVarChar, 100);
			parameters[4].Value = userInfo.Phone;
			parameters[5] = new SqlParameter("@Mail", SqlDbType.NVarChar, 100);
			parameters[5].Value = userInfo.Mail;
			parameters[6] = new SqlParameter("@Birthday", SqlDbType.DateTime);
			if (userInfo.Birthday.HasValue)
			{
				parameters[6].Value = userInfo.Birthday;
			}
			else
			{
				parameters[6].Value = DBNull.Value;
			}
			parameters[7] = new SqlParameter("@RegisterIp", SqlDbType.NVarChar, 50);
			parameters[7].Value = userInfo.RegisterIp;
			//返回user的Id，即数据库上次产生的标识列值
			return Convert.ToInt32(DBHelper.ExecuteScalar(sql,CommandType.Text,parameters));
		} 
		#endregion

		#region 修改用户
		/// <summary>
		/// 修改用户
		/// </summary>
		/// <param name="userInfo"></param>
		public void Update(UserInfo userInfo)
		{
			string sql = "Update users Set Name=@Name,Address=@Address,Phone=@Phone,Mail=@Mail,Birthday=@Birthday Where Id=@Id";
			SqlParameter[] parameters = new SqlParameter[6];
			parameters[0] = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
			parameters[0].Value = userInfo.Name;
			parameters[1] = new SqlParameter("@Address", SqlDbType.NVarChar, 200);
			parameters[1].Value = userInfo.Address;
			parameters[2] = new SqlParameter("@Phone", SqlDbType.NVarChar, 100);
			parameters[2].Value = userInfo.Phone;
			parameters[3] = new SqlParameter("@Mail", SqlDbType.NVarChar, 100);
			parameters[3].Value = userInfo.Mail;
			parameters[4] = new SqlParameter("@Birthday", SqlDbType.DateTime);
			if (userInfo.Birthday.HasValue)
			{
				parameters[4].Value = userInfo.Birthday;
			}
			else
			{
				parameters[4].Value = DBNull.Value;
			}
			parameters[5] = new SqlParameter("@Id", SqlDbType.Int);
			parameters[5].Value = userInfo.Id;
			DBHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
		}
		#endregion

		#region 删除
		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="id"></param>
		public void Delete(int id) 
		{
			string sql = "delete users where Id=@Id";
			DBHelper.ExecuteNonQuery(sql,CommandType.Text,new SqlParameter("@Id",id));
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
			UserInfo user = null;
			string sql = "select Id,LoginId,LoginPwd,Name,Address,Phone,Mail,Birthday,UserRoleId,UserStateId,RegisterIp,RegisterTime from Users where LoginId = @LoginId";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@LoginId", loginId));
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					user = LoadUserInfo(reader);
				}
			}
			reader.Close();
			return user;
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
			UserInfo user = null;
			string sql = "select Id,LoginId,LoginPwd,Name,Address,Phone,Mail,Birthday,UserRoleId,UserStateId,RegisterIp,RegisterTime from Users where Id = @Id";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Id", id));
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					user = LoadUserInfo(reader);
				}
			}
			reader.Close();
			return user;
		} 
		#endregion

		#region 查询所有(除管理员)
		/// <summary>
        /// 查询所有(除管理员)
		/// </summary>
		/// <returns></returns>
		public IList<UserInfo> GetUserInfosAll()
		{
			IList<UserInfo> users = new List<UserInfo>();
            string sql = "select Id,LoginId,LoginPwd,Name,Address,Phone,Mail,Birthday,UserRoleId,UserStateId,RegisterIp,RegisterTime from Users Where UserRoleId <> (select Id from UserRoles where Name='管理员')";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text);
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					users.Add(LoadUserInfo(reader));
				}
			}
			reader.Close();
			return users;
		}
		#endregion

		#region 读取用户
		/// <summary>
		/// 读取用户
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private UserInfo LoadUserInfo(SqlDataReader reader)
		{
			UserRole userRole = new UserRoleService().GetUserRoleById(reader.GetInt32(8));
			UserState userState = new UserStateService().GetUserStateById(reader.GetInt32(9));
			UserInfo user = new UserInfo(
				reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
				reader.GetString(4), reader.GetString(5), reader.GetString(6), userRole, userState
			);
			//对于可空的日期，需要判断
			if (!(reader["Birthday"] is DBNull))
			{
				user.Birthday = reader.GetDateTime(7);
			}
			else
			{
				user.Birthday = null;
			}
			if (!(reader["RegisterIp"] is DBNull))
			{
				user.RegisterIp = reader.GetString(10);
			}
			else
			{
				user.RegisterIp = null;
			}
			if (!(reader["RegisterTime"] is DBNull))
			{
				user.RegisterTime = reader.GetDateTime(11);
			}
			else
			{
				user.RegisterTime = null;
			}
			return user;
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
			IList<UserInfo> users = new List<UserInfo>();
			string sql = "select Id,LoginId,LoginPwd,Name,Address,Phone,Mail,Birthday,UserRoleId,UserStateId,RegisterIp,RegisterTime from Users where UserStateId=@UserStateId";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text,new SqlParameter("@UserStateId",stateId));
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					users.Add(LoadUserInfo(reader));
				}
			}
			reader.Close();
			return users;
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
			string sql = "update users set UserStateId=@UserStateId where Id=@Id";
			SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@UserStateId",stateId),
                new SqlParameter("@Id",userId)
            };
			DBHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
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
            string sql = "select Id from Users where LoginId = @LoginId";
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@LoginId", loginId));
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    return true;
                }
            }
            reader.Close();
            return false;
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
            string sql = "update Users set LoginPwd=@NewLoginPwd where Id=@Id";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@NewLoginPwd",newLoginPwd),
                new SqlParameter("@Id",id)
            };
            DBHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
        } 
        #endregion
    }
}
