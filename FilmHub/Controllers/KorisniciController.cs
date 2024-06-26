using FilmHub.Data;
using FilmHub.Misc;
using FilmHub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace FilmHub.Controllers
{
    [Authorize(Roles = OvlastiKorisnik.Administrator)]
    public class KorisniciController : Controller
    {
        BazaDbContext bazaPodataka = new BazaDbContext();
        // GET: Korisnici
        public ActionResult Index()
        {
            var listaKorisnika = bazaPodataka.PopisKorisnika.OrderBy(x => x.SifraOvlasti).ThenBy(x => x.Prezime).ToList();
            return View(listaKorisnika);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var korisnik = new Korisnik
                {
                    KorisnickoIme = model.KorisnickoIme,
                    Email = model.Email,
                    Lozinka = Misc.PasswordHelper.IzracunajHash(model.Lozinka),
                    Prezime = model.Prezime,
                    Ime = model.Ime,
                    SifraOvlasti = "KOR" // Pretpostavljamo da su novi korisnici "Moderator"
                };

                bazaPodataka.PopisKorisnika.Add(korisnik);
                bazaPodataka.SaveChanges();
                return RedirectToAction("Prijava");
            }

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Prijava(string returnUrl)
        {
            KorisnikPrijava model = new KorisnikPrijava();
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Prijava(KorisnikPrijava model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var korisnikBaza = bazaPodataka.PopisKorisnika.FirstOrDefault(x => x.KorisnickoIme == model.KorisnickoIme);
                if (korisnikBaza != null)
                {
                    var passwordOK = korisnikBaza.Lozinka == Misc.PasswordHelper.IzracunajHash(model.Lozinka);
                    if (passwordOK)
                    {
                        LogiraniKorisnik prijavljeniKorisnik = new LogiraniKorisnik(korisnikBaza);
                        LogiraniKorisnikSerializeModel serializeModel = new LogiraniKorisnikSerializeModel();
                        serializeModel.CopyFromUser(prijavljeniKorisnik);
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        string korisnickiPodaci = serializer.Serialize(serializeModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, prijavljeniKorisnik.Identity.Name, DateTime.Now, DateTime.Now.AddDays(1), false, korisnickiPodaci);
                        string tickedEncrypted = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, tickedEncrypted);
                        Response.Cookies.Add(cookie);
                        if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Neispravno korisnicko ime ili lozinka");
            return View(model);
        }

        [OverrideAuthorization]
        [Authorize]
        public ActionResult Odjava()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Korisnik korisnik = bazaPodataka.PopisKorisnika.Find(id);
            if (korisnik == null)
            {
                return HttpNotFound();
            }
            return View(korisnik);
        }

        // GET: Korisniks/Create
        public ActionResult Create()
        {
            ViewBag.SifraOvlasti = new SelectList(bazaPodataka.PopisOvlasti, "Sifra", "Naziv");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KorisnickoIme,Email,Lozinka,Prezime,Ime,SifraOvlasti")] Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                bazaPodataka.PopisKorisnika.Add(korisnik);
                bazaPodataka.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SifraOvlasti = new SelectList(bazaPodataka.PopisOvlasti, "Sifra", "Naziv", korisnik.SifraOvlasti);
            return View(korisnik);
        }

        // GET: Korisniks/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Korisnik korisnik = bazaPodataka.PopisKorisnika.Find(id);
            if (korisnik == null)
            {
                return HttpNotFound();
            }
            ViewBag.SifraOvlasti = new SelectList(bazaPodataka.PopisOvlasti, "Sifra", "Naziv", korisnik.SifraOvlasti);
            return View(korisnik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KorisnickoIme,Email,Lozinka,Prezime,Ime,SifraOvlasti")] Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                bazaPodataka.Entry(korisnik).State = EntityState.Modified;
                bazaPodataka.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SifraOvlasti = new SelectList(bazaPodataka.PopisOvlasti, "Sifra", "Naziv", korisnik.SifraOvlasti);
            return View(korisnik);
        }

        // GET: Korisniks/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Korisnik korisnik = bazaPodataka.PopisKorisnika.Find(id);
            if (korisnik == null)
            {
                return HttpNotFound();
            }
            return View(korisnik);
        }

        // POST: Korisniks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Korisnik korisnik = bazaPodataka.PopisKorisnika.Find(id);
            bazaPodataka.PopisKorisnika.Remove(korisnik);
            bazaPodataka.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        


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