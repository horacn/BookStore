using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BookStore.DAL
{
    /// <summary>
    /// 数据访问层帮助类
    /// </summary>
    public static class DBHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static readonly string connStr = ConfigurationManager.ConnectionStrings["mssqlserver"].ConnectionString;

        /// <summary>
        /// 数据访问层增、删、改通用方法
        /// </summary>
        /// <param name="sql">sql语句、存储过程的名字</param>
        /// <param name="cmdType">Command的类型（sql语句、存储过程）</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响的行数</returns>
        public static int ExecuteNonQuery(string sql,CommandType cmdType,params SqlParameter[] parameters)
        {
            SqlCommand cmd = PrepareCommand(sql,cmdType,parameters);
            int result =  cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return result;
        }

        /// <summary>
        /// 执行返回单行单列的方法
        /// </summary>
        /// <param name="sql">sql语句、存储过程的名字</param>
        /// <param name="cmdType">Command的类型（sql语句、存储过程）</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响的行数</returns>
        public static Object ExecuteScalar(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            SqlCommand cmd = PrepareCommand(sql, cmdType, parameters);
            Object result = cmd.ExecuteScalar();
            cmd.Connection.Close();
            return result;
        }

        /// <summary>
        /// 执行查询的方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            SqlCommand cmd = PrepareCommand(sql, cmdType, parameters);
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        /// <summary>
        /// 准备command、组装sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static SqlCommand PrepareCommand(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.CommandType = cmdType;
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            cmd.Connection = new SqlConnection(connStr);
            cmd.Connection.Open();
            return cmd;
        }
    }
}
