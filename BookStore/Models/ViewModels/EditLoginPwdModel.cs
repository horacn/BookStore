using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class EditLoginPwdModel
    {

        [DisplayName("原密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "{0}长度在{2}到{1}之间")]
        [RegularExpression(@"^\S{6,18}$", ErrorMessage = "{0}不能包含空格")]
        public string OldLoginPwd { get; set; }

        [DisplayName("新密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "{0}长度在{2}到{1}之间")]
        [RegularExpression(@"^\S{6,18}$", ErrorMessage = "{0}不能包含空格")]
        public string NewLoginPwd { get; set; }

        [DisplayName("确认密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Compare("NewLoginPwd", ErrorMessage = "两次密码输入不一致")]
        public string LoginPwdConfirm { get; set; }
    }
}