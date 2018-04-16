using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DAL;
using BookStore.Models;

namespace BookStore.BLL
{
    public class RecomBookManager
    {
        private RecomBookService rbs = new RecomBookService();

        #region 根据Id获得RecomBook对象
        /// <summary>
        /// 根据Id获得RecomBook对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RecomBook GetRecomBookById(int id)
        {
            return rbs.GetRecomBookById(id);
        } 
        #endregion
    }
}
