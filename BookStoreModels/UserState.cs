using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    /// <summary>
    /// 用户状态
    /// </summary>
    [Serializable]
    public class UserState
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string Name { get; set; }
        
        public UserState() { }
        public UserState(int id,string name) 
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
