using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace FilmHub.Models
{
    public class KorisnikPrijava
    {
        [Display(Name = "Korisničko ime")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        public string KorisnickoIme { get; set; }


        [Display(Name = "Lozinka")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }


    }
}