using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressSearch.Models
{
    public class ResultJson
    {
        public int status { get; set; }

        public string msg { get; set; }

        public object info { get; set; }
    }
}