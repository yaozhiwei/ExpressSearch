using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpressSearch.Models
{
    public class SysExpressSearchInfo
    {
        [Key]
        public int SysExpressSearchInfoId { get; set; }
        public string mailNo { get; set; }
        public DateTime AddTime { get; set; }
        public string context { get; set; }
        public int SysExpressSearchId { get; set; }

        public SysExpressSearch SysExpressSearch { get; set; }
    }
}