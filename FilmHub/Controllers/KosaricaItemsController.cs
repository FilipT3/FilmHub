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
            return View(bazaPodataka.PopisFilmova.ToList());
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
                Favoriti favItem = new Favoriti { Id = id, Naslov = film.Naslov, Kategorija = film.Kategorija, Godina = film.Godina };
                bazaPodataka.PopisFavorita.Add(favItem);
                bazaPodataka.SaveChanges();
            }
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
            return RedirectToAction("Index");
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
            var listaFavorita = bazaPodataka.PopisFavorita.ToList();

            foreach (var f in listaFavorita)
            {
                bazaPodataka.PopisFavorita.Remove(f);
            }

            bazaPodataka.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}

