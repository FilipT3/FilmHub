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
            var listaKorisnika = bazaPodataka.PopisKorisnika.
                OrderBy(x => x.SifraOvlasti).ThenBy(x => x.Prezime).ToList();
            return View(listaKorisnika);
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
                //dohvaćamo podatke o korisniku po korisničkom imenu
                var korisnikBaza = bazaPodataka.PopisKorisnika.FirstOrDefault(x => x.KorisnickoIme == model.KorisnickoIme);
                //provjeravamo hash lozinke iz baze i izračunati hash na temelju upisane lozinke na login formi
                bool passwordOK = korisnikBaza.Lozinka == Misc.PasswordHelper.IzracunajHash(model.Lozinka);

                if (passwordOK)
                {
                    LogiraniKorisnik prijavljeniKorisnik = new LogiraniKorisnik(korisnikBaza);
                    LogiraniKorisnikSerializeModel serializeModel = new LogiraniKorisnikSerializeModel();
                    serializeModel.CopyFromUser(prijavljeniKorisnik);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string korisnickiPodaci = serializer.Serialize(serializeModel);

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                        1,
                        prijavljeniKorisnik.Identity.Name,
                        DateTime.Now,
                        DateTime.Now.AddDays(1),
                        false,
                        korisnickiPodaci);

                    string ticketEncrypted = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                        ticketEncrypted);
                    Response.Cookies.Add(cookie);

                    //ako postoji url kojem je korisnik prvotno pristupao tada preusmjeravamo na taj url
                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Neispravno korisničko ime ili lozinka");
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
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(Korisnik model)
        {
            if (!String.IsNullOrWhiteSpace(model.KorisnickoIme))
            {
                var korImeZauzeto = bazaPodataka.PopisKorisnika.Any(x => x.KorisnickoIme == model.KorisnickoIme);
                if (korImeZauzeto)
                {
                    ModelState.AddModelError("KorisnickoIme", "Korisničko ime je već zauzeto");
                }
            }
            if (!String.IsNullOrWhiteSpace(model.Email))
            {
                var emailZauzet = bazaPodataka.PopisKorisnika.Any(x => x.Email == model.Email);
                if (emailZauzet)
                {
                    ModelState.AddModelError("Email", "Email je već zauzet");
                }
            }

            if (ModelState.IsValid)
            {
                model.Lozinka = Misc.PasswordHelper.IzracunajHash(model.LozinkaUnos);
                model.SifraOvlasti = "MO";

                bazaPodataka.PopisKorisnika.Add(model);
                bazaPodataka.SaveChanges();

                return View("RegistracijaOk");
            }

            var ovlasti = bazaPodataka.PopisOvlasti.OrderBy(x => x.Naziv).ToList();
            ViewBag.Ovlasti = ovlasti;

            return View(model);
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