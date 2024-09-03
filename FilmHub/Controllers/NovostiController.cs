using FilmHub.Data;
using FilmHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FilmHub.Controllers
{
    public class NovostiController : Controller
    {
        BazaDbContext bazaPodataka = new BazaDbContext();
        // GET: Novosti
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Popis()
        {
            var novosti = bazaPodataka.PopisNovosti.OrderByDescending(n => n.DatumUnosa).ToList();
            return View(novosti);
        }

        public ActionResult Detalji(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Popis");
            }


            

            Novost novost = bazaPodataka.PopisNovosti.FirstOrDefault(x => x.Id == id);


            if (novost == null)
            {
                return RedirectToAction("Popis");
            }
            return View(novost);

        }

        public ActionResult Azuriraj(int? id)
        {
            Novost novost = null;
            if (!id.HasValue)
            {
                novost = new Novost();
                ViewBag.Title = "Unos nove novosti.";
                ViewBag.Novi = true;

            }
            else
            {
                novost = bazaPodataka.PopisNovosti.FirstOrDefault(x => x.Id == id);

                if (novost == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Title = "Ažuriranje novosti.";
                ViewBag.Novi = false;

            }
            return View(novost);

        }

        [HttpPost]
        public ActionResult Azuriraj(Novost n)
        {

            if (ModelState.IsValid)
            {
                if (n.Id != 0)
                {
                    bazaPodataka.Entry(n).State = System.Data.Entity.EntityState.Modified;
                }

                else
                {
                    n.DatumUnosa = n.DatumUnosa ?? DateTime.Now;
                    bazaPodataka.PopisNovosti.Add(n);
                }
                bazaPodataka.SaveChanges();
                return RedirectToAction("Popis");
            }
            if (n.Id == 0)
            {
                ViewBag.Title = "Kreiranje novosti";
                ViewBag.Novi = true;
            }
            else
            {
                ViewBag.Title = "Ažuriranje novosti";
                ViewBag.Novi = false;
            }
            return View(n);
        }

        public ActionResult Brisi(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Novost n = bazaPodataka.PopisNovosti.FirstOrDefault(x => x.Id == id);

            if (n == null)
            {
                return HttpNotFound();
            }
            return View(n);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Brisi(int id)
        {
            Novost n = bazaPodataka.PopisNovosti.FirstOrDefault(x => x.Id == id);
            if (n  == null)
            {
                return HttpNotFound();
            }
            bazaPodataka.PopisNovosti.Remove(n);
            bazaPodataka.SaveChanges();
            return View("BrisiStatus");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bazaPodataka.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}