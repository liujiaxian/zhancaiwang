using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Model;
using Utility;

namespace WebApp.Controllers
{
    public class AccountController : BaseController
    {
        #region 登录
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostLogin()
        {
            string vcode = Request["vcode"];
            if (string.IsNullOrEmpty(vcode))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "验证码不能为空", null));
            }

            if (Session["zhancaiw_vcode"] == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "验证码服务端异常，请稍后再试！", null));
            }

            if (Session["zhancaiw_vcode"].ToString() != vcode)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "验证码错误", null));
            }

            string email = Request["email"];
            if (string.IsNullOrEmpty(email))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "邮箱不能为空", null));
            }

            if (email.IndexOf('@') != -1) //邮箱登录
            {
                if (!Regex.IsMatch(email, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))
                {
                    return Content(JsonReturn(Enum_ReturnStatus.失败, "邮箱格式不正确", null));
                }
            }

            string pwd = Request["password"];
            if (string.IsNullOrEmpty(pwd))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "密码不能为空", null));
            }

            bool check;
            if (!bool.TryParse(Request["check"], out check))
            {
                check = false;
            }

            string md5pwd = pwd.Md5();

            zhancaiw_user user = null;
            if (email.IndexOf('@') != -1) //邮箱登录
            {
                user = db.zhancaiw_user.Where(c => c.email == email && c.userPwd == md5pwd).FirstOrDefault();
            }
            else
            {
                user = db.zhancaiw_user.Where(c => c.userName == email && c.userPwd == md5pwd).FirstOrDefault();
            }

            if (user == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "账号或密码错误", null));
            }

            if (check) //自动登录
            {
                HttpCookie ck1 = new HttpCookie("username", EncryptHelper.EncryptCookie(email));
                ck1.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Add(ck1);

                HttpCookie ck2 = new HttpCookie("userpwd", EncryptHelper.EncryptCookie(md5pwd));
                ck2.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Add(ck2);
            }

            Session.Remove("zhancaiw_vcode");

            Session["zhancaiw_usermodel"] = user;

            user.lastTime = DateTime.Now;
            db.SaveChanges();
            return Content(JsonReturn(Enum_ReturnStatus.成功, "登录成功", user));
        }
        #endregion

        #region 注册
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostRegister()
        {
            string vcode = Request["vcode"];
            if (string.IsNullOrEmpty(vcode))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "验证码不能为空", null));
            }

            if (Session["zhancaiw_vcode"] == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "验证码服务端异常，请稍后再试！", null));
            }

            if (Session["zhancaiw_vcode"].ToString() != vcode)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "验证码错误", null));
            }

            string email = Request["email"];
            if (string.IsNullOrEmpty(email))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "邮箱不能为空", null));
            }
            if (!Regex.IsMatch(email, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "邮箱格式不正确", null));
            }

            #region 判断是否存在此账号
            var isext = db.zhancaiw_user.Where(c => c.email == email).FirstOrDefault();
            if (isext != null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "该邮箱已被注册！", null));
            }
            #endregion

            string pwd = Request["password"];
            if (string.IsNullOrEmpty(pwd))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "密码不能为空", null));
            }
            if (pwd.Trim().Length < 6)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "密码长度不能小于6位", null));
            }

            bool check;
            if (!bool.TryParse(Request["check"], out check))
            {
                check = false;
            }


            string md5pwd = pwd.Md5();

            zhancaiw_user user = new zhancaiw_user();
            user.email = email;
            user.userPwd = md5pwd;
            user.sex = (int)Enum_UserSex.保密;
            user.birthday = Convert.ToDateTime("1993-1-1");
            user.regTime = DateTime.Now;
            user.roleID = (int)Enum_UserRole.普通会员;

            db.zhancaiw_user.Add(user);
            db.SaveChanges();

            Session.Remove("zhancaiw_vcode");

            if (check)//免登录
            {
                Session["zhancaiw_usermodel"] = user;
            }

            return Content(JsonReturn(Enum_ReturnStatus.成功, "注册成功", user));
        }
        #endregion

        #region 忘记密码第一步验证邮箱
        public ActionResult ForgetPwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPwd(zhancaiw_user user)
        {
            string email = Request["email"];
            if (string.IsNullOrEmpty(email))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "邮箱不能为空", null));
            }
            if (!Regex.IsMatch(email, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "邮箱格式不正确", null));
            }

            string vcode = Request["vcode"];
            if (string.IsNullOrEmpty(vcode))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "验证码不能为空", null));
            }
            if (Session["zhancaiw_vcode"] == null||Session["zhancaiw_vcode"].ToString() != vcode)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "验证码错误", null));
            }

            //开始检验
            var model = db.zhancaiw_user.Where(c => c.email == email).FirstOrDefault();
            if (model == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "系统不存在此邮箱", null));
            }

            //检验
            if (Convert.ToDateTime(model.activeTime).AddMinutes(30) > DateTime.Now)
            {
                if (model.activeStatus==1)
                {
                    return Content(JsonReturn(Enum_ReturnStatus.成功, "发送成功！", new { email = email, token = model.activeToken }));
                }
                else
                {
                    return Content(JsonReturn(Enum_ReturnStatus.失败, "您的操作过于频繁，请30分钟后再来。", null));
                } 
            }

            #region 发送邮件
            //生成激活码
            Guid guid = Guid.NewGuid();
            string activetoken = guid.ToString().Md5();
            //存到数据库中 判断是否存在该用胡
            model.activeToken = activetoken;
            model.activeTime = DateTime.Now;
            model.activeStatus = 1;
            db.SaveChanges();
            //拼接激活的url
            string host = Request.Url.Scheme + "://" + Request.Url.Authority;
            string url = host + "/Account/ForgetPwdReset?email=" + email + "&token=" + activetoken;
            //获取内容
            string htmlbody = CreateHtmlPage(email,url, host);
            //调用邮件类发送邮件
            bool send = MailHelper.Send(email, email + ",重设您在展才网的密码", htmlbody);
            if (!send)//发送成功
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "发送邮件失败,请稍后重试！", null));
            }
            #endregion
            Session.Remove("zhancaiw_vcode");
            return Content(JsonReturn(Enum_ReturnStatus.成功, "发送成功！", new { email=email,token=activetoken }));
        }

        public string CreateHtmlPage(string email,string url,string domain)
        {
            string html = Request.MapPath("/Templates/sendemail.html");
            html = System.IO.File.ReadAllText(html);
            html = html.Replace("$Email", email).Replace("$Url", url).Replace("$Domain",domain).Replace("$Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return html;
        }
        #endregion

        #region 忘记密码第二步
        public ActionResult ForgetPwdLoginEmail()
        {
            string email = Request["email"];
            string token = Request["token"];
            var model = db.zhancaiw_user.Where(c => c.activeStatus == 1 && c.activeToken == token && c.email == email).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("ForgetPwd");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPwdLoginEmail(zhancaiw_user user)
        {
            string email = Request["email"];
            string token = Request["token"];
            string emailList = "";
            if (email.IndexOf('@') != -1)
            {
                string[] str = email.Split('@');
                emailList = str[1];
            }
            var loginemail = db.zhancaiw_email.Where(c => c.emailLast == emailList).FirstOrDefault();
            if (loginemail == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "系统官方暂无此邮箱官方登录地址，请自行登录！", null));
            }

            return Content(JsonReturn(Enum_ReturnStatus.成功, "获取成功！", loginemail.emailUrl));
        } 
        #endregion

        #region 忘记密码第三步
        public ActionResult ForgetPwdReset()
        {
            string email = Request["email"];
            string token = Request["token"];
            var model = db.zhancaiw_user.Where(c => c.activeStatus == 1 && c.activeToken == token && c.email == email).FirstOrDefault();
            if (model == null)
            {
                return Content("<script>alert('该链接已失效！');window.location='/Account/ForgetPwd'</script>");
            }

            if (model.activeStatus==0)
            {
                return Content("<script>alert('该链接未激活！');window.location='/Account/ForgetPwd'</script>");
            }

            //检验
            if (Convert.ToDateTime(model.activeTime).AddMinutes(30) < DateTime.Now)
            {
                return Content("<script>alert('该链接已失效！');window.location='/Account/ForgetPwd'</script>");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPwdReset(zhancaiw_user user)
        {
            string email = Request["email"];
            string token = Request["token"];
            var model = db.zhancaiw_user.Where(c => c.activeStatus == 1 && c.activeToken == token && c.email == email).FirstOrDefault();
            if (model == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "参数错误！", null));
            }

            string pwd = Request["password"];
            if (string.IsNullOrEmpty(pwd))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "密码不能为空！", null));
            }
            if (pwd.Trim().Length<6)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "密码不能小于6个字符！", null));
            }

            string vcode = Request["vcode"];
            if (string.IsNullOrEmpty(vcode))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "验证码不能为空", null));
            }
            if (Session["zhancaiw_vcode"] == null || Session["zhancaiw_vcode"].ToString() != vcode)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "验证码错误", null));
            }

            bool check;
            if (!bool.TryParse(Request["check"], out check))
            {
                check = false;
            }

            string md5pwd=pwd.Md5();
            if (model.userPwd == md5pwd)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "新密码不能与旧密码相同！", null));
            }

            model.userPwd = md5pwd;
            model.updateTime = DateTime.Now;
            model.activeStatus = 0;
            db.SaveChanges();
            Session.Remove("zhancaiw_vcode");

            if (check)//免登录
            {
                Session["zhancaiw_usermodel"] = model;
            }

            return Content(JsonReturn(Enum_ReturnStatus.成功, "重置成功！", null));
        } 
        #endregion

        #region 验证码
        [HttpGet]
        public ActionResult GetVcode(string codelength, string codesize)
        {
            int length;
            if (!int.TryParse(codelength, out length))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "验证码长度参数错误", null));
            }
            float size;
            if (!float.TryParse(codesize, out size))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "验证码字体大小参数错误", null));
            }
            var code = CaptchaHelper.CreateRandomCode(length);
            Session["zhancaiw_vcode"] = code;
            var img = CaptchaHelper.DrawImage(code, size, background: Color.White);
            return File(img, "Image/jpeg");
        }
        #endregion
    }
}
