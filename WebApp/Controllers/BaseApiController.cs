using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Newtonsoft.Json;
using Utility;

namespace WebApp.Controllers
{
    public class BaseApiController : Controller
    {
        public bds256641637_dbEntities db = new bds256641637_dbEntities();

        public static string JsonReturn(Enum_ReturnStatus status, string msg, object data)
        {
            return JsonConvert.SerializeObject(new { status = status, msg = msg, data = data });
        }
    }
}
