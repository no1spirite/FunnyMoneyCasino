using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace FunnyMoneyCasino.Controllers
{
    public class GamesController : Controller
    {
        //
        // GET: /Games/

        public ActionResult BlackJack()
        {
            return View();
        }

        public ActionResult Poker()
        {
            return View();
        }

        public ActionResult Roulette()
        {
            return View();
        }

    }
}
