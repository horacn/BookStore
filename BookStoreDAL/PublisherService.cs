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
    public class PublisherService
    {
		#region 根据Id获得Publisher对象
		/// <summary>
		/// 根据Id获得Publisher对象
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Publisher GetPublisherById(int id)
		{
			Publisher publisher = null;
			string sql = "select Id,Name from Publishers where Id = @Id";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Id", id));
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					publisher = LoadPublisher(reader);
				}
			}
			reader.Close();
			return publisher;
		} 
		#endregion

		#region 查询所有
		/// <summary>
		/// 查询所有
		/// </summary>
		/// <returns></returns>
		public IList<Publisher> GetPublishersAll()
		{
			IList<Publisher> publishers = new List<Publisher>();
			string sql = "select Id,Name from Publishers";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text);
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					publishers.Add(LoadPublisher(reader));
				}
			}
			reader.Close();
			return publishers;
		} 
		#endregion

		#region 从SqlDataReader中读取数据，返回Publisher对象
		/// <summary>
		/// 从SqlDataReader中读取数据，返回Publisher对象
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private Publisher LoadPublisher(SqlDataReader reader)
		{
			Publisher publisher = new Publisher(reader.GetInt32(0), reader.GetString(1));
			return publisher;
		} 
		#endregion

        #region 新增出版社
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int Add(string name)
        {
            string sql = "Insert Publishers Values(@Name);select @@IDENTITY";
            return Convert.ToInt32(DBHelper.ExecuteScalar(sql, CommandType.Text, new SqlParameter("@Name", name)));
        } 
        #endregion

        #region 修改出版社
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="p"></param>
        public void Update(Publisher p)
        {
            string sql = "Update Publishers Set Name=@Name Where Id=@Id";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@Name",p.Name),
                new SqlParameter("@Id",p.Id),
            };
            DBHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
        } 
        #endregion

        #region 删除出版社
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            string sql = "Delete Publishers Where Id=@Id";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@Id",id),
            };
            DBHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
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
            string sql = "select Id from Publishers where Name = @Name";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@Name", name)
            };
            if (id != -1)
            {
                sql += " And Id != @Id";
                parameters = new SqlParameter[] { 
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Id",id)
                };
            }
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, parameters);
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
    }
}
