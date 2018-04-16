using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    //注册页模型
    public class RegisterModel
    {
        [DisplayName("用户名")]
        [Required(ErrorMessage="{0}不能为空")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "{0}长度在{2}到{1}之间")]
        [RegularExpression(@"^\S{6,18}$", ErrorMessage = "{0}不能包含空格")]
        public string LoginId { get; set; }

        [DisplayName("姓名")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string Name { get; set; }

        [DisplayName("密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "{0}长度在{2}到{1}之间")]
        [RegularExpression(@"^\S{6,18}$", ErrorMessage = "{0}不能包含空格")]
        public string LoginPwd { get; set; }

        [DisplayName("确认密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Compare("LoginPwd",ErrorMessage="两次密码输入不一致")]
        public string LoginPwdConfirm  { get; set; }

        [DisplayName("出生日期")]
        public DateTime? Birthday { get; set; }

        [DisplayName("电话")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(11, MinimumLength = 3, ErrorMessage = "{0}长度在{2}到{1}之间")]
        [RegularExpression(@"^\d+$",ErrorMessage="电话号码只能为数字")]
        public string Phone { get; set; }

        [DisplayName("电子邮件")]
        [Required(ErrorMessage = "{0}不能为空")]
        [RegularExpression(@"^\w+@\w+(\.[a-zA-Z]{2,3}){1,2}$",ErrorMessage="{0}格式错误")]
        public string Mail { get; set; }

        [DisplayName("地址")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string Address { get; set; }

        [DisplayName("验证码")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string SecurityCode { get; set; }
    }
}