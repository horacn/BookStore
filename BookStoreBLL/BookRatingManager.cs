using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DAL;
using BookStore.Models;

namespace BookStore.BLL
{
    public class BookRatingManager
    {
        private BookRatingService brs = new BookRatingService();

        #region 根据Id获得BookRating对象
        /// <summary>
        /// 根据Id获得BookRating对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BookRating GetBookRatingById(int id)
        {
            return brs.GetBookRatingById(id);
        } 
        #endregion
    }
}
