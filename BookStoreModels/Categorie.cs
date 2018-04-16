using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    /// <summary>
    /// 书籍分类
    /// </summary>
    [Serializable]
    public class Categorie
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// PId
        /// </summary>
        public int? PId { get; set; }
        /// <summary>
        /// SortNum
        /// </summary>
        public int? SortNum { get; set; }

        public Categorie() { }
        public Categorie(int id,string name)
        {
            this.Id = id;
            this.Name = name;
        }
        public Categorie(int id, string name,int? pid,int? sortNum)
        {
            this.Id = id;
            this.Name = name;
            this.PId = pid;
            this.SortNum = sortNum;
        }
    }
}
