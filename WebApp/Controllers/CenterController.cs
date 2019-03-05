using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Utility;

namespace WebApp.Controllers
{
    public class CenterController : DefaultController
    {
        //
        // GET: /Center/
        public static int pageSize = 5;
        public int count = 0;

        #region 我的信息
        public ActionResult Index()
        {
            ViewData["usermodel"] = db.zhancaiw_user.Where(c => c.userID == UserInfo.userID).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult Index(zhancaiw_user user)
        {
            string username = Request["username"];
            if (string.IsNullOrEmpty(username))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "用户名不能为空", null));
            }
            string nickname = Request["nickname"];
            if (string.IsNullOrEmpty(username))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "昵称不能为空", null));
            }
            string birthday = Request["birthday"];

            string sex = Request["sex"];

            string hobby = Request["hobby"];
            string desc = Request["desc"];

            var model = db.zhancaiw_user.Where(c => c.userID == UserInfo.userID).FirstOrDefault();
            if (model == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "登录已失效，请重新登录", null));
            }

            //判断用户名是否唯一
            var isusername = db.zhancaiw_user.Where(c => c.userName == username && c.userName != model.userName).FirstOrDefault();
            if (isusername != null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "该用户名已存在", null));
            }

            model.userName = username;
            model.nickName = nickname;
            if (!string.IsNullOrEmpty(birthday))
            {
                model.birthday = Convert.ToDateTime(birthday);
            }
            if (!string.IsNullOrEmpty(sex))
            {
                model.sex = Convert.ToInt32(sex);
            }
            if (!string.IsNullOrEmpty(hobby))
            {
                model.hobby = hobby;
            }
            if (!string.IsNullOrEmpty(desc))
            {
                model.introduce = desc;
            }
            db.SaveChanges();
            return Content(JsonReturn(Enum_ReturnStatus.成功, "信息更新成功", null));
        }
        #endregion

        #region 我的头像
        public ActionResult UpdateFace()
        {
            var model = db.zhancaiw_user.Where(c => c.userID == UserInfo.userID).FirstOrDefault();
            ViewData["src"] = model.aover;
            return View();
        }
        [HttpPost]
        public ActionResult UpdateFace(zhancaiw_user user)
        {
            string imgurl = Request["imgurl"];
            if (string.IsNullOrEmpty(imgurl))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "参数错误", null));
            }

            var model = db.zhancaiw_user.Where(c => c.userID == UserInfo.userID).FirstOrDefault();
            if (model == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "登录已失效，请重新登录！", null));
            }

            model.aover = imgurl;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(JsonReturn(Enum_ReturnStatus.成功, "更新成功", null));
        }
        #endregion

        #region 修改密码
        public ActionResult UpdatePwd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdatePwd(zhancaiw_user user)
        {
            string oldpwd = Request["oldpwd"];
            if (string.IsNullOrEmpty(oldpwd))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "旧密码不能为空", null));
            }
            string newpwd = Request["newpwd"];
            if (string.IsNullOrEmpty(newpwd))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "新密码不能为空", null));
            }
            if (newpwd.Trim().Length < 6)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "新密码长度不能小于6个字符", null));
            }

            var model = db.zhancaiw_user.Where(c => c.userID == UserInfo.userID).FirstOrDefault();
            if (model == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "登录已失效，请重新登录！", null));
            }
            var oldmd5pwd = oldpwd.Md5();
            var md5pwd = newpwd.Md5();

            if (model.userPwd != oldmd5pwd)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "登录密码错误！", null));
            }

            model.userPwd = md5pwd;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(JsonReturn(Enum_ReturnStatus.成功, "更新成功！", null));
        }
        #endregion

        #region 视频管理
        #region 视频列表
        public ActionResult VideoList(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                id = 1;
            }

            count = db.zhancaiw_video.Where(c => c.userID == UserInfo.userID).Count();

            int pageCount = (int)Math.Ceiling(count * 1.0 / pageSize);

            id = id < 1 ? 1 : id;
            id = id > pageCount ? pageCount : id;

            List<zhancaiw_video> list = null;
            if (count > 0)
            {
                list = db.zhancaiw_video.Where(c => c.userID == UserInfo.userID).OrderByDescending(c => c.videoID).Skip((Convert.ToInt32(id) - 1) * pageSize).Take(pageSize).ToList();
            }

            ViewData["list"] = list;

            ViewBag.pageIndex = id;
            ViewBag.pageCount = pageCount;
            ViewBag.urlParams = "/center/videolist/";
            return View();
        }
        #endregion

        #region 视频添加
        public ActionResult VideoAdd()
        {
            //分类
            var list = db.zhancaiw_category.Where(c => c.categoryPID == 0).ToList();
            ViewData["plist"] = list;
            return View();
        }
        [HttpPost]
        public ActionResult VideoAdd(zhancaiw_video video)
        {
            int sortid;
            if (!int.TryParse(Request["sortid"], out sortid))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "分类参数错误", null));
            }
            string selval = Request["selval"];
            if (string.IsNullOrEmpty(selval)||selval.IndexOf(',')==-1)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "二级分类参数错误", null));
            }


            string title = Request["title"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "视频标题不能为空", null));
            }
            string author = Request["author"];
            if (string.IsNullOrEmpty(author))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "视频作者不能为空", null));
            }
            string imgurl = Request["imgurl"];
            if (string.IsNullOrEmpty(imgurl))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "视频封面还没有上传", null));
            }
            string desc = Request["desc"];
            if (string.IsNullOrEmpty(desc))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "视频描述不能为空", null));
            }
            string url = Request["url"];
            if (string.IsNullOrEmpty(url))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "视频播放地址不能为空", null));
            }
            string sourceurl = Request["sourceurl"];

            zhancaiw_video model = new zhancaiw_video();
            model.videoTitle = title;
            model.videoAuthor = author;
            model.videoCover = imgurl;
            model.videoDescripe = desc;
            model.videoUrl = url;
            model.videoSourceUrl = sourceurl;
            model.isRecommend = false;
            model.clickCount = 0;
            model.sortID = sortid;
            model.categoryID = selval;
            model.userID = UserInfo.userID;
            model.videoStatus = (int)Enum_VideoStatus.正常;
            model.addTime = DateTime.Now;

            db.zhancaiw_video.Add(model);
            db.SaveChanges();

            return Content(JsonReturn(Enum_ReturnStatus.成功, "添加成功", null));
        }
        #endregion

        #region 视频编辑
        public ActionResult VideoEdit(int? id)
        {

            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("VideoList");
            }
            var model = db.zhancaiw_video.Where(c => c.videoID == id && c.userID == UserInfo.userID).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("VideoList");
            }

            //分类
            var list = db.zhancaiw_category.Where(c => c.categoryPID == 0).ToList();
            ViewData["plist"] = list;

       
            ViewData["videomodel"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult VideoEdit(zhancaiw_video video)
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "参数错误", null));
            }
            var model = db.zhancaiw_video.Where(c => c.videoID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "该数据不存在或已被删除", null));
            }
            int sortid;
            if (!int.TryParse(Request["sortid"], out sortid))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "分类参数错误", null));
            }
            string selval = Request["selval"];
            if (string.IsNullOrEmpty(selval) || selval.IndexOf(',') == -1)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "二级分类参数错误", null));
            }
            string title = Request["title"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "视频标题不能为空", null));
            }
            string author = Request["author"];
            if (string.IsNullOrEmpty(author))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "视频作者不能为空", null));
            }
            string imgurl = Request["imgurl"];

            string desc = Request["desc"];
            if (string.IsNullOrEmpty(desc))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "视频描述不能为空", null));
            }
            string url = Request["url"];
            if (string.IsNullOrEmpty(url))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "视频播放地址不能为空", null));
            }
            string sourceurl = Request["sourceurl"];

            model.videoTitle = title;
            model.videoAuthor = author;
            if (!string.IsNullOrEmpty(imgurl))
            {
                model.videoCover = imgurl;
            }
            model.videoDescripe = desc;
            model.videoUrl = url;
            model.videoSourceUrl = sourceurl;
            model.sortID = sortid;
            model.categoryID = selval;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(JsonReturn(Enum_ReturnStatus.成功, "更新成功", null));
        }
        #endregion

        #region 视频删除
        [HttpPost]
        public ActionResult VideoDelete()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "参数错误", null));
            }

            var model = db.zhancaiw_video.Where(c => c.videoID == id && c.userID == UserInfo.userID).FirstOrDefault();
            if (model == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "该数据不存在或已被删除", null));
            }

            db.Entry(model).State = System.Data.EntityState.Deleted;
            db.SaveChanges();
            return Content(JsonReturn(Enum_ReturnStatus.成功, "删除成功", null));
        }
        #endregion
        #endregion

        #region 友情链接

        #region 链接列表
        public ActionResult LinkList(int? id)
        {
            if (UserInfo.roleID != (int)Enum_UserRole.管理员)
            {
                Response.Write("<script>alert('警告，您已非法入侵将被强制退出！');</script>");
                Layout();
            }

            if (string.IsNullOrEmpty(id.ToString()))
            {
                id = 1;
            }
            count = db.zhancaiw_vyw_links_user.Where(c => true).Count();

            int pageCount = (int)Math.Ceiling(count * 1.0 / pageSize);

            id = id < 1 ? 1 : id;
            id = id > pageCount ? pageCount : id;

            List<zhancaiw_vyw_links_user> list = null;
            if (count > 0)
            {
                list = db.zhancaiw_vyw_links_user.Where(c => true).OrderByDescending(c => c.linkID).Skip((Convert.ToInt32(id) - 1) * pageSize).Take(pageSize).ToList();
            }

            ViewData["list"] = list;

            ViewBag.pageIndex = id;
            ViewBag.pageCount = pageCount;
            ViewBag.urlParams = "/center/linklist/";
            return View();
        }
        #endregion

        #region 链接添加
        public ActionResult LinkAdd()
        {
            if (UserInfo.roleID != (int)Enum_UserRole.管理员)
            {
                Response.Write("<script>alert('警告，您已非法入侵将被强制退出！');</script>");
                Layout();
            }
            return View();
        }
        [HttpPost]
        public ActionResult LinkAdd(zhancaiw_links link)
        {
            if (UserInfo.roleID != (int)Enum_UserRole.管理员)
            {
                Layout();
                return Content(JsonReturn(Enum_ReturnStatus.失败, "警告，您已非法入侵将被强制退出！", null));
            }
            string title = Request["title"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "链接标题不能为空", null));
            }
            string url = Request["url"];
            if (string.IsNullOrEmpty(url))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "链接地址不能为空", null));
            }

            zhancaiw_links model = new zhancaiw_links();
            model.linkName = title;
            model.linkUrl = url;
            model.userID = UserInfo.userID;
            model.isDelete = false;
            model.addTime = DateTime.Now;

            db.zhancaiw_links.Add(model);
            db.SaveChanges();

            return Content(JsonReturn(Enum_ReturnStatus.成功, "添加成功", null));
        }
        #endregion

        #region 链接编辑
        public ActionResult LinkEdit(int? id)
        {
            if (UserInfo.roleID != (int)Enum_UserRole.管理员)
            {
                Response.Write("<script>alert('警告，您已非法入侵将被强制退出！');</script>");
                Layout();
            }

            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("LinkList");
            }
            var model = db.zhancaiw_links.Where(c => c.linkID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("LinkList");
            }
            ViewData["linkmodel"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult LinkEdit(zhancaiw_links link)
        {
            if (UserInfo.roleID != (int)Enum_UserRole.管理员)
            {
                Layout();
                return Content(JsonReturn(Enum_ReturnStatus.失败, "警告，您已非法入侵将被强制退出！", null));
            }
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "参数错误", null));
            }
            var model = db.zhancaiw_links.Where(c => c.linkID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "该数据不存在或已被删除", null));
            }

            string title = Request["title"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "链接标题不能为空", null));
            }
            string url = Request["url"];
            if (string.IsNullOrEmpty(url))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "链接地址不能为空", null));
            }

            model.linkName = title;
            model.linkUrl = url;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(JsonReturn(Enum_ReturnStatus.成功, "更新成功", null));
        }
        #endregion

        #region 链接删除
        [HttpPost]
        public ActionResult LinkDelete()
        {
            if (UserInfo.roleID != (int)Enum_UserRole.管理员)
            {
                Layout();
                return Content(JsonReturn(Enum_ReturnStatus.失败, "警告，您已非法入侵将被强制退出！", null));
            }
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "参数错误", null));
            }

            var model = db.zhancaiw_links.Where(c => c.linkID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "该数据不存在或已被删除", null));
            }

            if (model.isDelete == true)
            {
                model.isDelete = false;
            }
            else
            {
                model.isDelete = true;
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(JsonReturn(Enum_ReturnStatus.成功, "删除成功", null));
        }
        #endregion
        #endregion

        #region 导航管理

        #region 导航列表
        public ActionResult NavList()
        {
            if (UserInfo.roleID != (int)Enum_UserRole.管理员)
            {
                Response.Write("<script>alert('警告，您已非法入侵将被强制退出！');</script>");
                Layout();
            }


            ViewData["list"] = GetNavListData();

            return View();
        }

        #region 分类
        public List<zhancaiw_category> GetNavListData()
        {
            List<zhancaiw_category> newlist = new List<zhancaiw_category>();
            var list = db.zhancaiw_category.Where(c => c.categoryPID == 0).ToList();
            foreach (var item in list)
            {
                newlist.Add(item);
                var list1 = db.zhancaiw_category.Where(c=>c.categoryPID==item.categoryID).ToList();
                foreach (var item1 in list1)
                {
                    item1.categoryName = "　" + item1.categoryName;
                    newlist.Add(item1);
                    var list2 = db.zhancaiw_category.Where(c => c.categoryPID == item1.categoryID).ToList();
                    foreach (var item2 in list2)
                    {
                        item2.categoryName = "　　" + item2.categoryName;
                        newlist.Add(item2);
                    }
                }
            }

            return newlist;
        }
        #endregion
        #endregion

        #region 导航一级添加
        public ActionResult NavAdd()
        {
            if (UserInfo.roleID != (int)Enum_UserRole.管理员)
            {
                Response.Write("<script>alert('警告，您已非法入侵将被强制退出！');</script>");
                Layout();
            }

            var list1 = db.zhancaiw_category.Where(c => c.categoryPID == 0).ToList();
            ViewData["list1"] = list1;

            List<zhancaiw_category> newlist = new List<zhancaiw_category>();
            if (list1.Count>0)
            {
                foreach (var item in list1)
                {
                    var list2 = db.zhancaiw_category.Where(c => c.categoryPID == item.categoryID).ToList();
                    if (list2.Count>0)
                    {
                        foreach (var item1 in list2)
                        {
                            newlist.Add(item1);
                        }
                    }
                }
            }
           
            ViewData["list2"] = newlist;
            return View();
        }
        [HttpPost]
        public ActionResult NavAdd(zhancaiw_category Nav)
        {
            if (UserInfo.roleID != (int)Enum_UserRole.管理员)
            {
                Layout();
                return Content(JsonReturn(Enum_ReturnStatus.失败, "警告，您已非法入侵将被强制退出！", null));
            }

            string sort = Request["sort"];
            string url = Request["url"];

            string sortone = Request["sortone"];

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "导航名称不能为空", null));
            }
            
            zhancaiw_category model = new zhancaiw_category();
            model.categoryName = name;
            if (string.IsNullOrEmpty(sort) && string.IsNullOrEmpty(sortone)) //添加一级导航
            {
                if (string.IsNullOrEmpty(url))
                {
                    return Content(JsonReturn(Enum_ReturnStatus.失败, "url参数不能为空", null));
                }
                model.categoryPID = 0;
            }
            else if (string.IsNullOrEmpty(sortone)&&!string.IsNullOrEmpty(sort))//添加二级导航
            {
                model.categoryPID = Convert.ToInt32(sort);
            }
            else if (!string.IsNullOrEmpty(sortone))
            {
                model.categoryPID = Convert.ToInt32(sortone);
            }
            model.addTime = DateTime.Now;

            db.zhancaiw_category.Add(model);
            db.SaveChanges();

            return Content(JsonReturn(Enum_ReturnStatus.成功, "添加成功", null));
        }
        #endregion

        #region 导航编辑
        public ActionResult NavEdit(int? id)
        {
            if (UserInfo.roleID != (int)Enum_UserRole.管理员)
            {
                Response.Write("<script>alert('警告，您已非法入侵将被强制退出！');</script>");
                Layout();
            }

            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("NavList");
            }
            var model = db.zhancaiw_category.Where(c => c.categoryID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("NavList");
            }
            ViewData["navmodel"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult NavEdit(zhancaiw_category Nav)
        {
            if (UserInfo.roleID != (int)Enum_UserRole.管理员)
            {
                Layout();
                return Content(JsonReturn(Enum_ReturnStatus.失败, "警告，您已非法入侵将被强制退出！", null));
            }
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "参数错误", null));
            }
            var model = db.zhancaiw_category.Where(c => c.categoryID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "该数据不存在或已被删除", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "导航名称不能为空", null));
            }
            string url = Request["url"];

            

            model.categoryName = name;
            if (model.categoryPID == 0)
            {
                if (string.IsNullOrEmpty(url))
                {
                    return Content(JsonReturn(Enum_ReturnStatus.失败, "url参数不能为空", null));
                }
                model.urlParams = url;
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(JsonReturn(Enum_ReturnStatus.成功, "更新成功", null));
        }
        #endregion

        #region 导航删除
        [HttpPost]
        public ActionResult NavDelete()
        {
            if (UserInfo.roleID != (int)Enum_UserRole.管理员)
            {
                Layout();
                return Content(JsonReturn(Enum_ReturnStatus.失败, "警告，您已非法入侵将被强制退出！", null));
            }
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "参数错误", null));
            }

            var model = db.zhancaiw_category.Where(c => c.categoryID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "该数据不存在或已被删除", null));
            }

            //判断是否有数据
            string iid = id.ToString();
            var list = db.zhancaiw_video.Where(c => c.categoryID.Contains(iid)).ToList();
            if (list.Count>0)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "请先删除该分类下的数据", null));
            }


            db.Entry(model).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return Content(JsonReturn(Enum_ReturnStatus.成功, "删除成功", null));
        }
        #endregion
        #endregion

        #region 建议意见
        public ActionResult FeedBack()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FeedBack(zhancaiw_feedback feedback)
        {
            string title = Request["title"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "标题不能为空", null));
            }

            string qq = Request["qq"];

            string desc = Request["desc"];
            if (string.IsNullOrEmpty(desc))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "描述不能为空", null));
            }

            var ischeck = db.zhancaiw_feedback.Where(c => c.userID == UserInfo.userID).OrderByDescending(c => c.ID).Take(1).FirstOrDefault();
            if (ischeck != null)
            {
                if (DateTime.Now <= ischeck.addTime.AddDays(1))
                {
                    return Content(JsonReturn(Enum_ReturnStatus.失败, "反馈失败，您今天已反馈，请明天再来。", null));
                }
            }

            zhancaiw_feedback model = new zhancaiw_feedback();
            model.addTime = DateTime.Now;
            model.describe = desc;
            model.title = title;
            if (string.IsNullOrEmpty(qq))
            {
                model.qq = Convert.ToInt32(qq);
            }
            model.userID = UserInfo.userID;
            db.zhancaiw_feedback.Add(model);
            db.SaveChanges();

            return Content(JsonReturn(Enum_ReturnStatus.成功, "反馈成功，非常感谢您的支持！", null));
        }
        #endregion

        #region 上传文件
        public ActionResult Upload()
        {
            string imgurl = "";
            foreach (string key in Request.Files)
            {
                //这里只测试上传第一张图片file[0]
                HttpPostedFileBase file0 = Request.Files[key];
                //转换成byte,读取图片MIME类型
                Stream stream;
                int size = file0.ContentLength / 1024; //文件大小KB
                if (size > 1024)
                {
                    return Content(JsonReturn(Enum_ReturnStatus.失败, "图片不能超过1M：", null));
                }
                byte[] fileByte = new byte[2];//contentLength，这里我们只读取文件长度的前两位用于判断就好了，这样速度比较快，剩下的也用不到。
                stream = file0.InputStream;
                stream.Read(fileByte, 0, 2);//contentLength，还是取前两位
                //获取图片宽和高
                //System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                //int width = image.Width;
                //int height = image.Height;
                string fileFlag = "";
                if (fileByte != null && fileByte.Length > 0)//图片数据是否为空
                {
                    fileFlag = fileByte[0].ToString() + fileByte[1].ToString();
                }
                string[] fileTypeStr = { "255216", "7173", "6677", "13780" };//对应的图片格式jpg,gif,bmp,png
                if (fileTypeStr.Contains(fileFlag))
                {
                    string action = Request["action"];
                    string path = "/uploads/";
                    switch (action)
                    {
                        case "headimage":
                            path += "headimage/";
                            break;
                        case "videocover":
                            path += "videocover/";
                            break;
                    }
                    string fullpath = path + UserInfo.userID + "/";
                    if (!Directory.Exists(Server.MapPath(fullpath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(fullpath));
                    }
                    Request.Files[key].SaveAs(Server.MapPath(fullpath + Request.Files[key].FileName));
                    imgurl = fullpath + Request.Files[key].FileName;
                }
                else
                {
                    return Content(JsonReturn(Enum_ReturnStatus.失败, "图片格式不正确：" + fileFlag, null));
                }
                stream.Close();
            }
            return Content(JsonReturn(Enum_ReturnStatus.成功, "上传成功", imgurl));
        }
        #endregion

        #region 退出
        public void Layout()
        {

            if (Session["zhancaiw_usermodel"] != null)
            {
                if (Request.Cookies["username"] != null && Request.Cookies["username"].ToString() != "" && Request.Cookies["userpwd"] != null && Request.Cookies["userpwd"].ToString() != "")
                {
                    HttpCookie ck1 = Request.Cookies["username"];
                    ck1.Expires = DateTime.Now.AddDays(-3);
                    Response.Cookies.Add(ck1);

                    HttpCookie ck2 = Request.Cookies["userpwd"];
                    ck2.Expires = DateTime.Now.AddDays(-3);
                    Response.Cookies.Add(ck2);
                }
                Session.Remove("zhancaiw_usermodel");
            }

            Response.Redirect("/account/login.html");
        }
        #endregion

        #region 分类
        /// <summary>
        /// 顶级导航id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<List<zhancaiw_category>> newlist = new List<List<zhancaiw_category>>();
        [HttpPost]
        public ActionResult PostNavList(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败,"参数错误",null));
            }
            var plist = db.zhancaiw_category.Where(c => c.categoryPID == id).ToList();
            if (plist.Count <= 0)
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "没有数据", null));
            }

            foreach (var item in plist)
            {
                List<zhancaiw_category> list2 = new List<zhancaiw_category>();
                list2.Add(item);
                var list1 = db.zhancaiw_category.Where(c => c.categoryPID == item.categoryID && c.categoryPID != id).ToList();
                list2.AddRange(list1);
                newlist.Add(list2);
            }
            return Content(JsonReturn(Enum_ReturnStatus.成功, "获取成年", newlist)); ;
        }
        #endregion
    }
}
