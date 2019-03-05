using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

namespace WebApp.Controllers
{
    public class DefaultController : BaseController
    {
        public zhancaiw_user UserInfo { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["zhancaiw_usermodel"] == null)
            {
                var model = db.zhancaiw_user.Where(c => c.userID == 1).FirstOrDefault();
                Session["zhancaiw_usermodel"] = model;
                UserInfo = model;
                //if (Request.Cookies["username"] != null && Request.Cookies["username"].ToString() != "" && Request.Cookies["userpwd"] != null && Request.Cookies["userpwd"].ToString() != "")
                //{
                //    string username = EncryptHelper.DecryptCookie(Request.Cookies["username"].ToString());
                //    string userpwd = EncryptHelper.DecryptCookie(Request.Cookies["userpwd"].ToString());
                //    var model = db.zhancaiw_user.Where(c => (c.userName == username && c.userPwd == userpwd) || c.email == username && c.userPwd == userpwd).FirstOrDefault();
                //    if (model != null)
                //    {
                //        Session["zhancaiw_usermodel"] = model;
                //        UserInfo = model;
                //    }
                //    else
                //    {
                //        HttpCookie ck1 = Request.Cookies["username"];
                //        ck1.Expires = DateTime.Now.AddDays(-1);
                //        Response.Cookies.Add(ck1);

                //        HttpCookie ck2 = Request.Cookies["userpwd"];
                //        ck2.Expires = DateTime.Now.AddDays(-1);
                //        Response.Cookies.Add(ck2);
                //    }
                //}
                //else
                //{
                //    Response.Redirect("/account/login.html?redirect=" + HttpUtility.UrlEncode(Request.Url.ToString()));
                //}
            }
            else
            {
                UserInfo = Session["zhancaiw_usermodel"] as zhancaiw_user;
            }

        }
    }
}
