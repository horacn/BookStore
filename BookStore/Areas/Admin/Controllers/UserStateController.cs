using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.BLL;
using BookStore.Helpers;

namespace BookStore.Areas.Admin.Controllers
{
    [Authorize]
    [ExceptionLogInfo]
    public class UserStateController : Controller
    {
        private UserManager um = new UserManager();
        private UserStateManager usm = new UserStateManager();

        //显示用户状态页面
        public ActionResult Index(int stateId=-1,int pageIndex = 1)
        {
            IList<UserInfo> users = null;
            if (stateId==-1)
            {
                users = um.GetUserInfosAll();
            }
            else
            {
                users = um.GetUsersByState(stateId);
            }
            var userStates = usm.GetUserStatesAll();
            ViewBag.UserStates = userStates;
            ViewBag.StateId = stateId;
            int pageSize = 10;
            var pagedUsers = new PagedList<UserInfo>(users, pageSize, pageIndex);
            return View(pagedUsers);
        }
        //编辑用户状态
        public ActionResult UserStatus(string action,List<int> status)
        {
            if (status == null || status.Count() == 0)
            {
                TempData["message"] = "请选择用户";
            }
            else
            {
                int stateId = 0;
                if (action.Equals("启用用户"))
                {
                    stateId = usm.GetUserStateByName("正常").Id;
                }
                else if (action.Equals("禁用用户"))
                {
                    stateId = usm.GetUserStateByName("无效").Id;
                }
                foreach (int uid in status)
                {
                     um.UpdateUserState(uid, stateId);
                }
                TempData["message"] = "修改用户状态成功";
            }
            return RedirectToAction("Index", "UserState");
        }
    }
}
