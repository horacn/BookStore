using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    [Serializable]
    public class RecomBook
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public UserInfo UserInfo { get; set; }

        public RecomBook() { }
        public RecomBook(int id,Book book,UserInfo userInfo) 
        {
            this.Id = id;
            this.Book = book;
            this.UserInfo = userInfo;
        }
    }
}
