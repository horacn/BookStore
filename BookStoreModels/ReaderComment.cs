using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    [Serializable]
    public class ReaderComment
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public string ReaderName { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }

        public ReaderComment() { }
        public ReaderComment(int id,Book book,string readerName,string title,string comment,DateTime date)
        {
            this.Id = id;
            this.Book = book;
            this.ReaderName = readerName;
            this.Title = title;
            this.Comment = comment;
            this.Date = date;
        }
    }
}
