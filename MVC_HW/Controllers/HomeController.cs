using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_HW.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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

        public ActionResult FileDowload(string Name, int IsDonlod = 0)
        {

            //return File(Server.MapPath("~/Upload/blue.png"), "image/png");

            var path = "~/Image/客戶資料/" + Name + ".jpg";
            if (IsDonlod == 1)
            {
                return File(Server.MapPath(path), "image/png", Name + ".jpg");
            }
            else
            {
                return File(Server.MapPath(path), "image/png");
            }
        }
    }
}