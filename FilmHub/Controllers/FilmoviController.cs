
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using FilmHub.Data;
using FilmHub.Models;
using FilmHub.Reports;

namespace FilmHub.Controllers
{
    [Authorize]
    public class FilmoviController : Controller
    {
        BazaDbContext bazaPodataka = new BazaDbContext();
        // GET: Filmovi
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Početna o filmovima";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Trazilica()
        {
            ViewBag.Title = "Pretraži naslove";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Novosti()
        {
            ViewBag.Title = "Novosti";
            return View();
        }
        public ActionResult Favoriti()
        {
            ViewBag.Title = "Favoriti";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Popis(string naziv, string opis)
        {
            var filmovi = bazaPodataka.PopisFilmova.AsQueryable();

            if (!String.IsNullOrWhiteSpace(naziv))
            {
                filmovi = filmovi.Where(x => x.Naslov.ToUpper().Contains(naziv.ToUpper()));
            }

            

            var rezultatPretrage = filmovi.ToList();

            if (rezultatPretrage.Count == 0)
            {
                ViewBag.Poruka = "Nema rezultata pretrage.";
            }

            return View(rezultatPretrage);
        }


        [AllowAnonymous]
        public ActionResult Detalji(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Popis");
            }

            
            Film film = bazaPodataka.PopisFilmova.FirstOrDefault(x => x.Id == id);

            if (film == null)
            {
                return RedirectToAction("Popis");
            }
            return View(film);

        }

        


        public ActionResult Azuriraj(int? id)
        {
            Film film = null;
            if (!id.HasValue)
            {
                film = new Film();
                ViewBag.Title = "Unos novog filma";
                ViewBag.Novi = true;

            }
            else
            {
                film = bazaPodataka.PopisFilmova.FirstOrDefault(x => x.Id == id);

                if(film == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Title = "Ažuriranje podataka o filmu";
                ViewBag.Novi = false;

            }
            return View(film);

        }

        [HttpPost]
        public ActionResult Azuriraj(Film f)
        {

            if (ModelState.IsValid)
            {
                if (f.Id != 0)
                {
                    bazaPodataka.Entry(f).State = System.Data.Entity.EntityState.Modified;
                }

                else
                {
                    bazaPodataka.PopisFilmova.Add(f);
                }
                bazaPodataka.SaveChanges();
                return RedirectToAction("Popis");
            }
            if(f.Id == 0)
            {
                ViewBag.Title = "Kreiranje filma";
                ViewBag.Novi = true;
            }
            else
            {
                ViewBag.Title = "Ažuriranje podataka o filmu";
                ViewBag.Novi = false;   
            }
            return View(f);
        }

        public ActionResult Brisi(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film f = bazaPodataka.PopisFilmova.FirstOrDefault(x => x.Id == id);
            if (f == null)
            {
                return HttpNotFound();
            }
            return View(f);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Brisi(int id)
        {
            Film f = bazaPodataka.PopisFilmova.FirstOrDefault(x => x.Id == id);
            if (f == null)
            {
                return HttpNotFound();
            }
            bazaPodataka.PopisFilmova.Remove(f);
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