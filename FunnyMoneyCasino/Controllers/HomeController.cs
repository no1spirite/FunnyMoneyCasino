using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FunnyMoneyCasino.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to Funny Money Casino";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
