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
    public class CategoryService
    {
		#region 查询所有
		/// <summary>
		/// 查询所有
		/// </summary>
		/// <returns></returns>
		public IList<Categorie> GetCategoriesAll()
		{
			IList<Categorie> categorys = new List<Categorie>();
			string sql = "select Id,Name,PId,SortNum from Categories";
			SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text);
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					Categorie cate = LoadCategorie(reader);
					categorys.Add(cate);
				}
			}
			reader.Close();
			return categorys;
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
            Categorie categorie = null;
            string sql = "select Id,Name,PId,SortNum from Categories where Id = @Id";
            SqlDataReader reader = DBHelper.ExecuteReader(sql, CommandType.Text, new SqlParameter("@Id", id));
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    categorie = LoadCategorie(reader);
                }
            }
            reader.Close();
            return categorie;
        }
		#endregion

        #region 查询是否存在这个分类名称
       /// <summary>
       ///  查询是否存在这个分类名称
       /// </summary>
       /// <param name="name"></param>
       /// <param name="id"></param>
       /// <returns></returns>
        public bool Exists(string name,int id=-1)
        {
            string sql = "select Id from Categories where Name = @Name";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@Name", name)
            };
            if (id!=-1)
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

		#region 从SqlDataReader中读取数据，返回Categorie对象
		/// <summary>
        /// 从SqlDataReader中读取数据，返回Categorie对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Categorie LoadCategorie(SqlDataReader reader)
        {
            Categorie categorie = new Categorie(reader.GetInt32(0), reader.GetString(1));
            if (!(reader["PId"] is DBNull))
            {
                categorie.PId = reader.GetInt32(2);
            }
            else
            {
                categorie.PId = 0;
            }
            if (!(reader["SortNum"] is DBNull))
            {
                categorie.SortNum = reader.GetInt32(3);
            }
            else
            {
                categorie.SortNum = 0;
            }
            return categorie;
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
			string sql = "insert into Categories values(@Name,@PId,@SortNum)";
			SqlParameter[] parameters = new SqlParameter[3];
			parameters[0] = new SqlParameter("@Name", SqlDbType.NVarChar, 200);
			parameters[0].Value = category.Name;
			parameters[1] = new SqlParameter("@PId", SqlDbType.Int);
			if (category.PId.HasValue)
			{
				parameters[1].Value = category.PId;
			}
			else
			{
				parameters[1].Value = DBNull.Value;
			}
			parameters[2] = new SqlParameter("@SortNum", SqlDbType.Int);
			if (category.SortNum.HasValue)
			{
				parameters[2].Value = category.SortNum;
			}
			else
			{
				parameters[2].Value = DBNull.Value;
			}
			return DBHelper.ExecuteNonQuery(sql,CommandType.Text,parameters);
		}
		#endregion

		#region 更新
		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="category"></param>
		public void Update(Categorie category)
		{
			string sql = "Update Categories Set Name=@Name,PId=@PId,SortNum=@SortNum Where Id=@Id";
			SqlParameter[] parameters = new SqlParameter[4];
			parameters[0] = new SqlParameter("@Name", SqlDbType.NVarChar, 200);
			parameters[0].Value = category.Name;
			parameters[1] = new SqlParameter("@PId", SqlDbType.Int);
			if (category.PId.HasValue)
			{
				parameters[1].Value = category.PId;
			}
			else
			{
				parameters[1].Value = DBNull.Value;
			}
			parameters[2] = new SqlParameter("@SortNum", SqlDbType.Int);
			if (category.SortNum.HasValue)
			{
				parameters[2].Value = category.SortNum;
			}
			else
			{
				parameters[2].Value = DBNull.Value;
			}
			parameters[3] = new SqlParameter("@Id", SqlDbType.Int);
			parameters[3].Value = category.Id;
			DBHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
		}
		#endregion

		#region 删除
		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="category"></param>
		public void Delete(int id)
		{
			string sql = "Delete Categories Where Id=@Id";
			DBHelper.ExecuteNonQuery(sql, CommandType.Text, new SqlParameter("@Id",id));
		}
		#endregion
    }
}
