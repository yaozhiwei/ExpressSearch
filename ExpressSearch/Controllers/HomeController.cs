using ExpressSearch.application;
using ExpressSearch.code;
using ExpressSearch.Helpers;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ExpressSearch.Code;
using ExpressSearch.Models;

namespace ExpressSearch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //阿里云快递申请地址
            //https://market.aliyun.com/products/57126001/cmapi010996.html

            ExpressSearchService service = new ExpressSearchService();
            string resultJson = service.ExpInfo("zhongtong", "465943638723");

            ResultJson expressSearchModel = Code.Json.ToObject<ResultJson>(resultJson);
            return View(expressSearchModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}