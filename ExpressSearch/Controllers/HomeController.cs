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

namespace ExpressSearch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            //Models.ApplicationDbContext db = new Models.ApplicationDbContext();
            //var query = db.expressSearchs.Find(1).InfoList.ToList();

            //https://market.aliyun.com/products/57126001/cmapi010996.html
            ExpressSearchService service = new ExpressSearchService();
            string resultJson = service.ExpInfo("zhongtong", "465943638723");

            Models.ResultJson expressSearchModel = JsonMapper.ToObject<Models.ResultJson>(resultJson);
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