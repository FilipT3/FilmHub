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
    [Authorize(Roles = OvlastiKorisnik.Administrator)]
    public class KosaricaItemsController : Controller
    {
        BazaDbContext bazaPodataka = new BazaDbContext();
        BazaDbFavoriti bazaFavorita = new BazaDbFavoriti();


        [AllowAnonymous]
        // GET: KosaricaItems
        public ActionResult Index()
        {
            return View(bazaFavorita.PopisFavorita.ToList());
        }

        // GET: KosaricaItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favoriti favoriti = bazaFavorita.PopisFavorita.FirstOrDefault(x => x.Id == id);
            if (favoriti == null)
            {
                return HttpNotFound();
            }
            return View(favoriti);
        }

        [AllowAnonymous]
        public ActionResult DodajUKosaricu(int id)
        {
            /*if (id == null) { 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/

            Film film = bazaPodataka.PopisFilmova.Find(id);

            if (film == null)
            {
                return HttpNotFound();
            }

            Favoriti favItem = new Favoriti { Id = id, Naslov = film.Naslov , Kategorija = film.Kategorija , Godina = film.Godina };
            bazaFavorita.PopisFavorita.Add(favItem);
            bazaFavorita.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: KosaricaItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KosaricaItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Naslov,Kategorija,Godina")] Favoriti FavIt)
        {
            if (ModelState.IsValid)
            {
                bazaFavorita.PopisFavorita.Add(FavIt);
                bazaFavorita.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(FavIt);
        }

        [AllowAnonymous]
        // GET: KosaricaItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favoriti FAvIt = bazaFavorita.PopisFavorita.FirstOrDefault(x => x.Id == id);
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
            Favoriti FAvIt = bazaFavorita.PopisFavorita.FirstOrDefault(x => x.Id == id);
            bazaFavorita.PopisFavorita.Remove(FAvIt);
            bazaFavorita.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bazaFavorita.Dispose();
            }
            base.Dispose(disposing);
        }

        [AllowAnonymous]
        public ActionResult IspisFilmova(string Naslov, string Glumci, string Godina, string Trajanje)
        {
            var filmovi = bazaPodataka.PopisFilmova.ToList();

            if (!String.IsNullOrEmpty(Naslov))
            {
                filmovi = filmovi.Where(x => x.Naslov.ToUpper().Contains(Naslov.ToUpper())).ToList();
            }
            if (!String.IsNullOrEmpty(Glumci))
            {
                filmovi = filmovi.Where(x => x.Glumci.ToUpper().Contains(Glumci.ToUpper())).ToList();
            }
            if (!String.IsNullOrEmpty(Godina))
            {
                filmovi = filmovi.Where(x => x.Godina.ToString().Contains(Godina)).ToList();
            }

            FavoritisReports favoritiReport = new FavoritisReports();
            favoritiReport.ListaFilmovia(filmovi);

            return File(favoritiReport.Podaci, System.Net.Mime.MediaTypeNames.Application.Pdf, "Favorit.pdf");
        }

        [AllowAnonymous]
        public ActionResult ObrisiKosaricu()
        {
            var listaFavorita = bazaFavorita.PopisFavorita.ToList();

            foreach (var f in listaFavorita)
            {
                bazaFavorita.PopisFavorita.Remove(f);
            }

            bazaFavorita.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}

