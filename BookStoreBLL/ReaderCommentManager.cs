using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DAL;
using BookStore.Models;

namespace BookStore.BLL
{
    public class ReaderCommentManager
    {
        private ReaderCommentService rcs = new ReaderCommentService();

        #region 根据Id获得ReaderComment对象
        /// <summary>
        /// 根据Id获得ReaderComment对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReaderComment GetReaderCommentById(int id)
        {
            return rcs.GetReaderCommentById(id);
        } 
        #endregion
    }
}
