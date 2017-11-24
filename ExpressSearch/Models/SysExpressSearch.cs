using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpressSearch.Models
{
    public class SysExpressSearch
    {
        [Key]
        public int SysExpressSearchId { get; set; }
        public string MailNo { get; set; }
        public string LastQueryTime { get; set; }
        public string UpdateStr { get; set; }
        public bool Flag { get; set; }
        public int Status { get; set; }
        public string Tel { get; set; }
        public string ExpSpellName { get; set; }

        public string ExpTextName { get; set; }
        public List<SysExpressSearchInfo> InfoList { get; set; }
    }
}