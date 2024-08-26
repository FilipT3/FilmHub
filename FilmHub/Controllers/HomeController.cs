using FilmHub.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FilmHub.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            LogiraniKorisnik logKor = User as LogiraniKorisnik;
            if (logKor != null)
            {
                ViewBag.Logirani = logKor.KorisnickoIme;
            }

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
    }
}