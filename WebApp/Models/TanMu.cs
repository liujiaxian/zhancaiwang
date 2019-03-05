using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    [Serializable]
    public class TanMu
    {
        public int code { get; set; }

        public List<danmaku> danmaku { get; set; }
    }
    [Serializable]
    public class danmaku
    {
        public string _id { get; set; }
        public string author { get; set; }

        public float time { get; set; }
        public string text { get; set; }
        public string color { get; set; }
        public string type { get; set; }
        public int __v { get; set; }
        public string[] player { get; set; }
    }
}