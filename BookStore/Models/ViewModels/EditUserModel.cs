using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    /// <summary>
    /// 编辑用户页模型
    /// </summary>
    public class EditUserModel
    {
        [DisplayName("编号")]
        [Required(ErrorMessage = "{0}不能为空")]
        public int Id { get; set; }

        [DisplayName("用户名")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string LoginId { get; set; }

        [DisplayName("姓名")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string Name { get; set; }

        [DisplayName("电话")]
        [Required(ErrorMessage = "{0}不能为空")]
        [RegularExpression(@"^\d+$", ErrorMessage = "电话号码只能为数字")]
        [StringLength(11, MinimumLength = 3, ErrorMessage = "{0}长度在{2}到{1}之间")]
        public string Phone { get; set; }

        [DisplayName("电子邮件")]
        [Required(ErrorMessage = "{0}不能为空")]
        [RegularExpression(@"^\w+@\w+(\.[a-zA-Z]{2,3}){1,2}$", ErrorMessage = "{0}格式错误")]
        public string Mail { get; set; }

        [DisplayName("地址")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string Address { get; set; }

        [DisplayName("出生日期")]
        public DateTime? Birthday { get; set; }
    }
}