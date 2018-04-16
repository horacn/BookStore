using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Serializable]
    public class UserRole
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        public UserRole() { }
        public UserRole(int id, string name) 
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
