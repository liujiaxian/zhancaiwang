using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Newtonsoft.Json;
using Utility;
using WebApp.Models;
using System.Net.Http;
using System.Net;
using System.Text;

namespace WebApp.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //热门视频
            var list = db.zhancaiw_video.Where(c => c.sortID == (int)Utility.Enum_VideoType.视频 && c.videoStatus == (int)Utility.Enum_VideoStatus.正常).OrderByDescending(c => c.clickCount).Skip(0).Take(6).ToList();
            ViewData["videolist"] = list;

            //热门音乐
            var musiclist = db.zhancaiw_video.Where(c => c.sortID == (int)Utility.Enum_VideoType.音乐 && c.videoStatus == (int)Utility.Enum_VideoStatus.正常).OrderByDescending(c => c.clickCount).Skip(0).Take(6).ToList();
            ViewData["musiclist"] = musiclist;
            return View();
        }

        #region 音乐
        public ActionResult Music()
        {
            //音乐列表
            var list = db.zhancaiw_video.Where(c => c.sortID == 2 && c.videoStatus == (int)Utility.Enum_VideoStatus.正常).OrderByDescending(c => c.videoID).ToList();
            ViewData["videolist"] = list;

            //ViewData["plist1"] = JsonConvert.SerializeObject(GetNavList(2));
            ViewData["plist"] = GetNavList(2);
            return View();
        }
        [HttpPost]
        public ActionResult Music(FormCollection collection)
        {
            StringBuilder sb = new StringBuilder();
            IQueryable<zhancaiw_video> newlist = db.zhancaiw_video.Where(c => c.sortID == 2);
            string sort = Request["sort"];

            //if (string.IsNullOrEmpty(sort))
            //{
            //    newlist = newlist.Where(c => c.sortID == 2);
            //}

            //if (sort.IndexOf('/') == -1)
            //{
            //    newlist = newlist.Where(c => c.sortID == 2);
            //}

            string str = sort.Replace('/', ',').Replace("0,", "");

            newlist = newlist.Where(c => c.categoryID.Contains(str)).OrderByDescending(c => c.clickCount);



            if (newlist.Count() <= 0)
            {
                sb.Append("暂无数据！");
            }

            foreach (var item in newlist.ToList())
            {
                sb.Append("<li class='list-group-item span-1'>");
                sb.Append("<div class='mv-list'>");
                sb.Append("<a href='/home/detail/" + item.videoID + ".html' title='" + item.videoTitle + "'>");
                sb.Append("<i class='iconfont mv-pay-icon'>&#xe678;</i>");
                sb.Append("<img src='" + item.videoCover + "' alt='" + item.videoTitle + "' style='width:280px;height:158px;' />");
                sb.Append("</a>");
                sb.Append("<div class='mv_list_title'>");
                sb.Append("<a href='javascript:void(0);' title='" + item.videoTitle + "'>" + item.videoTitle + "</a>");
                sb.Append("</div>");
                sb.Append("<div class='mv_list_singer'>");
                sb.Append("<a href='javascript:;' title='" + item.videoAuthor + "'>" + item.videoAuthor + "</a>");
                sb.Append("</div>");
                sb.Append("<div class='mv_list_info'>" + Convert.ToDateTime(item.addTime).ToString("yyyy-MM-dd HH:mm:ss") + "</div>");
                sb.Append("</div>");
                sb.Append("</li>");
            }

            return Content(sb.ToString());
        }
        #endregion

        #region 街舞
        public ActionResult HipHop()
        {
            //街舞列表
            var list = db.zhancaiw_video.Where(c => c.sortID == 4 && c.videoStatus == (int)Utility.Enum_VideoStatus.正常).OrderByDescending(c => c.videoID).ToList();
            ViewData["videolist"] = list;

            ViewData["plist"] = GetNavList(4);
            return View();
        }

        [HttpPost]
        public ActionResult HipHop(FormCollection collection)
        {
            StringBuilder sb = new StringBuilder();
            IQueryable<zhancaiw_video> newlist = db.zhancaiw_video.Where(c => c.sortID == 4);
            string sort = Request["sort"];
            //if (string.IsNullOrEmpty(sort))
            //{
            //    newlist = newlist.Where(c => c.sortID == 4);
            //}

            //if (sort.IndexOf('/') == -1)
            //{
            //    newlist = newlist.Where(c => c.sortID == 4);
            //}

            string str = sort.Replace('/', ',').Replace("0,", "");

            newlist = newlist.Where(c => c.categoryID.Contains(str)).OrderByDescending(c => c.clickCount);



            if (newlist.Count() <= 0)
            {
                sb.Append("暂无数据！");
            }

            foreach (var item in newlist.ToList())
            {
                sb.Append("<li class='list-group-item span-1'>");
                sb.Append("<div class='mv-list'>");
                sb.Append("<a href='/home/detail/" + item.videoID + ".html' title='" + item.videoTitle + "'>");
                sb.Append("<i class='iconfont mv-pay-icon'>&#xe678;</i>");
                sb.Append("<img src='" + item.videoCover + "' alt='" + item.videoTitle + "' style='width:280px;height:158px;' />");
                sb.Append("</a>");
                sb.Append("<div class='mv_list_title'>");
                sb.Append("<a href='javascript:void(0);' title='" + item.videoTitle + "'>" + item.videoTitle + "</a>");
                sb.Append("</div>");
                sb.Append("<div class='mv_list_singer'>");
                sb.Append("<a href='javascript:;' title='" + item.videoAuthor + "'>" + item.videoAuthor + "</a>");
                sb.Append("</div>");
                sb.Append("<div class='mv_list_info'>" + Convert.ToDateTime(item.addTime).ToString("yyyy-MM-dd HH:mm:ss") + "</div>");
                sb.Append("</div>");
                sb.Append("</li>");
            }

            return Content(sb.ToString());
        }
        #endregion

        #region 绘画
        public ActionResult Painting()
        {
            //绘画列表
            var list = db.zhancaiw_video.Where(c => c.sortID == 5 && c.videoStatus == (int)Utility.Enum_VideoStatus.正常).OrderByDescending(c => c.videoID).ToList();
            ViewData["videolist"] = list;

            ViewData["plist"] = GetNavList(5);
            return View();
        }
        [HttpPost]
        public ActionResult Painting(FormCollection collection)
        {
            StringBuilder sb = new StringBuilder();
            IQueryable<zhancaiw_video> newlist = db.zhancaiw_video.Where(c => c.sortID == 5);
            string sort = Request["sort"];
            //if (string.IsNullOrEmpty(sort))
            //{
            //    newlist = newlist.Where(c => c.sortID == 5);
            //}

            //if (sort.IndexOf('/') == -1)
            //{
            //    newlist = newlist.Where(c => c.sortID == 5);
            //}

            string str = sort.Replace('/', ',').Replace("0,", "");

            newlist = newlist.Where(c => c.categoryID.Contains(str)).OrderByDescending(c => c.clickCount);



            if (newlist.Count() <= 0)
            {
                sb.Append("暂无数据！");
            }

            foreach (var item in newlist.ToList())
            {
                sb.Append("<li class='list-group-item span-1'>");
                sb.Append("<div class='mv-list'>");
                sb.Append("<a href='/home/detail/" + item.videoID + ".html' title='" + item.videoTitle + "'>");
                sb.Append("<i class='iconfont mv-pay-icon'>&#xe678;</i>");
                sb.Append("<img src='" + item.videoCover + "' alt='" + item.videoTitle + "' style='width:280px;height:158px;' />");
                sb.Append("</a>");
                sb.Append("<div class='mv_list_title'>");
                sb.Append("<a href='javascript:void(0);' title='" + item.videoTitle + "'>" + item.videoTitle + "</a>");
                sb.Append("</div>");
                sb.Append("<div class='mv_list_singer'>");
                sb.Append("<a href='javascript:;' title='" + item.videoAuthor + "'>" + item.videoAuthor + "</a>");
                sb.Append("</div>");
                sb.Append("<div class='mv_list_info'>" + Convert.ToDateTime(item.addTime).ToString("yyyy-MM-dd HH:mm:ss") + "</div>");
                sb.Append("</div>");
                sb.Append("</li>");
            }

            return Content(sb.ToString());
        }
        #endregion

        #region 详细
        public ActionResult Detail(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Index");
            }
            var model = db.zhancaiw_video.Where(c => c.videoID == id && c.videoStatus == (int)Utility.Enum_VideoStatus.正常).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            int count = 0;
            #region 浏览记录
            string ip = GetData.GetIPAddress();
            var ipmodel = db.zhancaiw_look.Where(c => c.ipAddress == ip && c.videoID == id).FirstOrDefault();
            if (ipmodel == null)
            {
                zhancaiw_look look = new zhancaiw_look();
                look.videoID = (int)id;
                look.ipAddress = ip;
                look.addTime = DateTime.Now;
                db.zhancaiw_look.Add(look);
                count = db.SaveChanges();
            }

            if (count > 0)
            {
                model.clickCount += 1;
                db.SaveChanges();
            }
            #endregion

            ViewData["videomodel"] = model;
            return View();
        }
        #endregion

        #region 获取弹幕
        [HttpGet]
        public ActionResult GetTanMu()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content("{\"code\":1,\"danmaku\":\"\"");
            }
            int max;
            if (!int.TryParse(Request["max"], out max))
            {
                max = 100;
            }
            var list = db.zhancaiw_vyw_tanmu_video_user.Where(c => c.isShow == true && c.videoID == id).OrderByDescending(c => c.addTime).Skip(0).Take(max).ToList();
            if (list.Count <= 0)
            {
                return Content("{\"code\":1,\"danmaku\":\"\"");
            }
            string[] str = { Request["id"] };
            List<danmaku> newlist = new List<danmaku>();
            foreach (var item in list)
            {
                danmaku model = new danmaku();
                model.author = item.nickName == null ? item.email : item.nickName;
                model._id = item.ID.ToString();
                model.text = item.text;
                model.time = (float)item.time;
                model.color = item.color;
                model.type = item.type;
                model.__v = 0;
                model.player = str;
                newlist.Add(model);
            }
            TanMu tanmu = new TanMu();
            tanmu.code = 1;
            tanmu.danmaku = newlist;

            return Content(JsonConvert.SerializeObject(tanmu));
        }
        [HttpPost]
        public ActionResult GetTanMu(PostTanMu item)
        {
            if (item.text == null)
            {
                return Content("{\"code\":1,\"data\":\"\"}");
            }

            zhancaiw_tanmu model = new zhancaiw_tanmu();
            model.color = item.color;
            model.text = item.text;
            model.time = Math.Round(Convert.ToDouble(item.time), 1);
            model.type = item.type;
            model.userID = 2;
            model.videoID = Convert.ToInt32(item.player);
            model.isShow = true;
            model.addTime = DateTime.Now;
            db.zhancaiw_tanmu.Add(model);
            db.Configuration.ValidateOnSaveEnabled = false;
            int count = db.SaveChanges();
            db.Configuration.ValidateOnSaveEnabled = true;

            string[] str = { item.player };

            danmaku newmodel = new danmaku();
            newmodel.__v = 0;
            newmodel._id = item.player;
            newmodel.author = item.author;
            newmodel.color = item.color;
            newmodel.player = str;
            newmodel.text = item.text;
            newmodel.time = (float)Math.Round(Convert.ToDouble(item.time), 1);
            newmodel.type = item.type;

            return Content("{\"code\":1,\"data\":" + JsonConvert.SerializeObject(newmodel) + "}");
        }
        #endregion


        #region 导航递归
        /// <summary>
        /// 顶级导航id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<List<zhancaiw_category>> newlist = new List<List<zhancaiw_category>>();
        public List<List<zhancaiw_category>> GetNavList(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return null;
            }
            var plist = db.zhancaiw_category.Where(c => c.categoryPID == id).ToList();
            if (plist.Count <= 0)
            {
                return null;
            }

            foreach (var item in plist)
            {
                List<zhancaiw_category> list2 = new List<zhancaiw_category>();
                list2.Add(item);
                var list1 = db.zhancaiw_category.Where(c => c.categoryPID == item.categoryID && c.categoryPID != id).ToList();
                list2.AddRange(list1);
                newlist.Add(list2);
            }
            return newlist;
        }
        #endregion

        #region 测试
        public ActionResult Test()
        {
            return View();
        }

        public ActionResult PTest(FormCollection collection)
        {
            return Content("<div style='background:red;'>第三页</div>");
        }
        #endregion

        #region 获取友情链接数据
        [HttpGet]
        public ActionResult GetLinks()
        {
            var list = db.zhancaiw_links.Where(c => c.isDelete == false).ToList();
            if (list.Count > 0)
            {
                return Content(JsonReturn(Enum_ReturnStatus.成功, "获取成功", list));
            }
            else
            {
                return Content(JsonReturn(Enum_ReturnStatus.失败, "服务端没有数据", null));
            }
        }
        #endregion

        #region 联系我们
        public ActionResult Contact()
        {
            return View();
        }
        #endregion

        #region faqs
        public ActionResult FAQs()
        {
            return View();
        }
        #endregion
    }
}
