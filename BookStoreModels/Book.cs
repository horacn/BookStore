using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    /// <summary>
    /// 书籍
    /// </summary>
    [Serializable]
    public class Book
    {
        [DisplayName("编号")]
        public int  Id { get; set; }

        [DisplayName("标题")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string Title { get; set; }

        [DisplayName("作者")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string Author { get; set; }

        [DisplayName("出版社")]
        [Required(ErrorMessage = "{0}不能为空")]
        public Publisher Publisher { get; set; }

        [DisplayName("出版日期")]
        [Required(ErrorMessage = "{0}不能为空")]
        public DateTime PublishDate { get; set; }

        [DisplayName("ISBN")]
        [Required(ErrorMessage = "{0}不能为空")]
        [RegularExpression(@"^[0-9a-zA-Z]+$", ErrorMessage = "{0}由数字、字母组成")]
        public string ISBN { get; set; }

        [DisplayName("单价")]
        [Required(ErrorMessage = "{0}不能为空")]
        [RegularExpression(@"^[0-9.]+$", ErrorMessage = "{0}格式错误")]
        public decimal UnitPrice { get; set; }

        [DisplayName("内容简介")]
        public string ContentDescription { get; set; }

        [DisplayName("目录")]
        public string TOC { get; set; }

        [DisplayName("分类")]
        [Required(ErrorMessage = "{0}不能为空")]
        public Categorie Categorie { get; set; }

        [DisplayName("销售量")]
        public int Clicks { get; set; }
      
        public Book() { }
        public Book(int id, string title, string author, Publisher publisher, DateTime publishDate,
            string isbn, decimal unitPrice, Categorie categorie, int clicks)
        {
            this.Id = id;
            this.Title = title;
            this.Author = author;
            this.Publisher = publisher;
            this.PublishDate = publishDate;
            this.ISBN = isbn;
            this.UnitPrice = unitPrice;
            this.Categorie = categorie;
            this.Clicks = clicks;
        }
        public Book(int id, string title, string author, Publisher publisher, DateTime publishDate,
           string isbn, decimal unitPrice, string contentDescription, string toc, Categorie categorie, int clicks)
        {
            this.Id = id;
            this.Title = title;
            this.Author = author;
            this.Publisher = publisher;
            this.PublishDate = publishDate;
            this.ISBN = isbn;
            this.UnitPrice = unitPrice;
            this.ContentDescription = contentDescription;
            this.TOC = toc;
            this.Categorie = categorie;
            this.Clicks = clicks;
        }
    }
}
