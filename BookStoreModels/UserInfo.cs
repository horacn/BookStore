using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    [Serializable]//可序列化
    public class UserInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string LoginPwd { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 住址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public UserRole UserRole { get; set; }  //外键属性
        /// <summary>
        /// 用户状态
        /// </summary>
        public UserState UserState { get; set; }//外键属性
        /// <summary>
        /// 注册IP
        /// </summary>
        public string RegisterIp { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime? RegisterTime { get; set; }

        public UserInfo() { }
        public UserInfo(int id, string loginId,string loginPwd,string name,string address,
            string phone, string mail, UserRole userRole,UserState userState) 
        {
            this.Id = id;
            this.LoginId = loginId;
            this.LoginPwd = loginPwd;
            this.Name = name;
            this.Address = address;
            this.Phone = phone;
            this.Mail = mail;
            this.UserRole = userRole;
            this.UserState = userState;
        }
        public UserInfo(int id, string loginId, string loginPwd, string name, string address,
            string phone, string mail, DateTime? birthday, UserRole userRole, UserState userState, string registerIp, DateTime? registerTime)
        {
            this.Id = id;
            this.LoginId = loginId;
            this.LoginPwd = loginPwd;
            this.Name = name;
            this.Address = address;
            this.Phone = phone;
            this.Mail = mail;
            this.Birthday = birthday;
            this.UserRole = userRole;
            this.UserState = userState;
            this.RegisterIp = registerIp;
            this.RegisterTime = registerTime;
        }
    }
}
