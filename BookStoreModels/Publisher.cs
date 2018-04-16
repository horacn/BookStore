using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    /// <summary>
    /// 出版社
    /// </summary>
    [Serializable]
    public class Publisher
    {
        /// <summary>
        /// 编号，唯一标识
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 出版社名称
        /// </summary>
        public string Name { get; set; }

        public Publisher() { }
        public Publisher(int id,string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
