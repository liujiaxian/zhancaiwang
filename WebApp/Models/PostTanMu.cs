using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    [Serializable]
    public class PostTanMu
    {
        public string author { get; set; }
        public string color { get; set; }
        public string player { get; set; }
        public string text { get; set; }

        public float time { get; set; }
        public string token { get; set; }

        public string type { get; set; }
    }
}