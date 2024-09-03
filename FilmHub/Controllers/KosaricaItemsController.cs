using FilmHub.Misc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FilmHub.Models;
using FilmHub.Reports;
using FilmHub.Data;

namespace FilmHub.Controllers
{
    public class KosaricaItemsController : Controller
    {
        BazaDbContext bazaPodataka = new BazaDbContext();


        [AllowAnonymous]
        // GET: KosaricaItems
        public ActionResult Index()
        {
            return View(bazaPodataka.PopisFavorita.ToList());
        }

        [AllowAnonymous]
        public ActionResult DodajUKosaricu(int id)
        {
            Film film = bazaPodataka.PopisFilmova.Find(id);

            if (film == null)
            {
                return HttpNotFound();
            }
            else
            {
                bool filmAlreadyInFavorites = bazaPodataka.PopisFavorita.Any(f => f.Naslov == film.Naslov);

                if (filmAlreadyInFavorites)
                {
                    TempData["Poruka"] = "Film je već u favoritima.";
                }
                else
                {
                    Favoriti favItem = new Favoriti
                    {
                        Id = id,
                        Naslov = film.Naslov,
                        Kategorija = film.Kategorija,
                        Godina = film.Godina
                    };
                    bazaPodataka.PopisFavorita.Add(favItem);
                    bazaPodataka.SaveChanges();
                    TempData["Poruka"] = "Film je uspješno dodan u favorite.";
                }
            }
            return RedirectToAction("Index");
        }



        // GET: KosaricaItems/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Naslov,Kategorija,Godina")] Favoriti FavIt)
        {
            if (ModelState.IsValid)
            {
                bazaPodataka.PopisFavorita.Add(FavIt);
                bazaPodataka.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(FavIt);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favoriti favoriti = bazaPodataka.PopisFavorita.FirstOrDefault(x => x.Id == id);
            if (favoriti == null)
            {
                return HttpNotFound();
            }
            return View(favoriti);
        }

        [AllowAnonymous]
        // GET: KosaricaItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favoriti FAvIt = bazaPodataka.PopisFavorita.FirstOrDefault(x => x.Id == id);
            if (FAvIt == null)
            {
                return HttpNotFound();
            }
            return View(FAvIt);
        }

        [AllowAnonymous]
        // POST: KosaricaItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Favoriti FAvIt = bazaPodataka.PopisFavorita.FirstOrDefault(x => x.Id == id);
            bazaPodataka.PopisFavorita.Remove(FAvIt);
            bazaPodataka.SaveChanges();
            return View("DeleteStatus");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bazaPodataka.Dispose();
            }
            base.Dispose(disposing);
        }


        [AllowAnonymous]
        public ActionResult IspisFavorita(string Naslov)
        {
            var f = bazaPodataka.PopisFavorita.ToList();

            if (!String.IsNullOrEmpty(Naslov))
            {
                f = f.Where(x => x.Naslov.ToUpper().Contains(Naslov.ToUpper())).ToList();
            }
            FavoritisReports favoritiReport = new FavoritisReports();
            favoritiReport.ListaFilmovia(f);

            return File(favoritiReport.Podaci, System.Net.Mime.MediaTypeNames.Application.Pdf, "Favorit.pdf");
        }

        [AllowAnonymous]
        public ActionResult ObrisiKosaricu()
        {
            var listaFavorita = bazaPodataka.PopisFavorita.ToList();

            foreach (var f in listaFavorita)
            {
                bazaPodataka.PopisFavorita.Remove(f);
            }

            bazaPodataka.SaveChanges();

            return View("DeleteStatusAll");
        }

    }
}

