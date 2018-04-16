using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    [Serializable]
    public class BookRating
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public UserInfo UserInfo { get; set; }
        public int? Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedTime { get; set; }


        public BookRating() { }
        public BookRating(int id, Book book, UserInfo userInfo,DateTime createdTime) 
        {
            this.Id = id;
            this.Book = book;
            this.UserInfo = userInfo;
            this.CreatedTime = createdTime;
        }
        public BookRating(int id, Book book, UserInfo userInfo,int? rating,string comment, DateTime createdTime)
        {
            this.Id = id;
            this.Book = book;
            this.UserInfo = userInfo;
            this.Rating = rating;
            this.Comment = comment;
            this.CreatedTime = createdTime;
        }
    }
}
