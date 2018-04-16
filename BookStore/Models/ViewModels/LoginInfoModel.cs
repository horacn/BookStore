using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    /// <summary>
    /// 登录页面的模型
    /// </summary>
    public class LoginInfoModel
    {
        [DisplayName("用户名")]
        [Required(ErrorMessage="{0}不能为空")]
        public string LoginId { get; set; }

        [DisplayName("密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string LoginPwd { get; set; }
    }
}